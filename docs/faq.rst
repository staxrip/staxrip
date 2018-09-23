Frequently Asked Questions
==========================

.. contents::

How can I encode with a fixed bitrate?
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the video encoder config dialog go to the Basic tab and choose a fixed bitrate mode like 2 pass.

At the bottom of the video encoder config dialog there is a drop down menu where a profile can be saved.


How can I batch encode with a fixed bitrate?
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

StaxRip remembers if the file size or bitrate was edited last so if you edit the bitrate last it will encode using a fixed bitrate.


Why is encoding with 2 pass using a fixed bitrate not recommended?
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Sources can vary greatly in complexity depending on the nature of the source, there still might be situations where fixed bitrates are useful.


Why don't settings persist?
~~~~~~~~~~~~~~~~~~~~~~~~~~~

StaxRip loads a default template on startup, templates can be saved with:

Main Menu > Project > Save As Template

Project options are per Template/Project/Job and settings are global.


How can I use custom AviSynth plugins?
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Custom plugins can either be loaded manually using LoadPlugin():

http://avisynth.nl/index.php/Plugins

Or they can be loaded automatically using the plugin auto load folder.

This folder can be opened in StaxRip with:

Main Menu > Tools > Folders > Plugins

StaxRip is only available for x64 and therefore only x64 plugins can be used.

The AviSynth script can be edited manually using the code editor (Filters > Edit Code) or the filters menu can be configured using the filter profile editor (Filters > Profiles).

New LoadPlugin / Import filter can be loaded through Filters tab in Option Menu.


How can I use custom AviSynth scripts?
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Custom scripts can either be loaded manually using Import():

http://avisynth.nl/index.php/Import

Or they can be loaded automatically using the plugin auto load folder.

This folder can be opened in StaxRip with:

Main Menu > Tools > Folders > Plugins

The AviSynth script can be edited manually using the code editor (Filters > Edit Code) or the filters menu can be configured using the filter profile editor (Filters > Profiles).