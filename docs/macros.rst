
======
Macros
======

Macros are placeholders that can be used in all locations where StaxRip allows to customize command lines and scripts.

Whenever StaxRip starts a process and shell execute is disabled it passes all macros as environment variables to the process.


Interactive Macros
------------------

Interactive macros can be used in certain menus like the filter profiles menu.

.. csv-table::
    :header: "Name", "Description"
    :widths: auto

    "$browse_file$","Filepath returned from a file browser."
    "$enter_text$","Text entered in a input box."
    "$enter_text:prompt$","Text entered in a input box."
    "$select:param1;param2;...$","String selected from dropdown, to show a optional message the first parameter has to start with msg: and to give the items optional captions use caption|value. Example: $select:msg:hello;caption1|value1;caption2|value2$"


Command Line Audio Encoder
--------------------------

StaxRip supports 2 types of audio encoding profiles, a GUI based profile used for the default audio profiles and a Command Line based profile which supports following macros:

==================  =====================
Name                Description
==================  =====================
%input%             Audio source file
%output%            Audio target File
%bitrate%           Audio bitrate
%delay%             Audio delay
%channels%          Audio channels count
%language_native%   Native language name
%language_english%  English language name
==================  =====================


Global Macros
-------------

.. include:: generated/macro-table.rst
