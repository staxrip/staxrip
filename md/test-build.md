# Test Build

#### Requirements

.NET 4.7 is required on systems prior Windows 10 Version 1703 (Creators Update)

https://www.microsoft.com/en-us/download/details.aspx?id=55170

#### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

https://onedrive.live.com/redir?resid=604D4754F64B0ABC!4140&authkey=!ANUm9V3vTPmEFNI&ithint=folder%2c7z

#### 1.5.1.5 unstable test build

- new: parallel audio processing 
- new: added setting to define how many projects to keep under: 'Main Menu > Project > Recent', the setting is located at: Main Menu > Tools > Settings > General 
- new: added new thumbnail generation options
- new: added new feature to archive log files in the settings directory, by default the last 50 log files are keept, this number can be customized at: 'Main Menu > Tools > Settings > General', the folder can be opened with the windows file explorer at: Main Menu > Tools > Folders > Log Files
- new: qsvenc options --fade-detect and --repartition-check

- fix: wrong fps display in main form for 50/60 fps
- fix: version detection disabled for all VC++ runtimes because of issues with file projection via hardlinks and WINSXS
- fix: two issues in x264 command line generation

- change: renamed and reseted setting to minimize to tray
- change: nvenc vpp deband support improved
- change: Option to delete temp files moved to: Tools > Settings > System
- change: scaling and layout improved for numeric up down control and custom menu editor
- change: code refactoring of dynamically generated dialogs (settings/options/codecs), this has let to changed data types and a reset of video encoder profiles
- change: the mkv title tag in the container options isn't overwritten by a title tag of the source file when the source is loaded

- update: mkvtoolnix 13.0.0.0
- update: qsvenc 2.70
- update: AviSynth+ 2508
- update: x265 2.4+89

#### 1.5.1.4 unstable test build

- new: the new x264 GUI is complete, the old is gone
- new: nvenc option --vpp-deband added
- new: x265 option --[no-]refine-vbv added
- new: added new menu to the PAR/DAR option in the container dialog
- fix: in some circumstances file paths with parenthesis failed to process (regression in 1.5.1.1)
- fix: right-click help didn't navigate to help URL on string options in the x265 and the new x264 dialog
- fix: audio settings/profile display was wrong generated if the audio source file has multiple audio streams
- fix: audio was converted to FLAC/W64 even if the output already existed and processing should be skipped (refers to a new feature added with 1.5.0.9)
- change: medium crf value was changed from 22 to 20 for x264 and x265
- update: x265 2.4+87
- update: nvenc 3.13

#### 1.5.1.2 unstable test build

- new: many improvements on the new x264 GUI introduced in 1.5.0.7, video profile settings were reset because at the same time I improve the x265 GUI, the video profile settings will be reset again until the GUI is finished in 1-2 weeks, the old GUI will then be removed
- fix: the feature to reuse existing audio output files from previous job runs had a bug that would result in the wrong file muxed
- fix: the automatic name generation for the audio settings wasn't always working correctly
- tweak: the 'Play audio and video' feature in the menu of the audio source files was changed, it still has limitations but shows a message box to tell the user about
- tweak: in case the option to delete the temp files is used the log file is copied to the target folder
- update: NVEnc 3.12
- update: 2.4+75

#### 1.5.1.1 unstable test build

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