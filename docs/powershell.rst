PowerShell Scripting
====================

StaxRip can be automated via PowerShell scripting.


Events
------

In order to run scripts on certain events the following events are available:

- ``ProjectLoaded`` After Project Loaded
- ``JobProcessed`` After Project Processed
- ``VideoEncoded`` After Video Encoded
- ``BeforeJobProcessed`` Before Job Processed
- ``AfterSourceLoaded`` After Source Loaded
- ``ApplicationExit`` Application Exit
- ``ProjectOrSourceLoaded`` After Project Or Source Loaded
- ``JobsEncoded`` After Jobs Encoded

Assign to an event by saving a script file in the scripting folder using the event name as file name.

The scripting folder can be opened with:

Main Menu > Tools > Scripts > Open script folder

Use one of the following file names:

- ProjectLoaded.ps1
- JobProcessed.ps1
- VideoEncoded.ps1
- BeforeJobProcessed.ps1
- AfterSourceLoaded.ps1
- ApplicationExit.ps1
- ProjectOrSourceLoaded.ps1
- JobsEncoded.ps1

Support
-------

If you have questions feel free to ask here:

https://github.com/stax76/staxrip/issues/200


Default Scripts
---------------

HDR to 10bit 1000nits(Rec.2100)AVS.ps1
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

.. literalinclude:: powershell/HDR to 10bit 1000nits(Rec.2100)AVS.ps1
   :language: powershell

HDR to 10bit 1000nits(Rec.2100)VS.ps1
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

.. literalinclude:: powershell/HDR to 10bit 1000nits(Rec.2100)VS.ps1
   :language: powershell

Re-mux v4.ps1
~~~~~~~~~~~~~

.. literalinclude:: powershell/Re-mux v4.ps1
   :language: powershell

_AfterSourceLoaded.ps1
~~~~~~~~~~~~~~~~~~~~~~

.. literalinclude:: powershell/_AfterSourceLoaded.ps1
   :language: powershell

