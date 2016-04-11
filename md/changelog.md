### Download

https://drive.google.com/open?id=0B-gPKiJYuKuITld4dzhuTC1WWWM

http://1drv.ms/1OqPDOe

### New Features

- support for 3 or more audio tracks added to the muxing dialog
- code preview added to filters menu and script preview renamed to code preview in script editor
- added a command line switch -show-dialogs:bool and project option 'Show all dialogs when invoked from CLI', by default dialogs are not shown when StaxRip receives CLI arguments
- yadifmod2 plugin for AviSynth added
- TComb AviSynth plugin added
- auto crop plugin replaced with new internal routine supporting both AviSynth and VapourSynth
- better subtitle support for m2ts container
- decoding options added for AMD encoder
- larger x265 custom command line TextBox
- x265 new switch added --rc-grain
- the encoding options dialogs for x265, AMD, Intel and NVIDIA have now an option to display the full command line
- new ffmpeg encoder output options 'H.264 NVIDIA', 'H.264 Intel', 'H.265 Intel', the currently used ffmpeg built doesn't yet support NVIDIA
- video encoder profiles added for all encoding codecs StaxRip supports for ffmpeg: 'VP9', 'Xvid', 'ASP', 'Theora', 'H.264 Intel', 'H.265 Intel', 'H.264 NVIDIA'
- ffmpeg decoding option -threads added, multi-threading is enabled by default but not recommended for dxva2 decoding and maybe for hardware decoding in general
- VapourSynth filter profiles added for mvsfunc functions AssumeFrame, AssumeTFF, AssumeBFF, AssumeCombed
- VapourSynth QTGMC parameters TFF and Preset added to menu
- more source and target parameters are now shown in the main dialog
- in the scripting editor there is a Join feature to join all filters into one
- QTGMC 3.33 for AviSynth
- profiles can be saved from the x265/AMD/Intel/NVIDIA/ffmpeg encoding options dialogs
- added shortcuts and Export to CSV to MediaInfo Folder View (Tools > Advanced)
- added C# (Roslyn) and PowerShell scripting support. Scripts can be started at main menu > Tools > Scripts. There are commands in the command line, main menu and event commands to execute script code or script files.
- three VapourSynth filter profiles added based on std.SetFieldBased
- added user contributed script 'Convert Bluray (BT709) to 10bit 400nits (MaxFALL) HDR (rev2)'
- in the options dialog the subtitles to auto load can now be defined with both two and three letter code, this is useful for people more familiar with three letter codes, both can be mixed, for undetermined previously 'iv' had to be used, now 'und' can also be used.
- many new switches added to QSVEncC Intel Quick Sync GUI
- added setting to scale the UI to a custom factor

### Fixed Bugs

- fixed window freeze in eac3to dialog happening when eac3to fails on the input.
- fixed showing filter selection dialog even if the source is vpy or avs
- fixed StaxRip trying to auto correct the script even if the source is avs
- fixed StaxRip recalculating the video bitrate even when the fixed bitrate option is used
- avi muxer supports two audio tracks now
- in some cases the two audio output file paths were identical 
- moving subtitles up and down wasn't working correctly
- the new mp4box update should fix a bug many people reported
- fixed editing dates in apps dialog not working with Persian calendar
- fixed crash using ' character in file paths using VapourSynth
- fixed inaccurate right and bottom magnifiers in crop dialog
- removed one x265 switch that was accidentally added twice :-)
- fixed Python installed by miniconda not being detected
- cropping values were multiplied by 2 using NVEncC
- fixed subtitle size detection
- fixed preview not fitting on screen under some conditions
- fixed some issues opening avs or vpy as source file
- added new source filter avisource.AVISource for VapourSynth, it can open avi and avs files
- the filter selection dialog has now automatic options for both AviSynth and VapourSynth. For avs sources it shows also an option for VapourSynth since VapourSynth's AVISource can open avs files
- d2vsource added to VapourSynth
- fixed wrong font used pasting code in code editor
- in the media info folder view (Tools > Advanced) the context menu functionality did not work for sub folders
- in same situations it wasn't possible to close the crop dialog due to an script error, now the message box showing the error has an additional exit button
- fixed High DPI scaling bug in the navigation bar of the x265 dialog
- in case the subtitle source file is mkv and has a title but the title was deleted in StaxRip it was still used
- line indentation wasn't preserved for multiline filter profiles

### Tweaks

- better error handling when the source is vpy or avs
- when loading an audio file without language info the language of the audio profile/stream stays instead of being changed to undetermined
- improved changelog
- cutting with VapourSynth is now possible without AviSynth being installed (the audio cutting code was using AviSynth)
- The info in the preview looks better and is readable regardless of the current background
- source filter preferences in the settings dialog under Source Filter to configure which source filter should be used for a given file in case the source filter in the project is called Automatic is now also available for VapourSynth, with this VapourSynth support in StaxRip should be on par with AviSynth support making both scripting engines first class citizens
- x265 tune grain defaults updated
- improved bitrate calculation
- The media info dialog shows the file size now also in mega byte instead of giga byte only
- removed ffmpeg writing long build info to the log file
- Video Comparison shows now indexing progress
- added version or at least change date and x86 or x64 to every app in the apps dialog
- set font to Consolas 10 in scripting editor
- the checked list controls of the event command and jobs dialog were replaced with themed controls, in the jobs dialog multi select and Ctrl+A and Delete shortcuts are supported
- besides detecting the python location of the regular setup and miniconda setup StaxRip now also searches in the PATH and Path environment variables for python

### Updated Tools

- AviSynth+ 1841
- AVSMeter 2.1.7
- ffmpeg 2016-03-10
- fmtconv (vs) 18
- mkvtoolnix 9.0.1
- mp4box 0.6.0
- mvtools (vs) v13
- NVEncC 2.04
- QSVEncC 2.44
- x264 2665
- x265 1.9+125
- yadifmod2 (avs) 0.0.3