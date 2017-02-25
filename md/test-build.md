# Test Build

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### New Features

- MPEG2DecPlus filter added to open d2v files with AviSynth+
- NVEncC added --lookahead --cbrhq --vbrhq --aq-temporal --no-b-adapt --i-adapt --enable-ltr --output-depth --strict-gop --vbr-quality --vpp-gauss --vpp-knn --vpp-pmd
- VCEncC added --check-features --codec --enforce-hrd --filler --fullrange --ltr --pre-analysis --ref --tier --vbaq
- x265 added --me sea --dynamic-rd --scenecut-bias --lookahead-threads --opt-cu-delta-qp --multi-pass-opt-analysis --multi-pass-opt-distortion --multi-pass-opt-rps --aq-motion --ssim-rd --hdr --hdr-opt

#### Fixed Bugs

- format 'E-AC3 EX' was unknown to eac3to demuxer
- fixed x265 command line generation for --limit-tu
- added missing check if Visual C++ 2012 is installed when masktools2, SangNom2 or VCEEncC are used

#### Tweaks

- menu item height increased

#### Updated Tools

- vslsmashsource (vs) 921
- L-SMASH-Works (avs) 921
- RgTools (avs) 0.94
- NVEncC 3.06
- VCEEncC 3.05v2
- QSVEncC 2.62
- mkvtoolnix 9.9.0
- x265 2.3+8
- masktools2 2.2.2