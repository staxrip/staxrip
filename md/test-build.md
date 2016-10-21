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

#### Fixed Bugs

- fixed C# scripting not working on Windows 7
- fixed bug in audio detection when using a network drive as temp folder
- fixed crash in processing dialog
- fixed bug with automatic generation of audio profile names
- fixed video comparison overwriting the log file
- fixed codepage problem of non western europe locales
- replaced ffmpeg with avs2pipemod for piping to x265 due to a character encoding bug with file names happening on non western locale systems

#### Tweaks

- improved audio detection using numeric order instead of alphanumeric order 

#### Updated Tools

- LSFmod 1.9
- havsfunc 23
- aWarpSharp2 2016-06-24
- VCEEncC 2.0
- MKVToolNix 9.3.1
- QSVEncC 2.54
- nnedi3 (avs) 0.9.4.31
- VC++ Runtime 2015
- AviSynth+ r2290
- VapourSynth R35 RC1
- x265 2.1+25
- ffms2 2.23