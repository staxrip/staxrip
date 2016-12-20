# v1.4.0.0-stable (2016-12-12)

#### New Features

- LSFmod added for both AviSynth and VapourSynth, to create the LSFmod default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > AviSynth/VapourSynth Filter Profiles > OK'
- added new commands SetTargetFile and LoadSourceFile
- added setting to define the minimum required disk space and added Continue and Abort option to the message box
- VCEEncC (AMD H.264 encoder) switches added
- SMDegrain added, to create the SMDegrain default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > AviSynth Filter Profiles > OK'
- some x265 changes
- nnedi3 added to VapourSynth, to create the nnedi3 default filter profile go to: 'Filters > Profiles > Restore Defaults > OK' or alternatively: 'Tools > Advanced > Reset Setting > VapourSynth Filter Profiles > OK'
- added setting to reverse mouse wheel video seek direction
- context menu to copy added to media info dialog

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
- license changed to MIT

#### Updated Tools

- LSFmod 1.9
- havsfunc 23
- aWarpSharp2 2016-06-24
- VCEEncC 2.0
- nnedi3 (avs) 0.9.4.31
- VC++ Runtime 2015
- ffms2 2.23
- qaac 2.61
- avs2pipemod 1.1.1
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
- VapourSynth 35
- x265 2.1+69
- MKVToolNix 9.6.0
- NVEncC 3.02
- QSVEncC 2.59

# v1.3.7-stable (2016-06-14)

#### New Features

- added new QSVEncC switches
- added support to use network drive as temp folder
- added yadifmod for VapourSynth. Due to popular request I didn't reset the filter profiles, to make the default filter profile for yadifmod available the filter profiles must be manually reset in the filter profile editor.
- added new command SaveJpgByPath to preview dialog which can be used to customize the menu to save a jpg file to a fixed path without the file browser showing, the path can contain macros.
- added taskbar progress support

#### Fixed Bugs

- fixed the check for enough free space on the disk of the target file, instead of three times the source file one time is now enough
- fixed crash StaxRip checking for enough free space in file batch mode and if target directory is network share
- fixed Umlaute don't work on Windows 7
- fixed many avs/vs scripts are created when the target file name is changed by typing
- fixed feature to run jobs in new instance and changed the behaviour of this feature in that when the jobs complete the instance exits without overwriting the settings

#### Updated Tools

- QSVEncC 2.51
- yadifmod (VapourSynth) r9



# 1.3.6

### New Features

- added setting to prevent the StaxRip window to become the active foreground window if certain applications are currently in the foreground
- added new option to disable audio and subtitle demuxing
- added subtitle formats column to MediaInfo Folder View
- added all missing x265 switches
- added hardware decoding methods to x265 'Other' tab, this will bypass AviSynth though
- added new standalone mkvextract GUI (Apps > Demux)
- added new QSVEncC switches for hard coded subtitles
- added MP4 support to standalone demux app
- added -avsinfo starting AVSMeter without a source opened
- added support for unicode filenames using VapourSynth, AviSynth don't support it
- added mkv cutting support without encoding
- added MSharpen x64 filter for AviSynth+ x64
- added x264 10-Bit support, binary is not included, StaxRip will ask for the location
- added ffmpeg codecs x264, x265 and ProRes
- added codec help menu to ffmpeg options dialog which shows help for the currently selected codec, it displayes the output queried with 'ffmpeg -h encoder=name'
- added option to define preferred audio languages
- added audio and subtitle demuxing modes 'Show Dialog', 'Preferred', 'All', 'None'
- added x265 changes for v1.9 183
- added new option to automatically add hard coded subtitle
- added option to define JPEG compression quality in 'Preview > Save JPG' and thumbnail generator

### Fixed Bugs

- custom switches were missing in the Intel Quick Sync encoding GUI for QSVEncC
- fixed crash with ass file by replacing VSFilter with VSFilterMod

### Tweaks

- the batch audio profile uses now always batch execution, the PATH variable knows the location of ffmpeg, eac3to and BeSweet, the temp files directory is set as current directory, if input files is empty all files are excepted 
- the search feature of the apps dialog searches now also the supported filters of plugins
- enabled audio demuxing using MP4Box for mov files
- removed mkvinfo.exe which is 18 MB large and not really needed. It's large due to QT toolkit being used
- mkv audio demuxing happens now all streams together instead of every stream separate, it's much faster now.
- changed filter profiles editor to support both tab and 4 spaces for multiline profiles, tabs are converted to 4 spaces which is the standard in Python and Visual Studio
- log file improvements
- filters list view did not accept drag and drop with source files 
- enabled posibility to remux mov to mp4
- the apps dialog writes the customized versions now to the settings folder so it don't matter if the startup folder has no write access
- demuxed subtitles are now detected by ID with proper numeric order
- improved Dolby Atmos handling 
- added check if enough disk space is available before a job runs
- replaced drop down menus in Event Commands editor with menu based drop downs

### Updated Tools

- ffmpeg 2016-04-25
- mkvtoolnix 9.2.0
- MP4Box 0.6.2
- mvsfunc 7
- mvtools2 2.7.0.22
- nnedi3 0.9.4.21
- StaxRip Toolbox Demux 1.1
- yadifmod2 0.0.4
- AviSynth 1858
- NVEncC 2.07
- L-SMASH-Works 879
- QSVEncC 2.50
- qaac 2.59
- x265 1.9 200
- AVSMeter 2.2.8
- MediaInfo 0.7.86
- x264 2694