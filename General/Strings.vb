
Friend Class Strings
    Public Const EventCommands As String = "A Event Command allows to define a command to be executed on a defined event. Furthermore criteria can be defined to execute the command only if certain criteria is matched."
    Public Shared BatchMode As String = "Normally StaxRip performs various tasks directly after a source file is opened like demuxing, auto crop, auto resize etc. In Batch Mode all this tasks are performed when the encoding is started."
    Public Shared MacrosHelp As String = "Macros are placeholders that can be used in command lines, scripts and other places, on execution StaxRip expands/replaces the macros with the actual values." + BR2 + "GUI Macros like $browse_file$ are typically used In custom menus like the video filter profiles menu."
    Public Shared TaskDialogFooter As String = "[copymsg: Copy Message]"
    Public Shared NoUnicode As String = $"Unicode filenames are not supported by AviSynth.{BR2}VapourSynth supports unicode, it can be enabled at:{BR2}Filters > Filter Setup > VapourSynth"
    Public Shared Muxer As String = "A muxer merges different video, audio and subtitle files into a single container file which is the actual output file. Using x264 or subtitles MKV or MP4 is required as container."
    Public Shared DGDecNV As String = "Shareware source filter with NVIDIA hardware acceleration and reliable transport stream support. DGIndexNV can be configured at Tools > Setting > Demux."
    Public Shared DGMPGDec As String = "StaxRip x64 uses DGIndex only for demuxing because the AviSynth source filter does not work on Win10. DGIndex can be enabled at Tools/Settings/Demuxing. Which file types DGIndex handles can be configured. DGIndex can process TS keeping audio in sync, an alternative method to process TS is using dsmux from Haali splitter which converts TS to MKV, it does this with better AV sync then mkvmerge. MPEG-2 can also be processed with ProjectX."
End Class
