Introduction
============

About
-----

StaxRip is a video encoding GUI for Windows, it executes and controls console apps like mkvtoolnix and x265.


Target Audience
---------------

The target audience are users that accept a learning curve in order to achieve the highest possible quality and learn the basics of video encoding and the underlying tools.


Comparison to similar applications
----------------------------------

There are two types of encoding frontends, the first type are frontends that have an own engine, these apps have a limited feature set but sufficiant for the majority of use cases. There are several advanteges of this type, apps are beginner friendly and don't use much disc space. Handbrake is by far the most popular encoding frontend, it's known to be capable and reliable, VidCoder is an alternative frontend for the Handbrake engine.

The second type are frontend that use external console tools and a scripting based video processing engine. AviSynth is the pionier and VapourSynth the modern alternative. These frontends have more features and a steeper learning curve. StaxRip belongs in this category.

Megui uses a toolbox approach where many features can be used independently because they are decoupled. It does not support all modern tools such as VapourSynth, NVEnc or SVT-AV1.

RipBot264 does not support all modern tools. It supports distributed encoding in networks. RipBot264 is not open source.

Hybrid supports other operating systems besides Windows like Handbrake and supports all modern tools. Hybrid is not open source.

StaxRip supports all modern tools.

The main differences are:

Windows only vs portable.

Open Source vs Closed Source.

Lightweight apps with an own engine vs heavyweight apps that use external tools.

The dominant basics like x265 and AviSynth vs modern alternatives like VapourSynth, NVEnc and SVT-AV1.


StaxRip Feature Highlights
--------------------------

- Support for a wide variety of formats and tools
- All modern encoders are supported, x264, x265, NVEnc, SVT-AV1
- Hardware encoding for AMD, Intel and NVIDIA
- Batch Processing
- Video editing using AviSynth+ and VapourSynth
- Extendable via PowerShell
- Minimal user interaction required due to rich configuration and automation features
- Copy modes for audio and video for plain remuxing
- Cut/Trim feature, for MKV output no re-encoding is required
- High DPI scaling


Requirements
------------

StaxRip is a x64 application, it's necesarry that all of the requirements are available in the x64 variant.

The application runs only on the Windows operating system, the minimum version is Windows 7.

Another requirement is the `.NET Framework <https://www.microsoft.com/net/download/dotnet-framework-runtime>`_ which must be installed in version 4.8.

`Visual C++ 2019 <https://support.microsoft.com/en-gb/help/2977003/the-latest-supported-visual-c-downloads>`_ is required with minimum version 14.25.28508.3.

StaxRip requires that either AviSynth+ or VapourSynth is installed, installers are bundled with StaxRip. It's possible to install and use both AviSynth+ and VapourSynth. VapourSynth requires that also Python is installed.

HEVC/H.265 hardware encoding using Intel hardware requires the `Skylake Platform <https://en.wikipedia.org/wiki/Skylake_%28microarchitecture%29>`_.

`A NVIDIA Maxwell gen2 card <https://en.wikipedia.org/wiki/Maxwell_%28microarchitecture%29#Second_generation_Maxwell_.28GM20x.29>`_ is required for HEVC/H.265 hardware encoding.

`A AMD Polaris card <http://www.amd.com/en-gb/innovations/software-technologies/radeon-polaris>`_ is required for HEVC/H.265 hardware encoding.

It's important to understand that x64 and x86 can never be mixed, everything StaxRip requires and everything it uses is x64.


License
-------

Licensed under the `MIT license <https://opensource.org/licenses/MIT>`_.


Download
--------

`Stable Releases <https://github.com/staxrip/staxrip/releases>`_

Before making a bug report please try the newest beta build:

`DropBox <https://www.dropbox.com/sh/4ctl2y928xkak4f/AAADEZj_hFpGQaNOdd3yqcAHa?dl=0>`_

`OneDrive <https://1drv.ms/u/s!ArwKS_ZUR01g0kH4d4eT_6a3GaKe?e=qbOfGS>`_

Setup
-----

StaxRip is for the most part portable, some features require certain apps to be installed, in such case StaxRip provides guidance. 

Whenever StaxRip starts the first time from a new location it will ask for a settings folder location to use for this particular start-up location. You can open the current settings folder from the main menu at Tools > Folders > Settings

It's not recommended to start StaxRip from a location without full write access so 'C:\\Program Files' shouldn't be used. StaxRip uses almost 200 tools and some of these tools might require write access to their start-up directory.

To add StaxRip to the windows start menu right-click the StaxRip.exe and choose *Add to Start*.
