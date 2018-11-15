$code = @"
# HDR to 10bit 1000nits (BT.2100) High Dynamic Range Video for Full HDR10.
# This is not Designed to Scale HDR to SDR.
# Don't Forget to Check all the Flags Before Starting.
# You can Use other Color Options like Tweak, Level or Range to make any other needed changes to the Color & Brightness(If Needed).
"@

$activeProject = [ShortcutModule]::p

if ($activeProject.Script.Engine -ne [ScriptEngine]::VapourSynth) {
    [MainModule]::MsgError("Load VapourSynth first", "Filters > Filter Setup > VapourSynth")
    exit
}

if ($activeProject.VideoEncoder.GetType().Name -ne "x265Enc") {
    [MainModule]::MsgError("Load x265 first")
    exit
}

$commands = [ShortcutModule]::g.DefaultCommands
$commands.SetFilter("HDR", "Color", $code)
$commands.ImportVideoEncoderCommandLine("--output-depth 10 --hdr --colorprim bt2020 --colormatrix bt2020nc --transfer smpte2084 --master-display G(8500,39850)B(6550,2300)R(35400,14600)WP(15635,16450)L(10000000,1) --hrd --aud --repeat-headers --max-cll 1000,180")