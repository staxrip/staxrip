PowerShell Scripting
====================

StaxRip can be automated via PowerShell scripting.


Code Examples
------------------

Sets a deinterlace filter if the MediaInfo property 'ScanType' returns 'Interlaced'::

	# active project
	$p = [ShortcutModule]::p

	#global object with miscelenius stuff
	$g = [ShortcutModule]::g

	if ([MediaInfo]::GetVideo($p.FirstOriginalSourceFile, "ScanType") -eq "Interlaced")
	{
	    $p.Script.SetFilter("yadifmod2", "Field", "yadifmod2()")
	}


Events
------

In order to run scripts on certain events the following events are available:

.. include:: events.rst


Support
-----------------

If you have questions feel free to ask here:

https://github.com/stax76/staxrip/issues/200