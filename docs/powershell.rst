
====================
PowerShell Scripting
====================

StaxRip can be automated via PowerShell scripting.


Script Folder
-------------

The Script folder is located at ``<settings>\Scripts`` and can be opened from
the main menu under ``Tools > Scripts > Open Script Folder``.

Scripts located in this folder are not loaded automatically but rather can be
invoked from the main menu under ``Tools > Scripts``.


Auto Load Folder
----------------

Scripts located under ``<settings>\Scripts\Auto Load`` are loaded on startup.

A second startup folder is ``<startup>\Apps\Scripts``, this folder should only
be used by StaxRip maintainers.


Events
------

- ``JobMuxed`` After a job was muxed.
- ``JobProcessed`` After a job was processed.
- ``JobsProcessed`` After jobs were processed.
- ``ProjectLoaded`` After a project was loaded.
- ``ProjectOrSourceLoaded`` After a project or source was loaded.
- ``AfterSourceLoaded`` After source files were loaded.
- ``VideoEncoded`` After video was encoded.
- ``ApplicationExit`` When the application exits.
- ``BeforeJobProcessed`` Before job processing starts.
- ``BeforeProcessing`` Before processing starts.


Example Scripts
---------------

`https://github.com/staxrip/staxrip/tree/master/Scripts <https://github.com/staxrip/staxrip/tree/master/Scripts>`_ 
