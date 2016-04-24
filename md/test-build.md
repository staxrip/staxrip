### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

http://1drv.ms/1OqPDOe

### New Features

- added setting to prevent the StaxRip window to become the active foreground window if certain applications are currently in the foreground
- added new option to disable audio and subtitle demuxing
- added subtitle formats column to MediaInfo Folder View
- added all missing x265 switches
- added hardware decoding methods to x265 'Other' tab, this will bypass AviSynth though
- added new standalone mkvextract GUI (Main Menu > Apps > Demux)
- added new QSVEncC switches for hordcoded subtitles

### Fixed Bugs

- custom switches were missing in the Intel Quick Sync encoding GUI for QSVEncC
- fixed crash with ass file by replacing VSFilter with VSFilterMod

### Tweaks

- the batch audio profile uses now always batch execution, the PATH variable knows the location of ffmpeg, eac3to and BeSweet, the temp files directory is set as current directory, if input files is empty all files are excepted 
- the search feature of the apps dialog searches now also the supported filters of plugins
- enabled audio demuxing using MP4Box for mov files
- removed mkvinfo.exe which is 18 MB large and not really needed. It's large due to QT toolkit being used
- mkv audio demuxing happens now all streams together instead of every stream separate, it's much faster now.

### Updated Tools

- AviSynth 1847
- mkvtoolnix 9.1.0
- QSVEncC 2.46
- MP4Box 0.6.2
- nnedi3 0.9.4.21