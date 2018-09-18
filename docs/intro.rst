Introduction
============

About
-----

StaxRip is a video encoding app for Windows with a unrivaled feature set and usability.


Features
--------

- Support for countless formats and tools
- Hardware encoding for AMD, Intel and NVIDIA
- Batch Processing
- AviSynth and VapourSynth code editor
- Extendable via PowerShell
- Minimal user interaction required due to rich configuration and automation features
- Copy modes for audio and video for plain remuxing
- Cut/Trim/Edit feature, for MKV output no re-encoding required
- Pixel perfect High DPI scaling


Requirements
------------

- Windows 7 x64 or Windows 10 x64, StaxRip is x64 only
- `.NET 4.7 <https://www.microsoft.com/en-us/download/details.aspx?id=55170>`_
- `Intel Skylake <https://en.wikipedia.org/wiki/Skylake_%28microarchitecture%29>`_ or newer for HEVC/H.265 hardware encoding. For Full HEVC You will need Kaby Lake or Coffee Lake CPU's.
- `NVIDIA Maxwell gen2 card <https://en.wikipedia.org/wiki/Maxwell_%28microarchitecture%29#Second_generation_Maxwell_.28GM20x.29>`_ or newer for HEVC/H.265 hardware encoding
- `AMD Polaris card <http://www.amd.com/en-gb/innovations/software-technologies/radeon-polaris>`_ or newer for HEVC/H.265 hardware encoding
- AviSynth+ x64 and VapourSynth, the installer is bundled with StaxRip. VapourSynth requires Python 3.6(R44).


License
-------

Licensed under the `MIT license <https://opensource.org/licenses/MIT>`_.


Download
--------

`Stable Releases <https://github.com/Revan654/staxrip/releases>`_

Before making a bug report please try the latest Version <https://github.com/Revan654/staxrip/blob/master/changelog.md>`_.


Setup
-----

StaxRip is for the most part portable, some features require certain apps to be installed, in such case StaxRip provides guidance so the user must not take great care.

Whenever StaxRip starts the first time from a new location it will ask for a settings folder location to use for this particular start-up location. You can open the current settings folder from the main menu at Tools > Folders > Settings

It's not recommended to start StaxRip from a location without full write access so 'C:\Program Files' shouldn't be used unless you have Admin Access. StaxRip utilizes dozens of tools and some of this tools might require write access to their start-up directory.

To add StaxRip to the windows start menu right-click the StaxRip.exe and choose *Add to Start*.