Usage
=====

Preprocessing
-------------

Supported are three different demuxing modes.

1. Everything is demuxed automatically.
2. Only audio and subtitle streams of preferred languages are automatically demuxed.
3. A dialog is shown where users can select which audio and subtitle streams should be demuxed.

StaxRip supports demuxing via MP4Box, mkvextrakt, ffmpeg, eac3to, DGMPGDec and DGDecNV.

Custom preprocessing tools for demuxing, re-muxing or indexing can be integrated and configured via command line to either execute a preprocessing tool like a demuxer directly or with a scripting tool like powershell.exe or python.exe, all console tools are added to the path environment variable and all macros are available as environment variables.


Video Processing
----------------

Video processing is supported via AviSynth+ and VapourSynth with AviSynth+ and VapourSynth being equally well supported.

The script code of AviSynth+ and VapourSynth can be edited directly or easily be generated via menu selection for which a profile system is available to integrate and customize custom filters and plugins.

With the help of a macro system script parameters can be changed with convenient GUI features like a resize slider and menu or a cropping dialog, due to the macro system the parameters can change at any time allowing much greater flexibility compared to a one dimensional and limiting one step after another approach.


Templates
---------

StaxRip uses a template system, technically a template is a empty project file. When StaxRip starts it loads a default template, this default template can be changed at:

``Main Menu > Tools > Settings > General > Startup Template``

Project options are saved in templates/projects while Tools > Settings are global settings.

A template can be saved using:

``Main Menu > Project > Save As Template``


Video Encoding
--------------

Supported encoders are:

- x264
- x265
- rav1e
- SVT-AV1
- nvenc
- qsvenc
- vceenc
- ffmpeg
- xvid


Command Engine
--------------

StaxRip uses a command engine for the following features:

- StaxRip's command line interface
- StaxRip's configurable main menu and various other menus
- StaxRip's Event Command Feature (Main Menu > Tools > Advanced > Event Commands)

The Event Command feature allows to run commands on defined events under defined conditions.
