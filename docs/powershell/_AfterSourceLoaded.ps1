# This script handles the AfterSourceLoaded event, remove the underscore from the
# filename in order to enable it. The script sets a deinterlace filter if the
# MediaInfo property 'ScanType' returns 'Interlaced'

# active project
$p = [ShortcutModule]::p

#global object with miscelenius stuff
$g = [ShortcutModule]::g

if ([MediaInfo]::GetVideo($p.FirstOriginalSourceFile, "ScanType") -eq "Interlaced")
{
    $p.Script.SetFilter("yadifmod2", "Field", "yadifmod2()")
}