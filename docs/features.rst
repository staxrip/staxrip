Features
========

Preprocessing
-------------

StaxRip includes demuxing GUIs for MP4Box, mkvextrakt, ffmpeg and eac3to.

The app can automatically demux all streams, show a GUI to select which streams should be demuxed or demuxing can be disabled in which case the streams are demuxed while job processing or are processed without demuxing.

Automatic demuxing with DGMPGDec, DGDecNV and DGDecIM is supported as well.

Custom preprocessing tools for demuxing, re-muxing or indexing can be integrated and configured via command line.


Video Processing
----------------

Video processing is supported via AviSynth+ and VapourSynth with AviSynth+ and VapourSynth being equally well supported.

The script code of AviSynth+ and VapourSynth can be edited directly or easily be generated via menu selection for which a profile system is available to integrate and customize custom filters and plugins.

With the help of a macro system script parameters can be changed with convenient GUI features like a resize slider and menu or a cropping dialog, due to the macro system the parameters can change at any time allowing much greater flexibility compared to a one dimensional and limiting one step after another approach.

Over 60 up to date AviSynth and VapourSynth plugin are included and ready to use.


Templates
---------

StaxRip uses a template system, technically a template is a empty project file. When StaxRip starts it loads a default template, this default template can be changed at:

``Main Menu > Tools > Settings > General > Startup Template``

Project options are saved in templates/projects while Tools > Settings are global.

A template can be saved using:

``Main Menu > Project > Save As Template``


Command Engine
--------------

StaxRip uses a command engine for the following features:

- StaxRip's command line interface
- StaxRip's configurable main menu and various other menus
- StaxRip's Event Command Feature (Main Menu > Tools > Advanced > Event Commands)

The Event Command feature allows to run commands on defined events under defined conditions.

A interesting command is -Perform-ExecuteBatchScript

It executes a batch script with solved macros. Macros are also available as environment variables. Executables started by the batch script inherit the access to these environment variables.

The 'Execute Batch Script' command has a option 'Interpret Output' that can be turned on to interpret each console output line from the batch script or executables started in the batch script as StaxRip command. The documentation of the StaxRip commands is located at: Main Menu > Help > Command Line.

Used executables can be powershell.exe, cscript.exe (VBScript/JScript), python.exe, any other scripting engine or any console app programed in any programming language.

Thumbnails
----------

Along with Video Processing, You can make a contact sheet(aka Thumbnails) with StaxRip. You have access to two engines to do this MTN and the StaxRip internal engine. 
 
The internal engine is capable of covering HDR and all different formats of HDR. The MTN engine requires zero indexing and is the ideal engine to use when you need a quick contact sheet.

Animation
---------

Other then video encoding, StaxRip is capable of creating short high quality animation in the form of gif or aPNG (it's just a normal PNG file but animated). You can create quick animation from project your encoding or on the fly animation. 

Both types supports different types of options to set for your animation.