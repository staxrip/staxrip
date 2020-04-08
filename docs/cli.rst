
Command Line Interface
======================

Switches are processed in the order they appear in the command line.

The command line interface, the customizable main menu and Event Command features are built with a shared command system.

There is a special mode where only the MediaInfo window is shown using -mediainfo , this is useful for File Explorer integration with an app like Open++.


Examples
--------

StaxRip C:\\Movie\\project.srip

StaxRip C:\\Movie\\VTS_01_1.VOB C:\\Movie 2\\VTS_01_2.VOB

StaxRip -LoadTemplate:DVB C:\\Movie\\capture.mpg -StartEncoding -Standby

StaxRip -ShowMessageBox:"main text...","text ...",info


Switches
--------

.. include:: generated/switches.rst
