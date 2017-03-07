# Test Build

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### New Features

- MPEG2DecPlus filter added to open d2v files with AviSynth+
- NVEncC added --lookahead --cbrhq --vbrhq --aq-temporal --no-b-adapt --i-adapt --enable-ltr --output-depth --strict-gop --vbr-quality --vpp-gauss --vpp-knn --vpp-pmd --device
- VCEncC added --check-features --codec --enforce-hrd --filler --fullrange --ltr --pre-analysis --ref --tier --vbaq
- x265 added --me sea --dynamic-rd --scenecut-bias --lookahead-threads --opt-cu-delta-qp --multi-pass-opt-analysis --multi-pass-opt-distortion --multi-pass-opt-rps --aq-motion --ssim-rd --hdr --hdr-opt
- icons added to menus and menu editor
- QSVEncC added --profile main10
- colour_primaries added to MediaInfo Folder View

#### Fixed Bugs

- format 'E-AC3 EX' was unknown to eac3to demuxer
- fixed x265 command line generation for --limit-tu
- added missing check if Visual C++ 2012 is installed when masktools2, SangNom2 or VCEEncC are used
- nnedi3 wasn't loaded using avs nnedi3_rpow2
- in the scripting editor it was often needed to right-click a second time until the context menu showed
- fixed incompatible format like wmv being passed to mkvmerge and mp4box
- KNLMeansCL wasn't loaded for HAvsFunc/SMDegrain 

#### Tweaks

- menu item height increased
- added colorspace = "YV12" default everywhere FFVideoSource is used to open 10Bit sources, profiles have not been reset
- antialiased font rendering added in some places

#### Updated Tools

- RgTools (avs) 0.94
- NVEncC 3.06
- QSVEncC 2.62
- mkvtoolnix 9.9.0
- x265 2.3+8
- masktools2 2.2.2
- ffms2 2.23.1
- VCEEncC 3.06
- HAvsFunc 2017-03-06
- x264 2762
- MediaInfo 0.7.93
- ffmpeg 3.2.2
- qaac 2.62
- L-SMASH-Works (avs) 929
- vslsmashsource (vs) 929
- KNLMeansCL 1.0.2