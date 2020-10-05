
2.1.4.8 Beta (2020-09-??)
============

- The Apps dialog supports drag & drop for zip/7z files,
  mostly usefull for the tool update maintainer (stax76)
- qaac 2.71 (long path support)
- chapterEditor 1.24
- mkvtoolnix 51


2.1.4.7 Beta (2020-10-02)
============

- Fix Audio decoding method *Pipe* causes crash (Dendraspis, #340)
- Fix parameters when audio converting to AAC with FFmpeg (Dendraspis, Patman86, #341)
- 8.3 filepath fix
- Improved Opus encoder (Patman86)
- Apps dialog has a experimental Auto Update feature (stax76)
- NeroAAC removal
- Option to disable MKV subtitle compression (stax76, [#334](/../../issues/334))
- AddGrainC 1.8.2
- VCEEnc 6.05
- L-Smash-Works 2020-07-28


2.1.4.6 Beta (2020-09-13)
============

- Fix eac3to AAC quality mode incorrect bitrate estimation (stax76)
- Fix error at: Main Menu > Tools > Settings > Source Filters (stax76, #322)
- ffmpeg fdk-aac has a option and is disabled by default (stax76, #337)
- Main window focus is supressed after processing in case the last
  window activation is older than 60 seconds (stax76, #333, #298)
- fdkaac removed due to non-free license (stax76, [#292](/../../issues/292))


2.1.4.5 Beta (2020-09-13)
============

- New configuration section in log files, displaying template, profile, muxer, ... - can be extended anytime (Dendraspis, #331)
- eac3to AAC encoding gets correct default quality value and limits (Dendraspis, #328)
- Fixed x265 Preset default value for LookaheadSlices (Dendraspis, (112e150))
- Update checker respects runtime architecture (x64/x86) (Dendraspis)
- MKVToolNix 50 long path support (stax76, #226)
- The wiki was integrated into the tool help. Anybody can add
  detailed tool info and access it from StaxRip (stax76, #292, #320)
- The automatic source filter detection supports detection by format,
  the defaults were reset and use now L-Smash-Works for VP9 (stax76, #312)
- Demuxing is skipped if output files already exist, the old recreate behaviour can be restored
  in the preprocessing settings. If demuxing is canceled then unfinished files are deleted (stax76)
- Fix L-Smash-Works indexing happening multiple times (stax76)
- ffms2 0055b2d StvG


2.1.4.4 Beta (2020-09-06)
============

- eac3to demux dialog showed incorrect language default selection (stax76, #318)
- MP4Box fail to demux OGG chapters (44vince44, #310)
- MP4Box muxer supports chunks encoded with x265 chunk encoding (stax76, #316)
- MP4Box 1.0.0-rev211-g71f1d75ea-x64-gcc10.2.0 (Patman86, #310)


2.1.4.3 Beta
============

- ffmpeg uses libfdk_aac as AAC encoder (stax76, #292)
- Long path support (stax76, #226, experimental)
- fix Decoder ffmpeg DXVA2 (stax76, #290)
- ffms2 2.40-RC1
- MediaInfo.NET 6.6 (long path support)
- AviSynth+ 3.6.2 test 2 (long path support)
- VapourSynth R52


2.1.4.2 Beta
============

- Fix incorrect output mod warning shown.
- Fix incorrect syntax for nvenc/qsvenc --avsdll in avisynth portable mode.


2.1.4.1 Beta
============

- New Presets for NVEnc (P1-P7) added. Reference: https://github.com/rigaya/NVEnc/blob/master/NVEncC_Options.en.md#-u---preset
- Soft links are only created if needed by the current config,
  there is now a confirm dialog with shield icon,
  mpv.net, NVEnc and QSVEnc no longer need soft links.
- New NVEnc options --multipass --repeat-headers.
- Fix mux audio profile Extract DTS Core not working for mka files.
- x264 r3018-db0d417 x86
- mkvtoolnix 49 x64
- NVEnc 5.14
- QSVEnc 4.07
- VCEEnc 6.02
- SVT-AV1 0.8.4-26-g0af191de-msvc1926 Patman
- MP4Box 1.0.0-rev188-g2aa266dfa-gcc10.2.0 Patman
- qaac 2.69-msvc1926 Patman
- ffmpeg N-98647-gffa6072fc7-gcc10.2.0 Patman
- QTGMC 3.368s


2.1.3.9 Beta
============

- Optional mode added to use AviSynth/VapourSynth via VFW.
- All VC++ x64 binaries from abbodi1406/vcredist added.
- Danger Zone setting to disable tool verifications.
- Setting to disable manual plugin loading.
- Experimental 32 bit support.
- VapourSynth R51
- NVEnc 5.13


2.1.3.8 Beta
============

- mkvtoolnix 48
- ec3 file extension support for eac3 files.
- R210/V210 video output using ffmpeg.
- Include beta versions for update checker and show changelog (Dendraspis)
- The final assistant tip supports SHIFT key to add the job at the top of the job list and
  CTRL to prevent showing the jobs dialog. Right-click shows a menu. (Dendraspis)
- Allow to open video files with relative paths on the command line (Dendraspis)
- Create F6 shortcut for Jobs button on Processing dialog (Dendraspis)
- Create F7 shortcut for Log dialog
- 4 commands added to open each source type separately (Dendraspis)
- x265 Presets and Tunes fixes (Dendraspis)
- The Processing dialog has a feature to skip the currently running job (Dendraspis)
- Confirmation on exceptions before files/windows are opened (Dendraspis)
- A new issue template for usage questions was created on the issue tracker and
  the docs were updated to describe that the issue tracker can be used for usage questions.
- For ffmpeg h264_nvenc the command line option generated for quality
  was using -q:v instead of -cq.
- Pass --demuxer-lavf-format=vapoursynth to mpv.net in case the source is vpy.


2.1.3.7 Beta
============

- AviSynth soft link fix.
- DGIndex re-enabled by default for VOB/MPG.


2.1.3.6 Beta
============

- Incorrect command line using VapourSynth and x265.


2.1.3.5 Beta
============

- x264 encoder by default uses avs input.
- x264 encoder has pipe options for raw format.
- Flicker in Video Comparison fixed.
- New command AddBatchJob.
- AviSynth ImageSource support.
- x264 M-0.160.3009-4c9b076-gcc11 Patman
- QSVEnc 4.04
- VCEEnc 6.02
- AVSMeter 3.0.2.0


2.1.3.4 Beta
============

- New setting to use AviSynth portable even when AviSynth
  is installed, portable mode is enabled by default.
- ffmpeg EAC3 encoding support.
- Default AAC encoder changed from eac3to to qaac.
- In batch mode script error handling was fixed.
- ffmpeg muxed audio even for No Muxing profile.
- AviSynth 3.6.1
- ffmpeg N-98276-g1d5d8a30b4-g842bc312ad-gcc10.1.0 Patman
- MP4Box 1.0.0-rev16-g10dd6533a-gcc11.0.0 Patman
- SVT-AV1 0.8.3-57-gba72bc85-msvc1926 Patman
- qaac 2.69
- Bwdif r2


2.1.3.2 Beta
============

- AviSynth/VapourSynth plugin auto load folder support in portable mode (Main Menu > Folders > Plugins)
- ffmpeg muxer uses always -strict -2, it enables TrueHD in MP4
- x264 M-0.160.3009-4c9b076-gcc11 Patman
- NVEnc 5.06
- MediaInfo.NET 6.5.0.0
- Subtitle Edit 3.5.1.5
- TMM2 0.1.4
- AviSynthShader 1.6.6


2.1.3.1 Beta
============

- Main dialog target image width and height text box supports mouse wheel.
- Subtitle file drag & drop in muxer dialog was broken.
- TemporalDegrain2 integration.
- AviSynth 3.6.1 Test 8 (fixes issue with VSFilterMod).
- The docs explain how to use the filter profile editor to edit filter profiles and
  how to create new custom filter profiles for plugins not yet included in StaxRip.
- Video demuxing defined in Preprocessing settings did not work in auto mode.


2.1.3.0
=======

- Processing dialog fix for new Stop After Current Job feature.
- Web URL is included in the search of the Apps dialog.
- Numerous bugs fixed.
- The Processing dialog has a new feature: Stop After Current Job.
- D2V Witch added and enabled by default for VOB/MPG.
- Command line for some processes like ffmsindex wasn't shown while processing.
- Render, scaling and layout fixes and improvements, especially for 96 DPI.
- PowerShell script host supports events, script examples use events, better error handling.
- Improved Log File Viewer (Main Menu > Tools > Log File).
  Tip: The Log File Viewer has a context menu.
- The Preview dialog can be resized with the mouse.
- Support for character # in filenames because MP4Box was finally fixed.
- Various dialogs made resizable and remember their size.
- Maximum number of parallel processes increased from 4 to 16.
- The documentation was greatly improved (still far from perfect though).
- Muxer dialog supports Drag & Drop for subtitles, audio and attachments.
- Video Comparison has hardware render support added.
- aomenc.exe GUI re-enabled.
- Dark color theme for built-in help.
- Improved built-in F1 help.
- ffmpeg video encoder codec FFV1.
- x264 and x265 dialogs have a new Bitrate option, the default value is 0 which
  means the bitrate of the project/template in the main dialog is used.
- For file batch jobs only the file name is shown in the jobs dialog and not the full path.
- Audio encoder supports extracting DTS core using ffmpeg.
- The audio Copy/Mux profile has a Extract DTS Core feature.
- The command line audio encoder has a Default and Forced option.
- New chunk encoding feature for x265 parallel processing.
- Media info dialog replaced with MediaInfo.NET.
- The command line video and audio encoder uses cmd.exe directly without creating a bat file,
  this avoids creating a temporary bat file and adds full unicode support.
- Portable support added, no need to install anything.
- Setting to allow to use tools with wrong version,
  for this a Danger Zone tab was added in the settings.
- The auto crop feature shows progress both in the processing dialog and in the crop dialog.
- Improved issue templates on the github issue tracker.
- The ExecuteCommandLine command has a new Working Directory parameter.
- The launch button in the Apps dialog for a console tool shows its help via Windows Terminal
- Windows Terminal available in the main menu with special StaxRip environment (apps and macros).
- The video encoder dialog feature *Show Command Line* is shown using Windows Terminal.
- Execute Command Line in video encoder dialogs is shown via Windows Terminal.
- In the Apps dialog the tools can be listed using PowerShell Out-GridView.
- Shell Execute flag was added to the command ExecuteCommandLine.
- The global setting 'Add filter to convert chroma subsampling to 4:2:0'
  uses now ConvertToYUV420 instead of ConverttoYV12.
- In various command line features the path environment variable of the process has all exe tools
  added and all macros are available as environment variables.
- Check added that blocks source files with too long path or filename.
  A setting that allows to change the limit exists in the Danger Zone section.
- SVT-AV1 support with GUI.
- Media info folder view was replaced with a new powershell based (Get-MediaInfo)
  dialog that supports caching for fast startup perforance.
- When a Event Command executes it writes a log entry, this is now disabled by default
  but there is a new setting: 'Write Event Commands to log file'.
- In the Jobs dialog there is a button that shows a menu, this menu can now also be shown
  as context menu via right-click on the jobs list and it has various new features.
- The Apps dialog allows to clear custom paths.
- The Apps dialog allows to locate files via Everything.
- Check for Updates added to main menu in Help section.
- Version is shown in main dialog title bar.
- 'Main Menu > Help > Info' shows list with contributors.


2.1.3.0
=======

- processing dialog fix for new Stop After Current Job feature
- x264 M-0.160.3000-33f9e14-gcc10.0.1 Patman
- Web URL is included in the search of the Apps dialog


2.1.2.2 Beta
============

- The Processing dialog has a new feature: Stop After Current Job.
- GPL licensed DGIndex binary re-added.
- D2V Witch added and enabled by default for VOB/MPG.
- Command line for some processes like ffmsindex wasn't shown while processing.
- DFTTest VS re-added to fix dependency issue.
- MCTemporalDenoise AVS dependency issue fix.
- Render, scaling and layout fixes and improvements.


2.1.1.9 Beta
============

- The cut feature was missing the last frame.
- In case the cut feature is used, flac input for qaac is disabled
  so if necessary a w64 file is created.
- Many nvenc improvements.
- Layout and scaling fixes.
- PowerShell script host supports events, script examples use events.
- The menu in the Jobs dialog was improved.
- mvtools2 2.7.43


2.1.1.8 Beta
============

- Improved Log File Viewer (Main Menu > Tools > Log File).
  Tip: The Log File Viewer has a context menu.
- The Jobs dialog has a feature to sort the job list alphabetically.
- The Preview dialog can be resized with the mouse.
- Support for character # in filenames because MP4Box was finally fixed.
- Improved PowerShell scripting error handling.
- Many UI improvements, especially on 96 DPI.
- 'Main Menu > Help > Info' shows list with contributors I could remember.
- x265 M-3.4+6-g73f96ff39-gcc11.0.0 Patman
- nvenc 5.03
- qsvenc 4.03
- mkvtoolnix 47
- AVSMeter 3.0.0.4
- chapterEditor 1.23
- f3kdb Neo r6
- MiniDeen Neo r9
- DFTTest Neo r7
- L-Smash-Works 2020-05-31
- JPSDR 3.2.2
- mvtools2 2.7.42
- VSFilterMod 5.2.2
- TIVTC 1.0.17
- TDeint 1.5
- SMDegrain 3.12.108s
- QTGMC 3.365
- LSFmod 2.187


2.1.1.7 Beta
============

- various dialogs made resizable and remember their size
- various UI issues fixed
- the play feature in the muxer dialog for subtitles was fixed
- flash3kyuu_deband replaced by Neo f3kdb


2.1.1.6 Beta
============

- muxer dialog size issue
- low DPI menu font render issue


2.1.1.5 Beta
============

- In the video encoder dialogs the feature *Execute Command Line* failed when
  Windows Terminal is installed and paths contained spaces.
- The video encoder dialog feature *Show Command Line* is shown using a console.
- Maximum number of parallel processes increased from 4 to 16.
- On systems with restricted PowerShell execution policy several features
  like the media info folder view feature failed.
- The Tools page in the docs lists all tools in a grid view
  with columns: Name, Type, Filename, Version, Modified Date.
- Muxer dialog is resizable and remembers the size.
- Muxer dialog supports Drag & Drop for subtitles, audio and attachments.
- Video Comparison crash fixed and hardware render support added.
- aomenc.exe GUI re-enabled, executable not included
- AviSynth headers updated.
- AviSynth 3.6
- x265 M-3.3+31-g431a22e82-gcc11.0.0 Patman
- NVEnc 5.02
- ffmpeg N-97868-gaa6f38c298-g38490cbeb3-gcc10.1.0 Patman
- yadifmod2 avs 0.2.2
- AVSMeter 2.9.9.3
- KNLMeansCL avs/vs 1.1.1
- MPEG2DecPlus avs 0.1.2 (untested)
- QTGMC avs 3.364s
- masktools2 2.2.23
- DCTFilter 0.5.1
- MediaInfo.NET 6.4.0.0


2.1.1.4 Beta
============

- dark color theme for built-in help.
- improved built-in help for various dialogs.
- various docs pages improved.
- ffmpeg video encoder codec FFV1.
- x264 and x265 dialogs have a new Bitrate option, the default value is 0 which
  means the bitrate of the project/template in the main dialog is used,
  this behavior is documented in the F1 help of the x264 and x265 dialog.
- for file batch jobs only the file name is shown in the jobs dialog and not the full path.
- audio encoder supports extracting DTS core using ffmpeg, generated name is _Extract DTS Core_.
- the audio Copy/Mux profile has a _Extract DTS Core_ feature.
- the command line audio encoder has a Default and Forced option.


2.1.1.3 Beta
============

- fix avs and vpy import adding unnecessary LoadPlugin and Import calls
- new setting: 'Main Menu > Tools > Settigs > System > Use included portable VapourSynth',
  to force usage of included portable VapourSynth instead of installed VapourSynth
- new chunk encoding feature for x265 parallel processing
- DGDecNV removal, there are better open source tools,
  L-Smash-Source supports hardware decoding for NVIDIA, Intel and AMD


2.1.1.2 Beta
============

- if AviSynth or VapourSynth was installed then StaxRip will use
  the installed version instead of the included portable version.
- fix x265 --limit-modes issue.
- fix batch encoding issue.
- mpv.net 5.4.8.0
- VapourSynth r50
- x265 3.3+27-g4780a8d99-gcc11.0.0 Patman
- nvenc 5.01
- mkvtoolnix 46
- RgTools 1.0
- AVSMeter 2.9.9.1
- new docs page Features giving a comprehensive feature list


2.1.1.1 Beta
============

- mpv.net 5.4.6.0
- MediaInfo.NET 6.3.0.0
- x265 3.3+25-ga6489d2fb-gcc10.0.1 Patman
- fix invalid --vpp-tweak nvenc command line generation (Patman86)
- fix import of invalid color metadata into encoder VUI settings (Patman86)
- fix taskbar indication of values below 1 (Dendraspis)
- fix x265 preset (Dendraspis)
- fix crash doing multi select drag operation in jobs dialog
- fix showing the Apps dialog for a tool with non OK status in endless loop
- the command line audio encoder uses cmd.exe directly without creating a bat file,
  this avoids creating a temporary bat file and adds full unicode support
- portable support added and enabled by default, no need to install anything
- nvenc --vpp-transform
- setting to allow to use tools with wrong version,
  for this a Danger Zone tab was added in the settings


2.1.0.9 Beta
============

- detection of Python location improved
- VapourSynth works even if Python nor VapourSynth are in path environment variable
- new x265, NVEnc and SVT-AV1 switches added
- fixed GUI being hidden while auto crop
- DGIndex disabled by default
- mpv.net 5.4.4.3
- SVT-AV1 0.8.2
- NVEnc 5.0
- VCEEnc 6.0
- MP4Box 0.9.0-DEV-rev0-g81b4481e1-gcc10.0.1 Patman
- x265 3.3+19-gcaf9d4dbe-gcc10.0.1 Patman
- ffmpeg N-97384-gcc9ba91bec-g4457f75c65-gcc9.3.0 Patman


2.1.0.8 Beta "The Power Of Walking Away"
========================================

new
---

- VapourSynth filter spline64 added (Patman)
- DG* tools binaries and urls removed according to author request


2.1.0.7 Beta
============

new
---

- a new documentation page [Commands](https://staxrip.readthedocs.io/commands.html) was created.
- the built-in MediaInfo GUI was replaced with MediaInfo.NET which was ported to .NET Framework 4.8.
- the MediaInfo folder view powered by Get-MediaInfo.ps1 v3.0 is shown
  without starting a terminal and it has few bugs fixed.
- the issue templates on the [github issue tracker](https://github.com/staxrip/staxrip/issues/new/choose) were improved.
- `Main Menu > Tools > Advanced > Command Prompt` can be configured in
  the menu editor because it's based on the ExecuteCommandLine command.
  Only people who reset or manually config the main menu will see the change.
- the ExecuteCommandLine command has a new Working Directory parameter.


fix
---

- issue causing audio to be silently ignored instead of muxed
- since v2.1.0.5 DGDecNV usage triggered stack overflow causing staxrip to die silently


2.1.0.6 Beta
============

new
---

- the launch button in the Apps dialog for a console tool shows its help via Windows Terminal
- Main Menu > Tools > Advanced > Command Prompt and PowerShell are shown via Windows Terminal
- Execute Command Line in video encoder dialogs is shown via Windows Terminal
- in the Apps dialog the tools can be shown using PowerShell Out-GridView
- one time tip messages added to inform about otherwise unknown functionality
- a Shell Execute flag was added to the command [ExecuteCommandLine](https://staxrip.readthedocs.io/cli.html#cmdoption-executecommandline-commandline-waitforexit-showprocesswindow-useshellexecute), this is a setting in the
  process class of dotnet, it causes the usage of the ShellExecute Win32 API instead of
  CreateProcess, this APIs have different feature sets like CreateProcess accepting only exe
  files and being able to pass in environment variables and use redirection
- the [Command Line Interface doc page](https://staxrip.readthedocs.io/cli.html) was significantly improved


fix
---

- CSV file creation possibly incompatible with Excel in certain locales


2.1.0.5 Beta
============

new
---

- the global setting 'Add filter to convert chroma subsampling to 4:2:0'
  uses now ConvertToYUV420 instead of ConverttoYV12
- in custom command line based demuxers and in the ExecuteCommandLine
  command the path environment variable of the process has all exe tools
  added and all macros are available as environment variables
- AviSynth filter profile using ColorYUV function


update
------

- mkvtoolnix 45.0.0
- nvenc 4.69
- DFTTest 1.9.5 Clang
- L-Smash-Works 2020-03-22 HolyWu
- ffms2 89bd1e1 StvG

fix
---

- the MediaInfo Folder view did not work unless Get-MediaInfo was installed,
  now it should work even without being installed, please try and give feedback


2.1.0.3 Beta
------------

- new: check added that blocks source files with too long path or filename. A setting
       that allows to change the limit exists and there is also a explanation as tooltip:
       In theory Windows supports paths that are longer than 260 characters, in reality
       neither Windows, nor the .NET Framework or the used tools have full long path support.
- new: in order to support unicode the command line based encoder used by XviD uses
       now cmd.exe directly without creating a batch file. This command line based encoder
       is not only useful for XviD but can be used for any command line based encoder
- new: very basic SVT-AV1 encoder support added

- update: MediaInfo 20.03
- update: AviSynth 3.5.1
- update: ffmpeg N-97107-g33c106d411-g72be5d4661+2-gcc9.3.0 Patman

- fix: 2 reported typos

### 2.1.0.2 Beta

- new: the MediaInfo folder view was replaced with a new powershell based
       dialog that supports caching for fast startup perforance

- fix: install instructions for wrong versions in the Apps dialog were improved
- fix: UNC path issue fix (AMED)
- fix: x265 --hdr-opt renamed to --hdr10-opt

- update: x265 3.3+10-g08d895bb6-gcc9.3.0 Patman
- update: Python 3.8.2
- update: VapourSynth R49
- update: VC++ 2019 14.25.28508.3

### 2.1.0.1 Beta

- fix: MediaInfo Folder View was broken and main window
       did not show the video codec, both had the same reason

- fix: Change default values of Early Skip and Psy-RDOQ
       for various presets (tnatiuk17piano)

- new: when a Event Command executes it writes a log entry,
       this is now disabled by default but there is
       a new setting: 'Write Event Commands to log file'

- new: in the Jobs dialog there is a button that shows a menu,
       this menu can now also be shown as context menu via right-click
       on the jobs list and it has a new menu item to check the
       selected jobs

### 2.0.9.12 Beta without apps

- fix: play issue in prieview

### 2.0.9.11 Beta without apps

- fix: the app verification reported that a old tool version
       was found when actually it was a new version

- new: the Apps dialog allows to clear custom paths
- new: the Apps dialog allows to locate files via Everything

### 2.0.9.10 Beta without apps

- fix: issue with network paths

### 2.0.9.1 Beta

- new: Check for Updates added to main menu in Help section

- fix: QTGMC did not work on systems without AVX2
- fix: Window position of the main dialog was not remembered
- fix: FrameServer.dll again released as debug build because
       of a bug in the new release script

### 2.0.8.12 Beta without apps

- fix: external avs/vpy scripts that use relative instead of
       absolute paths failed to load

### 2.0.8.11 Beta without apps

- new: Version is shown in main dialog title bar

- fix: event command issue
- fix: layout and usability issue in audio dialog (Patman)

### 2.0.8.0 Stable

- fix: using Event Command After Encoded with SetTargetFile
       caused exception
- fix: if the default code page is UTF8 then staxrip
       don't warn about avisynth not supporting Unicode

- update: mkvtoolnix 44

### 2.0.7.7 Beta

- new: the apps management and verification has been improved

- fix: usage of too old VC++ 2019 runtime is now prevented

- update: qsvenc 3.33
- update: nvenc 4.68
- update: vceenc 5.04
- update: MiniDeen r6

### 2.0.7.6

- fix: format compatibility was improved with an automated test
- fix: in the preview dialog the start position for mpc
       and mpv was incorrect when cut ranges were active
- fix: chapter file not being picked up
- fix: in the filter setup profiles dialog a exception could
       happen and loading a filter setup from the menu could fail
- fix: the filter parameter menu in the code editor was adding
       parameters even if they were already existing with different casing
- fix: cutting could cause an error where staxrip generates
       a chapter file without containing a ChapterAtom

- new: the logic to create and edit cut sections in the preview
       dialog is now smarter and more flexible       
- new: the assistant tells in case of a script error that the
       full error can be shown by clicking the preview button
- new: MiniDeen avs filter added
- new: Advanced Info in the code editor was moved to the top level menu
       and AviSynth Info() can be shown directly from the Advanced Info
       without adding Info() to the script, staxrip generates the script
       with perfect font size and shows it with mpv (paused, no osc, no osd)

### 2.0.7.5 Beta

- fix: when the cut feature was used and afterwards the preview was
       opened from the code editor then the cut settings got damaged
- fix: when AviSynthShader was used StaxRip did not show
       a warning in case DirectX 9 is not installed
- fix: HE-AAC as demuxed as mka

- new: setting that allows to define how many frames
       are used for auto cropping
- new: some default settings were changed
- new: the buttons in the preview dialog look much better now, also flicker
       was eleminated and the button and trackbar size was increased,
       standard buttons were used, I don't know how it looks on Win 7...
- new: the csv file content in the apps dialog was improved
- new: auto crop feature in crop dialog finally shows progress
- new: the avisynth and vapoursynth code editor has a new
       'Advanced > Advanced Info' feature, it offers the following info:
       avs2pipemod info, avsmeter info, avsmeter benchmark and vspipe info

### 2.0.7.4

- fix: bug in crop dialog was fixed
- fix: raw format of VP8/VP9 (IVF) was undefined which
       caused a NotImplementedException
- fix: OverflowException of unknown cause
- fix: using txt chapters with mkv muxer caused xml exception
- fix: if the width was not mod 16 the crop and preview
       dialog showed a distorted image

- update: AviSynth 3.5.0

- new: the apps dialog can create a CSV file listing all tools
       with various properties like Name, Version, Modified date etc.
- new: x265 --rskip switch updated
- new: improved main menu
- new: improved app management dialog
- new: mpv and mpc started in the preview dialog start at the current
       time position of the preview instead of the beginning

### 2.0.7.3 Beta

- fix: x265 issues with --refine-ctu-distortion and --high-tier
- fix: certain avs and vpy files failed to load before, hopefully all external
       scripts can now be opened
- fix: opening source files that contain single quotes caused a VapourSynth
       script error, the fix will however only work when people reset their
       filter profiles
- fix: the local context help of x264 did often not work

- update: nvenc 4.66
- update: L-Smash-Work 20200207 HolyWu
- update: VSFilterMod R5.2.1
- update: BDSup2Sub++ 1.0.3
- update: qsvenc 3.31
- update: JPSDR 3.2.0
- update: d2vsource 1.2
- update: AVSMeter 2.9.8
- update: chapterEditor 1.21
- update: Subtitle Edit 3.5.13
- update: rav1e 0.3.0
- update: x264 2991-1771b55 Patman

- new: improved possibility to show AviSynth and VapourSynth script
       information with a new dialog and new pixel format parameter       
- new: the right click context help in the x264 and x265 dialog can now
       alternativly open the online help with ctrl or shift + right click
       to navigate directly to the online help of the right-clicked switch
- new: the F1 help in the video encoder dialogs was greatly improved
- new: in the nvenc dialog custom switches do overwrite regular switches
- new: Bwdif plugin for VapourSynth (similar to yadif)
- new: filters can be removed from the filter list view in the main dialog
       using the delete key
- new: qsvenc --colorrange --dhdr10-info --key-on-chapter --sub-source
- new: x264 --avcintra-flavor --index --input-fmt --muxer --quiet
       --verbose --video-filter
- new: the short version command line switches (like -t) are now supported
       in the x264 dialog


### 2.0.7.2 Beta

- new: showing MediaInfo for the source file works now even if
       the source file is an d2v/dgi index file

- new: improved detection to find out if all video encoder
       command line switches have a GUI implementation,
       (there is still a lot work to do)

- fix: use FrameServer.dll release build instead of debug build

### 2.0.7.1 Beta

- new: icons added in encoder dialog menus
- new: option dialog is shown in case a tool has multiple help
       resources, x265 for instance has a local help file containing
       the console help and it has a comprehensive online help.
       L-Smash-Works for instance has separate help pages
       for AviSynth and for VapourSynth
- new: medium quality for x264 was changed from crf 20 to 22
- new: medium quality for x265 was changed from crf 20 to 18
- new: %dpi% macro added, returnes the DPI value of the main dialog screen
- new: High DPI aware Info() AviSynth filter profile added to misc section
- new: the VFW interface used to access AviSynth and VapourSynth
       was replaced with a new library
- new: a dozen new x265 switches
- new: nvenc switches --colorrange, --psnr, --ssim
- new: search matches in the video encoder config dialog
       stay now permanently highlighted with bold font
- new: the search feature in the video encoder config dialog now
       also searches in option values, for instance searching
       for 'medium' will find and highlight the 'preset' switch
- new: in the video encoder config dialogs the context menu of the
       command line preview has a new menu item to search
       for the switch or string at the caret or cursor
- new: Opus format enabled for MP4Box muxer, this was requested
       but did not work in my test
- new: bmp format for cover art can be used in muxer dialog

- update: nvenc 4.65
- update: qsvenc 3.31
- update: vceenc 5.02
- update: x265 3.3+2-gbe2d82093 GCC 9.2.0 Patman
- update: ffmpeg N-96788 GCC 9.2.0 Patman
- update: mkvtoolnix 43
- update: MP4Box 0.8.0-rev178-g44c48d630 Patman

- fix: x265 three pass encoding (untested)
- fix: zoom in/out was flipped in preview dialog

### 2.0.6.2

- new: crop dialog supports hardware accelerated video rendering

- fix: info feature in preview dialog is back
- fix: when a second preview was opened the first one had broken rendering
- fix: image was vertically flipped using preview dialog with VapourSynth
- fix: main window DPI scaling issue on 96 DPI, please post a screenshot
       if you find something that doesn't look good!
       
### 2.0.6.1

- new: the preview dialog uses now a Direct2D hardware accelerated video renderer

- fix: the info tool in the code editor was not showing the correct colorspace

### 2.0.6.0 Stable

no changes since 2.0.5.3 Beta

### 2.0.5.3 Beta

- update: mkvtoolnix 39
- update: mpv.net 5.4.3
- update: NVEnc 4.55

- fix: few UI issues
- fix: avs MCTemporalDenoise/GradFun2DBmod
- fix: VUI luminance issue

### 2.0.5.1 Beta

- update: DGHDRtoSDR 1.13
- update: NVEncC 4.54
- update: VapourSynth R48
- update: x265 3.2+9-971180b100f8 Patman

- new: various UI improvements
- new: the short version of x265 and nvenc switches like -c, -f etc.
       were integrated into the search field and command line import
       feature, the search feature now first looks for a exact match
- new: x265 switches --dup-threshold, --frame-dup
- new: nvenc switches --multiref-l0, --multiref-l1
- new: StaxRip can use MediaInfo.NET instead of the built-in MediaInfo GUI,
       it requires MediaInfo.NET to be installed and started at least once

- fix: FFTW not always asked to be installed when needed

### 2.0.4.10 Beta

- update: mkvtoolnix 38.0.0
- update: x265 3.2+5-gfbe9fef31 Patman

- fix: issue using trim with multiple preview instances

### 2.0.4.9 Beta

- new: code formatting of avs/vs code was improved according to most common standard
- new: tab order changed in nvenc options

- fix: apps with custom path and unknown filename failed to launch
- fix: nvenc --aq-strength is now only visible if --aq is enabled
- fix: the right-click/context help in the encoder options failed in some cases 
- fix: a critical issue with the playback feature in the preview was fixed

### 2.0.4.8 Beta

- fix: MPC was shown as missing even for operations that do not require MPC

### 2.0.4.7 Beta

- new: to use nvenc --vbr-quality there is now a checkbox 'Constant Quality Mode'
       on the rate control tab (Patman86)
- new: --bref-mode added to nvenc
- new: after a source is loaded StaxRip automatically adds a filter
       to convert chroma subsampling to 4:2:0, this can now be
       disabled in the settings on a new Video tab
- new: MPC player integration was added to script editor and preview
       via menu and F10 key, mpv.net key was changed to F9

- fix: --aq-strength enabled for nvenc h265
- fix: there was a issue with the resize slider and resize menu using VapourSynth
- fix: in the preview the reload feature was causing an error in case
       the player was invoked before, also trim wasn't applied in the player
- fix: shortcut keys in the script editor did not work

### 2.0.4.6 Beta

- fix: main dialog was not set to quality mode using nvenc --vbr-quality

### 2.0.4.5 Beta

Thanks to Patman for providing the download links of the tools !!!

- update: JPSDR 3.1.3
- update: nvenc 4.50
- update: AVSMeter 2.9.6
- update: DGHDRtoSDR 1.12
- update: mvtools2 2.7.41
- update: FFT3dFilter 2.6.7
- update: TIVTC 1.0.14
- update: RgTools 0.98
- update: havsfunc r32
- update: DFTTest r6
- update: TCanny r12
- update: BM3D r8
- update: L-Smash Works 2019-09-17 HolyWu
- update: x265 3.2+3-fdd69a76688 Patman
- update: ffmpeg 2019-09-27 Patman

- new: x265 switch --selective-sao added
- new: L-Smash Works parameters prefer_hw 3 HW auto added to menu
- new: the way --vbr-quality works in nvenc was changed,
       if it's value is -1 the bitrate in the main dialog is used,
       if it's higher then -1 the bitrate is set to 0
- new: add support of video track title/name using MP4Box
- new: simple QP mode added to nvenc to use one instead of three QP values
- new: the menu renderer has now Win 10 style arrows for sub menu indication
- new: icons added to filter list and script editor, shortcuts added to script editor
- new: the preview can be closed by clicking in the top right corner
- new: in the script editor there are now two players available,
       mpv.net and whatever is registered for mkv, mpc-be supports vpy playback
- new: the way filters are added, replaced and inserted in the script editor
       was changed in order to improve the menu performance
- new: in the main menu a sub menu was added: Apps > Script Info,
       it contains avsmeter, vspipe and avs2pipemod for showing
       info about the currently active AviSynth or VapourSynth script,
       vspipe and avs2pipemod show parameters like bit depth, colorspace and framecount.
       In the script editor there is a new 'Script Info' menu item (Ctrl+I) doing the same.
- new: the mediainfo dialog shows the formats in the tab captions
- new: the mediainfo dialog has a new feature to move
       to the next and previous file via menu and shortcut
- fix: docs weren't built automatically
- fix: some bugs were fixed in the nvenc command line import feature

### 2.0.4.4 Beta

- fix: nfo files with non xml content caused an exception
- fix: if subtitle and audio titles contained illegal
  file system characters then these characters were replaced
  with an underscore and thus being lost, now the characters
  are escaped/unescaped and thus preserved
- fix: update x265 switch --refine-mv
- fix: in the nvenc dialog, Intel decoding options were present
  even when no Intel GPU is present
- new: a link to the web site has been added to the menu
- new: option to check online for new stable version once per day
- new: avs plugin HDRTools
- new: avs, vs plugin DGHDRtoSDR
- new: ffms2 parameter colorspace has menu support in the avs code editor

### 2.0.4.3 Beta

- new AviSynth script CropResize added
- update: x265 3.1+15-a092e82 Wolfberry
- update: MP4Box 0.8.0-rev69-5fe3ec1 Wolfberry
- update: ffmpeg 4.2.1 Wolfberry
- update: L-Smash Works 2019-09-14 HolyWu
- the filter list in the main dialog shows shorter filter names,
  for example 'BicubicResize' instead of 'Resize BicubicResize'
- SubtitleEdit is now included
- chapterEditor has been integrated into the main menu and
  into the container/muxer dialog
- a Tag File option has been added to the container/muxer dialog
- container/muxer has a Tags tab added with a grid view for editing tags
- Kodi nfo files are imported into tags

### 2.0.4.2 Beta

- new macros added: %source_dar%, %target_dar%,
  %source_par_x%, %source_par_y%, %target_par_x%, %target_par_y%
- fix: menu support of L-Smash Works parameters prefer_hw was incomplete

### 2.0.4.1 Beta

- nvenc switch --avhw updated (Patman86), --sub-source added
- qsvenc switches added: --data-copy, --adapt-ltr
- progress display support added for DGIndexNV, MP4Box and ffmpeg audio encoding
- L-Smash Works parameters prefer_hw and format have menu support in the avs/vs code editor
- the container/muxer dialog has a new tab for MKV attachments

- fix: vpy import did not work when the output variable isn't named 'clip'
- fix: a format exception was fixed, it was happening when the cut/trim feature was used

- update: nvenc 4.47
- update: L-Smash Works 2019-09-10 HolyWu
- update: MediaInfo 19.09
- update mpv.net 5.4.1.1

### 2.0.4.0 Stable

- update: ffms2 2019-08-30 StvG, successfully tested was the load time,
          memory usage and avs/vs compatibility
- update: L-Smash-Works r935+34 2019-08-29 MeteorRain, successfully
          tested was the load time, memory usage and avs/vs compatibility
- update: MediaInfo 19.07
- update: ffmpeg 4.2 Wolfberry
- update: nvenc 4.46
- update: qsvenc 3.24

### 2.0.3.1 Beta

- piping tool option added to x264 encoder
- piping with ffmpeg supports 10 bit now
- the track title wasn't detected from demuxed audio and subtitle tracks
- closing the settings dialog was setting the icon location as last source directory
- debug logging was improved, more debug info is included, it's no longer
  needed to edit the config file to enable debug logging, it's sufficient
  to enable it in the settings: Tools > Settings > General > Enable Debug Logging

### 2.0.3 Stable

- colorspace="YV12" removed from ffms2 defaults because it converts to 8 bit
- the x264 encoder uses now avs2pipemod64 because the avs input did not support 10 bit
- DTS-X is now demuxed as dtshd instead of mka
- update: mkvtoolnix 36.0.0
- update: avs FluxSmooth 1.4.7
- update: avsmeter 2.9.5

### 2.0.2.7 Beta

- added fdkaac pipe input support, in the audio settings
  go to: More > General > Decoding Mode > Pipe
- added missing icons
- file creation/write dates of apps were recovered
- update: ffms2 r1275+2-2019-08-11 HolyWu
- update: eac3to libraries libdcadec and libFLAC
- update: MP4Box v0.8.0-rev41-gb78fe5fbe Barough
- update: qaac 2.68

### 2.0.2.6 Beta

- dialogs were closing slow with 4K sources
- .NET Framework version updated to 4.8
- nvencc switch --vpp-select-every added
- fix for play menu item in filters menu of main dialog being disabled
- fix audio being not loaded by mpv.net when the avs/vs script is played
- fix a issue with the custom icon feature
- update: x265 3.1+11-de920e0 Wolfberry
- x265 --aq-mode update, new switches --hme and --hme-search added
- update: nvenc 4.44, new switches --data-copy, --nonrefp, fix --vpp-subburn
- update: L-Smash r935+31-2019-08-17 HolyWu
  added support for showing indexing progress and cachefile parameter and
  for native high bit depth, avs and vs l-smash are now contained in the same dll,
  avs and vs filter profiles have been changed to contain the the new cachefile
  parameter and high bit depth support has been added to the profiles, the profiles
  have not been reset so users have to update existing profiles manually
- update: VapourSynth R47.2, the new L-Smash update requires this
  new VapourSynth update
- update: python-3.7.4-amd64-webinstall
- update: mpv.net 5.3

### 2.0.2.3 Beta

- the progress bar in the processing window was using black as text color,
  this was changed to use SystemBrushes.ControlText which should be defined
  by the OS
- if the x265 dialog was opened before a source file was loaded, there could
  be an exception after the source file was loaded
- tooltip improvement in the muxer options

### 2.0.2.2 Beta

- new icons added and credits in about (info) dialog updated,
  thanks to: Freepik, ilko-k, nulledone, vanontom
- the progress bar in the processing window has better contrast
- video encoding with ffmpeg shows now progress in the progress bar and task bar
- if in the codec dialogs search field the enter key is pressed in order to cycle
  there is no longer an annoying sound made 
- encoding was failing when a ambersand (&) character was used in the path
- new AV1 codec option added to ffmpeg encoder
- various improvements and fixes made in the ffmpeg encoder
- new vceenc switches added
- new nvencc switches added
- new x265 switches added
- update: x265, Wolfberry build with avs input support,
  in the x265 options: Other > Piping Tool > None
- update: mpvnet
- update: ffmpeg, Wolfberry build with vpy input support
- update: nvencc, untested
- update: qsvencc, untested
- update: vceencc, untested

### 2.0.2.1 Beta

- Under 'Tools > Settings > User Interface' there is a setting to define a icon file

### 2.0.2.0 Release

- fix: downloads were using 7z extension even though it was zip instead of 7z

- update: LSMASHSource avs
- update: mkvtoolnix

### 2.0.1.3

- new: again new experimental icon, please give feedback if you like it,
  if it's not liked then it will be reverted to the classic icon, the problem
  with the classic icon is it looks outdated because it's not flat
- new: added ConvertFromDoubleWidth to AviSynth profiles, it's useful to fix the
  double width output that l-smash outputs for 10 (or more) bit sources.
  The profiles were not reset so the changes are only available
  after reseting the profiles manually
- new: in the x265 dialog it's possible to select the preferred piping tool,
  use at your own risk, not all combinations will work. In theory the included
  x265 built don't need a piping tool but I could not make it work without
  a piping tool. Options are: Automatic, None, vspipe, avs2pipemod, ffmpeg

- update: ffmpeg, unlike the built before this one supports mp3 encoding
- update: x264, x265
- update: d2v vs filter plugin

### 2.0.1.2

- fix for audio streams demuxed as mka

### 2.0.1.1

- new: nvencc --vpp-nnedi, --vpp-yadif
- new: few new x265 switches added

### 2.0.1.0

- new: experimental new icon, please give feedback if you like it,
  if it's not liked then it will be reverted to the old icon
- new: in the main menu under 'Apps > Media Info > vspipe' you can show info
  like shown [here](https://forum.doom9.org/showpost.php?p=1874254&postcount=3394).
  The main menu was not reset so it shows only if you reset it at: Tools > Edit Menu 

- change: selecting a filter in the filter menu, the names that are shown in the list were improved

- update: nvenc, x265
- update: ffmpeg, it's now possible to use vpy with ffmpeg
- update: rav1e, it's now possible to use rav1e with VapourSynth

- internal: Release() method (F11 key) added to automate the release process
- internal: Test() method updated to use relative paths

### 2.0.0.2

- there are no longer plugins that are shared between VapourSynth and AviSynth,
  ffms2 for VapourSynth was updated, the AviSynth version is 3 years old,
  ffms2 with C interface was removed because it was leaking memory
- the apps manage dialog displays the built date (using last modified date)
- on startup the main window often wasn't activated
- there was a bug that resulted in subtitles being demuxed without file extension
  and because of that staxrip couldn't find and include the subtitles, the reason
  was a breaking change in MediaInfo
- audio files were as mka demuxed due to a breaking change in MediaInfo
- webm files without mpeg-4 video format failed to load
- the navigation tree in the app manage dialog had a flicker issue
- MediaInfo was updated, most recent bugs are due to breaking changes in MediaInfo
- vslsmashsource updated
- mkvtoolnix update
- mpv.net was updated to version 3.5
- a major performance issue using AviSynth was fixed

### 2.0.0.1

- Removed Auto insert for RGB for VS and Convert Bits for AVS. Most are no longer useful due to 10Bit+ Videos are more common now.
- This is a pure Refresh Release, Mainily designed to update some of the encoding tools.
- This Release contains all the Changes from the Pipeline along with the Following.
- Updated Temp Folder Creation and Long Path Checker (Long Path Checker is only used on any OS below Window 10)
- Updated Comparison Tool It shouldn't need to index file when ffms2 is used.
- Updated Both Contact Sheet Creator and Video Comparison tool to use LWLibavVideoSource over the Standard L-Smash.
- Updated Visual Studio to 2019 RC, Anything Compiled will be using RC Build of VS 2019.
- VS: The default Resizer Code is slightly Different Compared to Base VS Resizer Code in the Context Menu.
- VS: SVPFlow Altered to Target CurrentFramte & Num / Den to achieve the Final Framerate instead of rounding. Type quality has also been Changed, aswell linear light support has been added (GPU Only).
- x265: AQmode Tweaked for the new Defaults.
- XAA Remove from AVS (Temp), due to certain Functions being alter in AVS+ core.
- VS: Updated DGIndexNV Load Function It now loads using it's native method instead of AVS load function.
- VS: G41Fun replaces hnwvsfunc
- VS: hnwvsfunc Script removed
- VS: fvsfunc functions updated
- VS: Auto-Deblock Added to Context Menu
- VS: nnedi3_rpow2 added to Script folder for Support.
- VS: Added FixTelecinedFades to Context Menu for RCR.
- Rebuilt QVSEnc Encoder Script
- x265 Has been updated to 3.0-AU With any Additional Chnages.
- Added Merge: XviD Encoding code has been Updated (Credit: jkilez)
- Added Merge: Chapter Cutting Feature to MkvMuxer (Credit: wybb)

2.0
- Brand New Update System All done within the App
- New Tab Added to x265 for Bitstream due to all the new Flags.
- Updated the default state of the x265 flags that were changed.
- x265 Flags Added:
    - Zone File
    - HRD Concat(The Flag is currently improperly marked in x265 cli)
    - Dolby Vision RPU
    - Dolby Vision Profiles
    - Tuning: Animation
    - Refine CTU Distortion    
    - hevc-aq
    - qp-adaptation-range
    - refine-mv-type renamed to refine-analysis-type    
- rav1e Flags Added:
    - Matrix
    - transfer
    - primaries
    - Min Key Int
- Added Support additional support for LongPaths 
    - There are two ways the app can name the files & Folders based on your OS & Harddrive Type
    - If One Method fails it now has a fallback method to use.
- Fixed QSVEnc key, Which was locking some users out.
- Updated the VapourSynth scripts that have been updated recently.
- Updated the filter names for Vapoursynth and removed the ones that don't exist anymore.
- Updated the switches for VCEnc
- Updated the MTModes Syntax for AVSynth
- Updated DFTTest syntax for Vapoursynth, Opt Settings is now set to AVX2(Settings it below 3 will set it to AVX or None).
- Added additional source filter when StaxRip internal thumbnail is used, some source files did not get along with ffms.
- Updated VapourSynth and Tweaked SVPFlow & Added Additional Option to use BlockFPS.
- Updated FFmpeg, MKVToolNix, NVENC, Rav1e, x265 & MediaInfo
- Python Support now includes Miniconda as well, Python 3.7.2 can be used as well.
- Included Both Old and Newer versions of MediaInfo Just incase it breaks something again.
- Re-Enabled MediaInfo Folder Function, Since The Bug in MediaInfo has been fixed.
- Added & Updated All the Output Path Options for MTN, StaxRip Thumbnailer, Gif & PNG Creators.
- The Help System for the Encoders Has been Altered. All the Help System works the Same now.
- Updated the dll files for both Avisynth & VapourSynth for filters that have been updated.
- Support for PNG has been added for VFW Saving Screenshot(Default Options are PNG & BMP, With JPG being able to be added in the Edit Menu).
- Cleaned up and Organized for File Structure inside App Folder.
- a small tweak in regards to the VFW.
- Updated mpvnet
- Plus some other changes as well.
- Plus all the Changes From Previous Two Beta Release(See Below)

2.0 Beta 2
- Updated a few of FFMpeg Flags for Audio Demuxing / Encoding.
- Moved HDR Ingest in Options Menu to Video Tab.
- Support for HLG Metadata has been Added to Ingest.
- Added Support for VUI for the Following ColorSpaces: Display P3 and DCI P3 (It still Must pass the other HDR Checks)
- Proper Master-Display data has been added for Display P3 and DCI P3.
- Support for Webm Subtitles has been Added For srt, sup, idx Files.
- Updated the VUI Import for HDR10, Due to MediaInfo Changes to Output Names.
- Changed the VUI import name for HLG, Due to MediaInfo not using proper colorspace name.
- Added Some Support for HLG to VUI import function, MediaInfo does not contain all HLG Metadata.
- Updated all the Help Files for the Encoders.
- Rav1e Encoder Has Been Added to Support AV1 codec.
    Flags:
        --tune
        --limit
        --speed
        --quantizer
        --keyint
        --low_latency
        --custom
- NVEnc Added Flags:
    -profile(h265)
    -vpp-padding(Left,Top,Right,Bottom)
    -vpp-tweak(Contrast,Gamma,Saturation,Hue,Brightness)
    -chromaloc
    -interlace tff
    -interlace bff
    -tier
    -pic-struct
    -aud
    -slices
    + Others...
- QSVEnc Added Flags:
    -vbv-bufsize
    -chromaloc    
    -vpp-scaling -> -vpp-resize
    -Filter: mctf
    -sao
    -ctu
    -tskip
    + Others...
- Removed any Switches that no longer exist in the CLI Encoders.
- Updated FFMPEG to 4.1
- ReBuilt MPVNet to work on Both older and Newer Systems(Requires Dotnet 4.8).
- Support has been added for MKV, Webm and MP4 to AV1.
- 32 Float Filters have been Added(Oyster, Plum, Vine)
- FFMPEG Shared dll Files has been Updated to 4.1
- Added mpvnet back with fully working mpv dll file.
- Re-Enabled MediaInfo Folder, It's Been Fixed to with latest MediaInfo Code.
- x264 has Been Updated to 2935
- x265 has Been Updated to 2.9+9
- Cleaned up the Config Files.
- Update Script has been moved to Python Code, instead of basic Powershell Script.
    - Site Packges Required: bs4(BeautifulSoup), Requests, win32api, tqdm, & psutil
- Added Update Script for NVEnc & QVSEnc.
- mtn has been Updated, Uses less Shared files.


2.0 Beta 1
- Temp Folder Creation has been slightly altered
- Removed some old Code, That wasn't doing anything anymore.
- AVSMeter has Been Updated to 2.8.6.
- Updated eac3to, fdkaac & qaac.
- nnedi3 for AVS has Been Updated to 0.9.4
- x264 & x265 have compiled Using GCC ToolChain instead of MVS.
- Python Search Path has been Updated to 3.7/3.7.1 & Python 3.6 has been Removed.
- Also Included Support for Anaconda3.
- The VapourSynth Script has been slightly altered to work better with Python Search Path.
- VS has been Updated to R45.
- MKVToolNix Updated to 28.2.0.7
- Added hnwvsfunc for VS & mClean to Context Menu.
- Cleaned up the AVS Filter Names.
- Added MultiSharpen Function
- Modified the Profile List for x265, Only Profiles that work with Current Selected Depth with Display now.
- Removed any 8Bit x265 Profiles that no Longer used with 2.9(Based on: https://x265.readthedocs.io).
- Moved Some Extra Functions to the System Process.
- Custom Directory Output Option has been Added to StaxRip Thumbnail Creator & MTN. It's Default Directory is the last Used Location. If none is known it defaults to C Drive(Fresh Install)
- Added support for Long Path Aware(Make Sure to Enable it If your OS Supports it)
- Fixed any Issues for very Short filenames being converted to wrong type.
- Removed MediaInfo 18.08.1 Due to Bugs it contains and Re-Added MediaInfo 18.05.
- Added More Support for HDR to Intel Encoder.
- VS Filters Added: W3FDIF, MiniDeen, IT, TDeintMod, VC* Filters & TemporalMedian.
- Updated mvtools-sf to AVX, The latest Builds only use AVX2 which not all CPU's Support.
- Added Support for RawSourcePlus for Avisynth and RawSource for VapourSynth. Default Pixel_type is set to YUV420P10 (aka P010).
- Dual Package Setup for x264 have been Removed for Single release x264(8+10Bit). The Depth menu will set the Output Bits, exactly like x265.
- Following Have Been Added to x264:
    - ColorMatrix: Added chroma-derived-nc, chroma-derived-c, and ICtCp.
    - Added Alternative Transfer
    - vbv-init has been modded to have a locked value and proper float point scaling.
- FFTW Files will auto copy to System Directory, if your PC is missing these files in the System32 Folder(Fresh Install).
- MP4Box is back to Static Once again.
- Included Setup files for Python 3.7.1 & VS R45 to make sure the proper versions are being installed for StaxRip.

1.9.0.0

- Added a new Marco to the list.
    - %Script_files%    
- Added Total System Memory to System Environment & Logging
- mpv has been updated to the latest nightly release
- FFmpeg has been updated to 4.0.2
- Mediainfo has been updated to 18.08.1
- MP4Box has been Updated and includes required dll files that are required now.
- NVEnc has been updated to 4.12
- QSVEnc has been updated to 3.09
- x264 has been updated to 2932
- x265 has been updated to 2.8+68
- MKVToolNix has been Updated to 26.0.0
- AVSMeter has been Updated to 2.8.5
- Added Two new Scripts for HDR10+ AVS & VS Version. Along with updated Script for Source load.
- Added Support Added for FFMS LoadCPlugins. FFMS2K will use the normal LoadPlugin Function.
- Avisynth and Vapousynth have Separate Menu tabs in the App Forms. Vapoursynth is no longer buried inside Avisynth Menu at the bottom.
- Removed all the dead links from Package Section and Replaced them with the most up to date URL's, If one existed.
- Added Support for PNG & Tiff when saving a screenshot of a single Frame in the Preview Window. To add or Remove a format, Everything can be done through the Edit Menu.
- StaxRip now has a Update Function through PowerShell Script(Can be Accessed Via: Tools -> Advanced -> Check For Update. It will first Shutdown the app and check for updates. If a new version is avaible, it download, extract and re-open StaxRip.
- Updated the PowerShell 5 Reference Assemblies to version 1.1.0.
- Framework has been Updated to 4.7.2.
- The main drop-down menus have been cleaned up and better organized
- The control boxes For the codecs have also been cleaned up, to create a shorter drop down.
- Subtitle Tools have been moved to Advanced -> Tools -> Subtitles.
- Added vscube, Second Scripts for Utils and some small tweaks to selected scripts for AVS+.
- Updated VapourSynth Plugins & Context menu (See Below for Context Options).
- Like with AVS+ There are allot more functions supported, Not all have been added to the context menu.
- Settings:
    - Template Folder has been Renamed to just Templates & .dat file has been Renamed to Settings.dat. All your previous Templates will still work, Just move them to the new folder.
- Internal Thumbnail Creator Has been Updated with the Following Features:
    - Added support for PNG, BMP & TIFF. 
    - It Fully supports HDR10, HDR10+ and HLG.
    - Add support for Gap (The space between each shot, default: 0).
    - The ability to disable the StaxRip Logo has been added.
    - The Font styles have been changed to DejaVu Serif which give a much clearer & cleaner look. 
    - The Default Logo Font Style has been changed to mikadan, Which gives the logo a bit more Style.
    - If for what reason you do not have DejaVu Serif or mikadan font, They have been included. Once installed They can be deleted. FontAwesome & Segoe-MDL2-Assets must remain next to .exe file.
    - Cleaned up the Header to make look more Professional like and added more video & Audio Details.
        - File: name
        - Size: Filesize in bytes / MBS, Length, and average bitrate.
        - Video: Codec, Format Profile, colorspace,colour_range, Bitrate, FrameRate, ChromaSubsampling, Height & Width
        - VideoCodec (The old code wasn't working with latest version of Mediainfo.)
        - Audio: Codecs, Channels, kHz and Bitrate
    - The Matrix for the Creator has also Been tweaked for correct colorspace.

- Added Support for New Thumbnail Creator Which can be used by it self or added into the project process. It Uses FFMPEG engine to create the Thumbnails without the need to index any file(s).
    Options Support as of Right Now:
        - Number of Columns
        - Number of Rows
        - Width of Each Shot
        - Height of Each Shot
        - Depth of Each Shot
        - Quality of Jpg

- Added Animation Support in the Form of gif & apng(png). by it self or be added into a project process. It Uses FFMPEG engine to create the animation without the need to index any file(s).
    Options for Gif:
    - Scale (Width x auto)
    - FrameRate (No Float points are Support, it must be a solid number like 60 and not 59.940.)
    - Statistics Mode
    - Diff Mode
    - Dither Options
    - Starting Time (in Seconds) - Supports Float Points like 25.2.
    - Length(in Seconds) - Supports Float points.
- Options For aPNG:
    - Starting Time (in Seconds) - Supports Float points.
    - Length(in Seconds) - Supports Float points.
    - FrameRate -  No Support for Float points.
    - Scale    (Width x auto)
    - Opt Settings (zlib,7zip & Zopfli) - zlib is the fastest with Zopfli being the slowest. 7zip is the balance between the two.

- Updated all Vapoursynth variable names for existing filters that have been changed or removed since 1.7.
- Tweaked the Job Loading function, When a previous job is loaded into StaxRip the form will now automatically close.
- Added Support for Remaining HDR10 colorspace Metadata, To make it full compatible with HDR10 Standards(MKV Only). The function can be enabled in the option section under Image -> Extras. It can also be done Manually The tool can be accessed by Apps -> MediaInfo -> MKVHDR
- Altered VUI import system to match MediaInfo changes. For HDR10 it must match BT.2020 Colorspace. It will now also enable the additional flags in VUI and Bitstream to encode to HDR10. GOP & VBV Settings will have to be set manually.
- Removed the Character Limit on the Temp Folders. It will not longer create the '...' after it hits 30 characters Which was designed for Windows 260 limit(Mainly for older Windows). If you need more then 260 characters you can alter the GroupPolicy Settings which will allow your to bypass the limit.
- Added MKVinfo App to menu to check colorspace coordinates (Which Mediainfo no longer displays), This is useful for HDR content to make sure metadate has the correct HDR metadata.
- MediaFolder Folder Option has been Disabled(It was no longer Working correctly due to latest update to Mediainfo) and has been replaced with the internal MediaInfo GUI that allows you to open any media file, If it's loaded in StaxRip or not.

- x265 Changes:
    - 12Bit Profiles Have Been Added    
    - VUI Menu has been Broken Up into two Menus now due to the Growing Number of options in the VUI section.
    - Added:
        - chromaloc
        - DHDR10-Info
        - atc-sei
        - pic-struct
        - display-window
        - opt-ref-list-length-pps
        - opt-qp-pps
        - single-sei
        - dynamic-refine
        - idr-recovery-sei
        - VBVinit, VBVend, VBV Adjustment with VBVinit being tweaked.
        - Maximum AU size Factor
        - refine-intra has been increased to Level 4
        - multi-pass-opt-analysis & multi-pass-opt-distortion have been moved to Rate Control 1 menu to make room for VBV options in Rate Control 2 Menu.
- VapourSynth Updated Context Menu:
    The List of all Included Filters:
- Source: 
    - AVISource
    - d2vsource
    - DGSource
    - ffms2
    - LibavSMASHSource
    - LWLibavSource
- FrameRate
    - AssumeFPS
    - InterFrame
    - SVPFlow
- Color
    - BitDepth
    - Matrix
    - Transfer
    - Primaries
    - Cube
    - Range
    - Tweak
    - Convert To
    - Format (Mainly Designed for HDR to SDR)
    - To 444
    - To RGB
    - To YUV
    - Dither Tools
- Line
    - MAA
    - DAA
    - Sangnom
    - TAAmbk
    - SeeSaw
    - FineSharp
    - MSharpen
    - aWarpSharpen2
    - LSFmod
    - pSharpen
    - SharpAAMcmod
- Resize
    - Resize
    - ReSample
    - Dither_Resize16
- Field
    - IVTC
    - QTGMC
    - nnedi3
    - Yadifmod
    - nnedi3cl
    - znedi3
    - Support Filters
- Noise
    - DFTTest
    - BM3D
    - HQDN3D
    - KNLMeansCL
    - MCTemporalDenoise
    - DegrainMedian
    - RemoveGrain
    - SMDegrain     
    - TTempSmooth
    - VagueDenoiser
- Misc
    - Anamorphic to Standard
    - UnSpec
    - Histogram
- Restoration
    - Deblock
    - Deblock PP7
    - Deblock_QED
    - MDeRing
    - MSmooth
    - abcxyz
    - BlindDeHalo3
    - DeHaloAlpha
    - EdgeCleaner
    - HQDering
    - YAHR
    - Vinverse
    - Vinverse 2

1.8.0.0

- All Filter are 64Bit enabled.
- Scripts and Code is designed for Avisynth+. Using older versions of Avisynth will unlikely work with everything.
- Restored the Old Icon and removed the RIP Icon.
- Added Support for Max CLL, MAX FALL and Master-Display for Nvidia H.265 Encoder.
- Added Support for Max CLL, MAX FALL and Master-Display for QSVEnc(Intel) H.265 Encoder.
- XviD has been Updated to 1.3.5
- Updated x265 to version 2.8+12(8+10+12 Bit Support)
- Add Support for AVX512 for x265 
- Updated FFMPEG to 4.0+
- Updated NVENCc to 4.07
- QSVEnc(Intel) has been Updated to 3.05
- MKVToolNix has been Updated to version 23.
- Updated MP4Box to 0.7.2
- eac3to has been Updated to 3.34
- AVSMeter has been Updated to 2.7.8
- fdkaac has been compiled for 64Bit
- Mediainfo has been Updated to 18.05
- FrameServer for VirtualDub2 and VirtualDub 64Bit works with Video. *Currently a small issue with Audio not decoding properly* (Files are Included)
- MPVnet has been Replaced with MPV. It uses the extact same code as MPVnet. MPVnet was replaced due to playback issues it was creating.
- Project X has been Removed
- Java has been Removed
- AV1 has been Removed for the Time Being (Encoder is unstable).
- mClean has been Updated to 3.2.
- awarpsharp2 has been Updated to 2.0.1
- FrameRateConventer Has been Updated to 1.2.1
- JPSDR has been Updated to 2.2.0.7
- MaskTools2 Has been Updated to 2.2.14
- MVTools2 has been Updated to 2.7.31
- QTGMC has been Updated to 3.358s
- TIVTC has been Updated to 1.0.11
- Added Color, FrameRate, Line, and Restoration context Menu.
The List of all Included Filters:
- Source: 
    - Automatic
    - AviSource
    - DirectShowSource
    - FFVideoSource 
    - Manual
    - MPEG2Source
    - DGSource
    - DGSourceIM*
    - DGSourceNV*
    - DSS2
    * = Not Freeware.
- Resizers: 
    - Dither_Resize16 
    - ResizeMT
    - Resize
    - Hardware Resize
    - JincResize
    - SuperResXBR
    - SuperRes
    - SuperXBR
    - Resize(Z) - Uses ZLib
- Crop: 
    - Dither_Crop16
    - Crop
    - Hardware Crop
- Line:
    - MCDAA3
    - DAA3Mod
    - MAA2Mod
    - XAA
    - HDRSharp
    - pSharpen
    - MSharpen
    - aWarpSharp2
    - FineSharp 
    - LSFmod (With More Syntax added for better results then Default)
- Misc:
    - MTMode (Multithreading)
    - SplitVertical (For MT) 
- Restoration: 
    - CNR2
    - Deblock    (Includes Dll for Both old and New versions, New version is enabled by Default).
    - Deblock_QED 
    - DehaloAlpha
    - FineDehalo
    - HQDeringmod
    - MipSmooth
    - SmoothD2 
- Color:
    - AutoAdjust
    - Histogram
    - ColorYUV (AutoGain)
    - Levels and the Correct color levels for each BitDepth.
    - Tweak
    - Convert(Format(ColorSpace), To(DataFormat), Bits(BitDepth)
    - Dither Tools
    - DFTTest (For Stack)
    - HDRCore
    - HDRColor*    
    - HDRNoise (Function must be Entered Manually to Use)
    - HDRMatrix
    - HDR Tone Mapping
    * = Not Freeware
- Framerate:
    - AssumeFPS
    - AssumeFPS_Source
    - InterFrame
    - ConvertFPS
    - ChangeFPS
    - SVPFlow (Scales to 59.940 and uses GPU by Default)
    - YFRC
- Field:
    - QTGMC (With additional Syntax for both Interlaced and Progressive Sources. Plus Repair option for bad interlaced sources (Includes Progressive)
    - nnedi3
    - IVTC
    - Assume (AssumeTFF & AssumeBFF)
    - EEDI3
    - FieldDeinterlace
    - yadifmod2
    - Select (SelectEven & SelectOdd)
- Noise:
    - KNLMeansCL (Added Device_type to Syntax)
    - DeNoiseMD + Histogram
    - DenoiseMF + Histogram
    - DFTTest
    - mClean
    - RemoveGrain16 
    - Repair16
    - Repair
    - HDRNoise (Syntax must be entered Manually)
    - RemoveGrain
    - FFT3DGPU 
    - HQDN3D
    - SpatialSoften (YUY2 Only)
    - TemporalSoften 
    - MCTemporalDenoise (Script has been Updated to work properly with GradFun2DB & GradFun2DBMod).
    - MCTemporalDenoisePP (Same as Above)
    - SMDGrain
    - DFTTest
    - TNLMeans 
    - VagueDenoiser 
    - xNLMeans)
Support Filters & Scripts:
    - AddGrainC
    - DeGrainMedian
    - GradFun2DB
    - GradFun2DBMod
    - HDRCore Source
    - AVSTP
    - TEMmod
    - MT Expand Multi
    - SmoothD2c
    - edi_rpow2
    - Average
    - nnedi3 rpow2
    - LUtils
    - TMM2
    - SVPFlow 1
    - SVPFlow 2
    - TTempSmooth
    - Depan
    - DepanEstimate
    - TemporalDegrain
    - AnimeIVTC
    - f3kdb
    - TComb
    - TDent
Altered:
    Tweak: Syntax has been alter for Adjusting saturation and includes both realcalc and dither_strength(Only Avisynth+ use these Functions).
    - RemoveGrain: added a secondary option for for artifact removal.    
    - KNLMeansCL: Added additional syntax for Device_type which is set to Auto. It will try to use GPU first before trying to use the CPU. 
     - Levels with correct range for each bitdepth. Level function does not autoscale based on the bitdepth.
    - added more modern Framerates to Assumefps, ConvertFPS and ChangeFPS. 144 & 240 rates have been added.
- Removed from Filter context Menu but can still works with the auto loading feature(Syntax must be Entered Manually): 
    - Clense (Part of JPSDR)
    - Checkmate
    - SangNom2
    - TComb
    - Undot
    - MedianBlurTemporal
    - FluxSmoothST
    - FluxSmoothT
    - MedianBlur
    - SangNom2
    - f3kdb
    - Vinverse 2

Filters and Software that Support AVX2: aWarpSharp2, DCTFilter, DFTTest, JPSDR, Masktools2, TMM2, TNLMeans, yadifmod2, x265
    - By default AVX or below are activate for compaiblity. 
- VapourSynth is untouched(No Changes).
