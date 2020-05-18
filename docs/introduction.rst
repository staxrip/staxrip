
============
Introduction
============


About
=====

StaxRip is a video encoding GUI for Windows, it executes and controls console apps like x265, mkvmerge and ffmpeg and uses the scripting based frame servers AviSynth and VapourSynth for video processing.


Feature Highlights
------------------

- Support for a wide variety of formats and tools
- All popular and modern video encoders like x265 and nvenc are supported
- Hardware encoding for AMD, Intel and NVIDIA
- Batch Processing
- Video editing using AviSynth and VapourSynth
- Scriptable via PowerShell
- Minimal user interaction required due to rich configuration and automation features
- Copy modes for audio and video for plain remuxing
- Cut/Trim feature, for MKV output no re-encoding is required
- High DPI scaling
- Portable, nothing has to be installed
- Parallel Job processing, parallel audio video processing, parallel junk encoding for x265


Download
========

Stable
------

`<https://github.com/staxrip/staxrip/releases>`_

.. _beta:

Beta
----

`DropBox <https://www.dropbox.com/sh/4ctl2y928xkak4f/AAADEZj_hFpGQaNOdd3yqcAHa?dl=0>`_

`OneDrive <https://1drv.ms/u/s!ArwKS_ZUR01g0kH4d4eT_6a3GaKe?e=qbOfGS>`_


Setup
=====

It's important to understand that x64 and x86 can never be mixed, everything StaxRip requires and everything it uses is x64, x86 plugins generally don't work in x64 applications.

Windows 7 x64 is the minimum required Windows version.

`.NET Framework <https://www.microsoft.com/net/download/dotnet-framework-runtime>`_ is required in verrsion 4.8.

Another requirement is `Visual C++ 2019 x64 <https://support.microsoft.com/en-gb/help/2977003/the-latest-supported-visual-c-downloads>`_ with minimum version 14.25.28508

StaxRip has portable versions of AviSynth, VapourSynth and Python included, a setup is not required.

If AviSynth or VapourSynth was installed then StaxRip will use the installed version instead of the included portable version.

Hardware encoding works only on newer hardware and up to date drivers.

It's not possible to start StaxRip from a location without full write access.
