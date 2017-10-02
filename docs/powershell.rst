PowerShell Scripting
====================

Scripting Events
----------------

In order to run scripts on certain events read the help at:

Main Menu > Help > Scripting


Scripting Support
-----------------

If you have questions feel free to ask here:

https://github.com/stax76/staxrip/issues/200


Scripting Examples
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