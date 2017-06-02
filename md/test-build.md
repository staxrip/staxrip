# Test Build

#### Requirements

.NET 4.7 is required on systems prior Windows 10 Version 1703 (Creators Update)

https://www.microsoft.com/en-us/download/details.aspx?id=55170

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### 1.5.0.1 unstable test build

- new: experimental AV1 codec support
- new: the demux app was removed, the built-in demuxing GUIs for mkvextract, mp4box, eac3to and ffmpeg can be used as independent tool found at: Tools > Advanced > Demux
- fix: FLAC was extracted to mka instead of flac
- tweak: staxrip is a bit smarter to find out if all job processing is completed and shutdown/standby can be performed
- update: MP4Box 0.7.2-DEV-rev79 which fixes issues with qsvenc output
- update: MediaInfo 0.7.96
- update: ffmpeg 3.3.1
