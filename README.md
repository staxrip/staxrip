# About

staxrip is a multiformat video encoding application for Windows. It's a GUI frontend to processing tools such as mp4box, mkvtoolnix, avisynth, vapoursynth, x264, x265, nvenc, eac3to and many more.

## Features

- Supports all popular formats and tools
- Hardware encoding for AMD, Intel and NVIDIA
- Batch Processing
- AviSynth and VapourSynth code editor
- Extendable via PowerShell, .NET and COM+
- Minimal user interaction required due to rich configuration and automation features
- copy modes for audio and video for plain remuxing
- Cut/Trim/Edit feature, for MKV output no re-encoding required
- Aiming for perfect High DPI scaling 

## Requirements

- Windows 7 x64 or Windows 10 x64
- [.NET 4.7 or higher](https://www.microsoft.com/en-us/download/details.aspx?id=55170)
- [Intel Skylake](https://en.wikipedia.org/wiki/Skylake_%28microarchitecture%29) or newer for HEVC/H.265 hardware encoding
- [NVIDIA Maxwell gen2 card](https://en.wikipedia.org/wiki/Maxwell_%28microarchitecture%29#Second_generation_Maxwell_.28GM20x.29) or newer for HEVC/H.265 hardware encoding
- [AMD Polaris card](http://www.amd.com/en-gb/innovations/software-technologies/radeon-polaris) or newer for HEVC/H.265 hardware encoding
- AviSynth+ x64, the installer is bundled with StaxRip x64 or alternativly VapourSynth x64 which requires Python x64

## License

Licensed under the [MIT license](LICENSE.TXT).

## Download

https://github.com/stax76/staxrip/releases

Before making a bug report please try the newest test build:

https://github.com/stax76/staxrip/blob/master/md/changelog.md

## Support

For bug reports and feature requests please use the issue tracker:

https://github.com/stax76/staxrip/issues

I also provide help in 3 forums:

English http://forum.doom9.org/showthread.php?t=172068&page=55555

English http://forum.videohelp.com/threads/369913-StaxRip-x64-for-AviSynth-VapourSynth-x264-x265-GPU-encoding/page55555

German http://forum.gleitz.info/showthread.php?26177-StaxRip-Encoding-Frontend-%28Diskussion%29/page55555

If you don't want to register at github or in a forum you can also mail me at frank.skare.de at gmail com

Often I need a full log file which you can paste at www.pastebin.com, enable auto expire after 30 days.

Screenshots are also often useful.

Before making a bug report please try the newest test build, download it here:

https://github.com/stax76/staxrip/blob/master/md/changelog.md

## Screenshots

https://github.com/stax76/staxrip/wiki/Screenshots

## Setup

StaxRip is for the most part portable, some features require certain apps to be installed, in such case StaxRip provides guidance so the user must not take great care.

Whenever StaxRip starts the first time from a new location it will ask for a settings folder location to use for this particular start-up location. You can open the current settings folder from the main menu at Tools > Folders > Settings

It's not recommended to start StaxRip from a location without full write access so 'C:\Program Files' shouldn't be used. StaxRip utilizes dozens of tools and some of this tools might require write access to their start-up directory.

To add StaxRip to the windows start menu use the context menu of the windows file explorer.

## AviSynth and VapourSynth scripting

StaxRip uses AviSynth x64 and VapourSynth x64 for video processing and includes many x64 plugins, 32-bit/x86 plugins are not compatible.

## Command Engine

StaxRip uses a command engine for the following features:

- StaxRip's command line interface (documented at: Main Menu > Help > Command Line)
- StaxRip's configurable main menu and various other menus
- StaxRip's Event Command Feature (located at: Main Menu > Tools > Advanced > Event Commands)

The Event Command feature allows to run commands on defined events under defined conditions.

A interesting command is -Perform-ExecuteBatchScript

It executes a batch script with solved macros. Macros are also available as environment variables. Executables started by the batch script inherit the access to these environment variables.

The 'Execute Batch Script' command has a option 'Interpret Output' that can be turned on to interpret each console output line from the batch script or executables started in the batch script as StaxRip command. The documentation of the StaxRip commands is located at: Main Menu > Help > Command Line.

Used executables can be powershell.exe, cscript.exe (VBScript/JScript), python.exe, any other scripting engine or any console app programed in any programming language.

## PowerShell Scripting

StaxRip can be automated via PowerShell scripting. There are two example scripts included, they can be found at:

Main Menu > Tools > Scripts > Open Scripts Folder

### Scripting Events

In order to run scripts on certain events read the help at:

Main Menu > Help > Scripting

### Scripting Support

If you have questions feel free to ask here: https://github.com/stax76/staxrip/issues/200

### Scripting Examples

Sets a deinterlace filter if the MediaInfo property 'ScanType' returns 'Interlaced':

```
# active project
$p = [ShortcutModule]::p

#global object with miscelenius stuff
$g = [ShortcutModule]::g

if ([MediaInfo]::GetVideo($p.FirstOriginalSourceFile, "ScanType") -eq "Interlaced")
{
    $p.Script.SetFilter("yadifmod2", "Field", "yadifmod2()")
}
```