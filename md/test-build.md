# Test Build

#### Requirements

.NET 4.7 is required on systems prior Windows 10 Version 1703 (Creators Update)

https://www.microsoft.com/en-us/download/details.aspx?id=55170

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### 1.5.0.6 unstable test build

- new: various new x265 options
- new: various new aspect ratio features
- update: x265 2.4+61
- update: QSVEncC 2.66

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
