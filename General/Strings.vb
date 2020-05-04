
Public Class Strings
    Public Shared CharacterLimitReason As String = "In theory Windows supports paths that are longer than 260 characters, in reality neither Windows, nor the .NET Framework or the used tools have full long path support. StaxRip has a setting that allows to increase the limit but it's not recommended changing this limit! It will almost certainly not work!"
    Public Shared Muxer As String = "A muxer merges different video, audio and subtitle files into a single container file which is the actual output file. Using x264 or subtitles MKV or MP4 is required as container."
    Public Shared NoUnicode As String = $"Unicode filenames are not supported by AviSynth.{BR2}VapourSynth supports unicode, it can be enabled at:{BR2}Filters > Filter Setup > VapourSynth"
    Public Shared TaskDialogFooter As String = "[copymsg: Copy Message]"
End Class
