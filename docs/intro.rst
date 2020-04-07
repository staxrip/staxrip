============
Introduction
============

About
=====

StaxRip is a video encoding GUI for Windows, it executes and controls console apps like x265, mkvmerge and ffmpeg and uses the scripting based frame servers AviSynth+ and VapourSynth for video processing.


Target Audience
===============

The target audience are users that accept a learning curve in order to achieve the highest possible quality and learn the basics of video encoding and the underlying tools.


Alternative Applications
========================

Normal User
-----------

`Handbrake <https://handbrake.fr>`_ 

`VidCoder <https://vidcoder.net>`_ 


Power User
----------

`MeGUI <https://sourceforge.net/projects/megui>`_ 

`RipBot264 <https://forum.doom9.org/showthread.php?t=127611>`_ 

`Hybrid <http://www.selur.de/>`_ 


Feature Highlights
==================

- Support for a wide variety of formats and tools
- All modern encoders are supported: x264, x265, NVEnc, SVT-AV1
- Hardware encoding for AMD, Intel and NVIDIA
- Batch Processing
- Video editing using AviSynth+ and VapourSynth
- Extendable via PowerShell
- Minimal user interaction required due to rich configuration and automation features
- Copy modes for audio and video for plain remuxing
- Cut/Trim feature, for MKV output no re-encoding is required
- High DPI scaling


Requirements
============

StaxRip is a x64 application, it's necesarry that all of the requirements are available in the x64 variant.

The application runs only on the Windows operating system, the minimum version is Windows 7.

Another requirement is the `.NET Framework <https://www.microsoft.com/net/download/dotnet-framework-runtime>`_ which must be installed in version 4.8.

`Visual C++ 2019 <https://support.microsoft.com/en-gb/help/2977003/the-latest-supported-visual-c-downloads>`_ is required with minimum version 14.25.28508.3.

StaxRip requires that either AviSynth+ or VapourSynth is installed, installers are included. It's possible to install and use both AviSynth+ and VapourSynth. VapourSynth requires that also Python is installed.

Hardware encoding works only on newer hardware and up to date drivers.

It's important to understand that x64 and x86 can never be mixed, everything StaxRip requires and everything it uses is x64.


License
=======

Licensed under the `MIT license <https://opensource.org/licenses/MIT>`_.


Download
========

`Stable Releases <https://github.com/staxrip/staxrip/releases>`_

Before making a bug report please try the newest beta build:

`DropBox <https://www.dropbox.com/sh/4ctl2y928xkak4f/AAADEZj_hFpGQaNOdd3yqcAHa?dl=0>`_

`OneDrive <https://1drv.ms/u/s!ArwKS_ZUR01g0kH4d4eT_6a3GaKe?e=qbOfGS>`_


Setup
=====

It's required to use Windows 7 or higher in the x64 variant.

First `Visual C++ 2019 <https://support.microsoft.com/en-gb/help/2977003/the-latest-supported-visual-c-downloads>`_ must be installed in the x64 variant. The version must be up to date.

StaxRip requires that either AviSynth+ or VapourSynth is installed in the x64 variant, installers are included under Apps\Installer. It's possible to install and use both AviSynth+ and VapourSynth. VapourSynth requires that Python is installed before VapourSynth, both in the x64 variant.

Whenever StaxRip starts the first time from a new location it will ask for a settings folder location to use for this particular start-up location. You can open the current settings folder from the main menu at Tools > Folders > Settings. It's possible to run different StaxRip versions side by side with different settings.

It's not recommended to start StaxRip from a location without full write access so 'C:\\Program Files' shouldn't be used. StaxRip uses almost 200 tools and some of these tools might require write access to their start-up directory.

To add StaxRip to the windows start menu right-click the StaxRip.exe and choose *Add to Start*.
