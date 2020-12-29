
=====
Usage
=====

.. contents::

Preprocessing
=============

Demuxing
--------

StaxRip supports demuxing via MP4Box, mkvextrakt, ffmpeg, eac3to and DGMPGDec.

Supported are three different demuxing modes:

1. Everything is demuxed automatically.
2. Only audio and subtitle streams of preferred languages are automatically demuxed.
3. A dialog is shown where users can select which audio and subtitle streams should be demuxed.

The demuxing mode can be defined in the project options under the Audio and Subtitles tab.

When demuxing is disabled StaxRip will still find and include audio and subtitle tracks directly from the source file using a demux free mode, to prevent this the Preferred Languages option has to be cleared.

Custom preprocessing tools for demuxing, re-muxing or indexing can be integrated and configured via command line to either execute a preprocessing tool like a demuxer directly or with a scripting tool like PowerShell or Python. When a preprocessing command line executes, all console tools are added to the path environment variable and all macros are available as environment variables. Preprocessing can be customized in the Settings dialog under Preprocessing.


Video Processing
================

Video processing is supported via AviSynth and VapourSynth with AviSynth and VapourSynth being equally well supported.

The script code of AviSynth and VapourSynth can be edited directly or easily be generated via menu selection for which a profile system is available to integrate and customize custom filters and plugins.

With the help of a macro system script parameters can be changed with convenient GUI features like a resize slider and menu or a cropping dialog, due to the macro system the parameters can change at any time allowing much greater flexibility compared to a one dimensional and limiting one step after another approach.


Filter Profiles
---------------

StaxRip includes a large set of filter plugins and filter presets for the plugins. The presets can be selected in the filter menu.


Custom Filter Profiles
~~~~~~~~~~~~~~~~~~~~~~

The filter profiles editor can be used to customize the included filter profiles and to create new custom filter profiles.

The profile editor can be found at:

Filter Menu > Profiles

The filter profiles use the INI format:

.. code-block:: INI

    [Filter Type]

    Menu item name = Filter code

Cascading/sub menus:

.. code-block:: INI

    Top level | Level 2 | Level 3 = Filter code

Multi-line code:

.. code-block:: INI

    Menu name =
        code line 1
        code line 2

Many of the included presets use :ref:`interactive_macros` to show a selection box.

Example:

.. code-block:: INI

    [Field]

    yadifmod2 =
        LoadPlugin("D:\yadifmod2\yadifmod2.dll")
        yadifmod2()


Opening scripts with external apps
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Opening AviSynth and VapourSynth scripts with external apps
from outside of StaxRip might require adding various directories
to the PATH environment variable. The Apps dialog has a feature
that helps adding the directories:

Apps > Manage > Tools > Path Env Var

- AviSynth

  Legacy app like VirtualDub2 are VFW based and therefore
  require AviSynth being installed. For modern apps it's
  sufficient if the portalbe AviSynth folder is added to PATH.
  

Templates
=========

StaxRip uses a template system, technically a template is a empty project file. When StaxRip starts it loads a default template, this default template can be changed at:

*Main Menu > Tools > Settings > General > Startup Template*

Project options are saved in templates/projects while *Tools > Settings* are global settings.

A template can be saved using:

*Main Menu > File > Save Project As Template*


Video Encoding
==============

Supported encoders are:

- xvid
- x264
- x265
- nvenc
- qsvenc
- vceenc
- rav1e
- SVT-AV1
- ffmpeg


Parallel Processing
===================

Job Processing
--------------

The jobs list can be processed with multiple StaxRip instances in parallel. This feature is only recommended for power users that know exactly what their hardware is capable of.


Chunk Encoding
--------------

StaxRip supports chunk encoding for the x265 encoder, it splits the encoding into chunks and encodes the chunks in parallel. Only recommended for power users that know exactly what their hardware is capable of. This feature can be configured at:

x265 dialog > Other > Chunks

Main Menu > Tools > Settings > General > Maximum number of parallel processes


Batch Processing
================

For Batch Processing first wanted options have to be changed, after that files can be added at:

File > Open Video File > File Batch

Not only encoding is supported but any task like remuxing, demuxing or AviSynth/VapourSynth script generation, this can be achieved by disabling unwanted features, almost every feature can be disabled:

- The video encoder profiles have a Copy/Mux profile.
- The muxer profiles have a No Muxing profile.
- The audio profiles have a Copy/Mux and a No Audio profile.
- Indexing can be disabled by installing LAV Filters and by enabling the DSS2 source filter.
- Audio and subtitle demuxing can be disabled in the Options dialog.


Command Engine
==============

StaxRip uses a command engine for the following features:

- StaxRips command line interface
- StaxRips configurable main menu and various other menus
- StaxRips Event Command Feature (Main Menu > Tools > Advanced > Event Commands)

The Event Command feature allows to run commands on defined events under defined conditions.
