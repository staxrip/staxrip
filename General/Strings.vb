
Public Class Strings
    Public Shared EventCommands As String = "A Event Command allows to define a command to be executed on a defined event. Furthermore criteria can be defined to execute the command only if certain criteria is matched."
    Public Shared MacrosHelp As String = "Macros are placeholders that can be used in command lines, scripts and other places, on execution StaxRip expands/replaces the macros with the actual values." + BR2 + "GUI Macros like $browse_file$ are typically used In custom menus like the video filter profiles menu."
    Public Shared TaskDialogFooter As String = "[copymsg: Copy Message]"
    Public Shared NoUnicode As String = $"Unicode filenames are not supported by AviSynth.{BR2}VapourSynth supports unicode, it can be enabled at:{BR2}Filters > Filter Setup > VapourSynth"
    Public Shared Muxer As String = "A muxer merges different video, audio and subtitle files into a single container file which is the actual output file. Using x264 or subtitles MKV or MP4 is required as container."
End Class
