# This script handles the AfterSourceLoaded event, remove the underscore from the
# filename in order to enable it. The script sets QTGMC filter to type 0 (Interlaced) if the
# MediaInfo property 'ScanType' returns 'Interlaced'. 
# However if it's not 'interlaced' and the Scantype is Progressive it will set QTGMC to type 1 (Progressive) if the 
# MediaInfo property 'ScanType' returns 'Progressive'. 

# active project
$p = [ShortcutModule]::p

#global object with miscelenius stuff
$g = [ShortcutModule]::g

if ([MediaInfo]::GetVideo($p.FirstOriginalSourceFile, "ScanType") -eq "Interlaced")
{
    $p.Script.SetFilter("Field", "QTGMC Interlaced", "QTGMC(Preset = ""Medium"", InputType=0, SourceMatch=3, Sharpness=0.2, EdiThreads=8)")
}
elseif ([MediaInfo]::GetVideo($p.FirstOriginalSourceFile, "ScanType") -eq "Progressive")
{
	$p.Script.SetFilter("Field", "QTGMC Progressive", "QTGMC(Preset = ""Medium"", InputType=1, Sharpness=0.2, EdiThreads=8)")
}