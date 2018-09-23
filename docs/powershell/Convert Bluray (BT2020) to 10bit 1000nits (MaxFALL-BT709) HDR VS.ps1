$code = @"
# Convert Bluray Video (BT2020) to 10bit 1000nits (BT.709) High Dynamic Range Video for Full HDR10.
# Don't Forget to Disable No Open GOP and Set your VBV.

clip = core.fmtc.resample(clip, css = '444')
clip = core.fmtc.matrix(clip, mat = '2020', fulls = False, fulld = True)
clip = core.fmtc.transfer(clip, transs = 'linear', transd = 'linear', gcor = 2.4)
clip = core.fmtc.transfer(clip, transs = 'linear', transd = '2084', cont = 1000 / 10000)
clip = core.fmtc.transfer(clip, transs = 'linear', transd = 'linear')
clip = core.fmtc.matrix(clip, mat = '709', fulls = True, fulld = False)
clip = core.fmtc.resample(clip, css = '420')
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
$commands.ImportVideoEncoderCommandLine("--output-depth 10 --colorprim bt2020 --colormatrix bt2020nc --transfer smpte2084 --master-display G(8500,39850)B(6550,2300)R(35400,14600)WP(15635,16450)L(10000000,1) --hrd --aud --repeat-headers --max-cll 1000,180")