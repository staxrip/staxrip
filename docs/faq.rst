
==========================
Frequently Asked Questions
==========================

.. contents::


How can I encode with a fixed bitrate?
--------------------------------------

In the video encoder config dialog go to the Basic tab and choose a fixed bitrate mode like 2 pass.

At the bottom of the video encoder config dialog there is a drop down menu where a profile can be saved.


How can I batch encode with a fixed bitrate?
--------------------------------------------

StaxRip remembers if the file size or bitrate was edited last so if you edit the bitrate last it will encode using a fixed bitrate.


Why is encoding with 2 pass using a fixed bitrate not recommended?
------------------------------------------------------------------

Sources can vary greatly in complexity depending on the nature of the source, there still might be situations where fixed bitrates are useful.


Why don't settings persist?
---------------------------

StaxRip loads a default template on startup, templates can be saved with:

Main Menu > File > Save Project As Template

Project options are per Template/Project/Job and settings are global.


How can I use custom AviSynth and VapourSynth plugins?
------------------------------------------------------

Custom plugins can either be loaded manually using LoadPlugin():

http://avisynth.nl/index.php/Plugins

Or they can be loaded automatically using the plugin auto load folder.

This folder can be opened in StaxRip with:

Main Menu > Tools > Folders > Plugins

StaxRip is only available for x64 and therefore only x64 plugins can be used.

The AviSynth script can be edited manually using the code editor (Filters > Edit Code) or the filters menu can be configured using the filter profile editor (Filters > Profiles).


How can I use custom AviSynth scripts?
--------------------------------------

Custom scripts can either be loaded manually using Import():

http://avisynth.nl/index.php/Import

Or they can be loaded automatically using the plugin auto load folder.

This folder can be opened in StaxRip with:

Main Menu > Tools > Folders > Plugins

The AviSynth script can be edited manually using the code editor (Filters > Edit Code) or the filters menu can be configured using the filter profile editor (Filters > Profiles).


Why does AviSynth portable mode require soft links?
---------------------------------------------------

StaxRip has portable mode enabled by default because many users have incompatible AviSynth versions installed, portable mode can be disabled in the settings, if AviSynth is installed and portable mode is disabled then soft links are not necessary.

If AviSynth is installed and portable mode should be used instead of the installed version then it can only be achieved with soft links because there is no way to tell a tool from where it should load AviSynth, default DLL loading of the OS applies and that includes the system folder where AviSynth is installed. A tool needs an implementation to use a custom/portable path and right now not all tools have such an implementation.

If AviSynth is not installed then only ffmpeg needs a soft link because ffmpeg blocks default DLL loading of the OS which searches in the PATH environment variable which StaxRip sets in portable mode.


Where can I find the log file?
------------------------------

The StaxRip log file ends with _staxrip.log and can be found in the temp folder, by default the temp folder is located next to the source file.

If a project is re-opened from the Recent menu, then the log file can be opened from the main menu:

File > Recent Projects

Tools > Log File (F7)

The log file history can be found at:

Tools > Folders > Log Files
