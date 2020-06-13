
======================
Command Line Interface
======================

.. note:: Switches are processed in the order they appear in the command line.


Examples
--------

.. code::

    StaxRip C:\Movie\project.srip
    
    StaxRip C:\Movie\VTS_01_1.VOB C:\Movie 2\VTS_01_2.VOB
    
    StaxRip -LoadTemplate:DVB C:\Movie\capture.mpg -StartEncoding -Standby
    
    StaxRip -ShowMessageBox:"title ...","text ...",info


Rules
-----

Strings don't have to be enclosed in quotes unless they contain spaces.


Commands
--------

.. include:: generated/switches.rst
