# Test Build

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### New Features

- NVEncC added --lookahead --cbrhq --vbrhq --aq-temporal --no-b-adapt --i-adapt --enable-ltr --output-depth --strict-gop --vbr-quality --vpp-gauss --vpp-knn --vpp-pmd
- VCEncC added --check-features --codec --enforce-hrd --filler --fullrange --ltr --pre-analysis --ref --tier --vbaq
- x265 --me sea added
- MPEG2DecPlus filter added to open d2v files with AviSynth+

#### Fixed Bugs

- format 'E-AC3 EX' was unknown to eac3to demuxer
- fixed x265 command line generation for --limit-tu
- added missing check if Visual C++ 2012 is installed when masktools2, SangNom2 or VCEEncC are used

#### Tweaks

- menu item height increased

#### Updated Tools

- x265 2.2+17
- vslsmashsource (vs) 921
- L-SMASH-Works (avs) 921
- RgTools (avs) 0.94
- NVEncC 3.06
- VCEEncC 3.05v2
- QSVEncC 2.62
- mkvtoolnix 9.9.0