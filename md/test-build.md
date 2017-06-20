# Test Build

#### Requirements

.NET 4.7 is required on systems prior Windows 10 Version 1703 (Creators Update)

https://www.microsoft.com/en-us/download/details.aspx?id=55170

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### (upcoming) 1.5.1.2 unstable test build

- update: NVEnc 3.12
- tweak: the 'Play audio and video' feature in the menu of the audio source files was changed, it still has limitations but shows a message box to tell the user about

#### (online) 1.5.1.1 unstable test build

- new: added option to pre-render slow scripts into a lossless AVI file
- new: improvements on the new x264 GUI introduced in 1.5.0.7
- fix: downmix not used with qaac (regression in 1.5.0.9)

#### 1.5.0.9 unstable test build

- new: to run PowerShell scripts on certain events the Event Command feature is no longer needed, it's documented here: https://github.com/stax76/staxrip#powershell-scripting
- new: there are 2 new options what to do in case the video and audio encoder output files alread exists from a previous job run (reuse, overwrite or ask (default)), the 'Just Mux' video encoder profile does alse reuse the output file from previous job runs, in case it don't exist it uses the source video
- new: there is a new option to define which intermediate format should be used in case the audio encoder don't support the input format, supported is FLAC (default) and W64 (WAV > 4 GB)
- new: there is a new option to define which app should be used to create the intermediate audio file (default is ffmpeg)
- new: the MediaInfo window has 'Developer Mode' in the context menu to show the property names for programmers
- tweak: help improved for: Main Menu > Help > Command Line: There is a special mode where only the MediaInfo window is shown using -mediainfo "inputfile", this is useful for Windows File Explorer integration with an app like Open++.

#### 1.5.0.8 unstable test build

- new: download button added to apps dialog's toolbar, only certain apps have a download URL defined
- new: DGIndex added, this was removed before because there wasn't a x64 d2v source filter, now staxrip has x64 d2v filters for both avisynth and vapoursynth included
- new: improvements on the new x264 GUI introduced in 1.5.0.7
- fix: opening a project from CLI had asked to save the current project even if no changes were made since it was loaded
- fix: wrong channel count used with dolby atmos
- fix: the automatically generated audio profile caption in the main dialog of the current profile gets updated when the source changes
- tweak: improved scaling on 96 DPI (relates to layout changes from 1.5.0.4)
- tweak: improved audio specs parameter display in the main dialog
- tweak: the MediaInfo window is startet as separate process, the window can be minimized and maximized, the layout improved
- update: RgTools 0.96
- update: NVEnc 3.11

#### 1.5.0.7 unstable test build

- new: Deblock VapourSynth plugin added: https://github.com/HomeOfVapourSynthEvolution/VapourSynth-Deblock/
- new: basic new x264 GUI based on the same framework then all other codec GUIs, this will soon replace the old GUI 
- new: support for subtitle cutting
- new: added setting to minimze processing window to task bar instead of the tray area
- new: the options for --sar in the encoder options have now the same dropdown menu then the PAR menus in the main dialog
- tweak: the option Fixed Bitrate was removed, if the bitrate or filesize is fixed depends now on what was edited last
- update: masktools2 (AviSynth+) 2.2.10
- update: mvtools (VapourSynth) 19

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