# Test Build

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### New Features

- LSFmod added for both AviSynth and VapourSynth, to create the LSFmod default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > AviSynth/VapourSynth Filter Profiles > OK'
- added new commands SetTargetFile and LoadSourceFile
- added setting to define the minimum required disk space and added Continue and Abort option to the message box
- VCEEncC (AMD H.264 encoder) switches added
- SMDegrain added, to create the SMDegrain default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > AviSynth Filter Profiles > OK'
- some x265 changes
- nnedi3 added to VapourSynth, to create the nnedi3 default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > VapourSynth Filter Profiles > OK'
- added setting to reverse mouse wheel video seek direction

#### Fixed Bugs

- fixed C# scripting not working on Windows 7
- fixed bug in audio detection when using a network drive as temp folder
- fixed crash in processing dialog
- fixed bug with automatic generation of audio profile names
- fixed video comparison overwriting the log file
- fixed codepage problem of non western europe locales
- replaced ffmpeg with avs2pipemod for piping to x265 due to a character encoding bug with file names happening on non western locale systems
- fixed various DPI scaling issues

#### Tweaks

- improved audio detection using numeric order instead of alphanumeric order 

#### Updated Tools

- LSFmod 1.9
- havsfunc 23
- aWarpSharp2 2016-06-24
- VCEEncC 2.0
- nnedi3 (avs) 0.9.4.31
- VC++ Runtime 2015
- x265 2.1+25
- ffms2 2.23
- QSVEncC 2.57
- NVEncC 3.01
- MKVToolNix 9.5.0
- qaac 2.61
- avs2pipemod 1.1.1
- VapourSynth 34
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