$code = @"
# Convert Bluray Video (BT2020) to 10bit 1000nits (BT.709) High Dynamic Range Video for Full HDR10.
# Don't Forget to Disable No Open GOP and Set your VBV.
#You can Use other Color Options like Tweak, Level or Range to make any other needed changes to the Color & Brightness(If Needed).

SetFilterMTMode("z_ConvertFormat", MT_MULTI_INSTANCE) # May not be needed.
ConvertBits(10)
z_ConvertFormat(pixel_type="RGBPS",colorspace_op="2020ncl:st2084:2020:l=>rgb:linear:2020:l", dither_type="none")
DGHable()
z_ConvertFormat(pixel_type="YV12",colorspace_op="rgb:linear:2020:l=>709:709:709:l",dither_type="ordered")
prefetch(4)

"@

$activeProject = [ShortcutModule]::p

if ($activeProject.Script.Engine -ne [ScriptEngine]::Avisynth) {
    [MainModule]::MsgError("Load Avisynth first", "Filters > Filter Setup > Avisynth")
    exit
}

if ($activeProject.VideoEncoder.GetType().Name -ne "x265Enc") {
    [MainModule]::MsgError("Load x265 first")
    exit
}

$commands = [ShortcutModule]::g.DefaultCommands
$commands.SetFilter("HDR", "Color", $code)
$commands.ImportVideoEncoderCommandLine("--output-depth 10 --colorprim bt2020 --colormatrix bt2020nc --transfer smpte2084 --master-display G(8500,39850)B(6550,2300)R(35400,14600)WP(15635,16450)L(10000000,1) --hrd --aud --repeat-headers --max-cll 1000,180")