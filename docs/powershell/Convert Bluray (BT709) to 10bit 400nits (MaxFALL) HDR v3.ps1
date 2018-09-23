$code = @"
# Convert Bluray Video (BT709) to 10bit 400nits (MaxFALL) High Dynamic Range Video.
# In some scenes, HDR output of this script saves as much as 50% bitrate in x265 CRF mode and doubles the quality in x265 2pass CBR mode.
# You must have a HDR compliant TV to play the output file.
# Script is 100% compliant with current HDR standards and has been tested extensively on HDR compliant TVs.
# Do NOT modify the script. Input and output are guaranteed to look 100% identical with default script settings.  All x265 HDR switches are automatically set by Staxrip.
# Current UHD Bluray and HDR TV specifications only support exactly 400nits MaxFALL. Not more, not less. Standards will not change for many years.
# Do NOT change MaxFALL. Hardware players and TVs will not correct/scale it for you. White level and brightness will be off.
# MaxFALL and MaxCLL are both equal to 400nits.  You can not independently set them.
# Do not change the gamma extraction to a value other than 2.2 specified in the script. It is NOT related to the gamma correction values used by the studios when mastering Bluray video. It is NOT related to the gamma settings of your monitor/TV either.
# We are extracting 2.2 gamma only because the exact same amount will be added back by SMPTE 2084.  Not more, not less.
# SMPTE 2084 is a specialized HDR perceptual quantizer that already has built in gamma encoding that is exactly equivalent to a 2.2 pure gamma function.

clip = core.fmtc.resample(clip, css = '444')
clip = core.fmtc.matrix(clip, mat = '709', fulls = False, fulld = True)
clip = core.fmtc.transfer(clip, transs = 'linear', transd = 'linear', gcor = 2.2)
clip = core.fmtc.transfer(clip, transs = 'linear', transd = '2084', cont = 400 / 10000)
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
$commands.ImportVideoEncoderCommandLine("--output-depth 10 --colorprim bt709 --colormatrix bt709 --transfer smpte-st-2084 --master-display G(15000,30000)B(7500,3000)R(32000,16500)WP(15635,16450)L(4000000,1) --max-cll 400,400")