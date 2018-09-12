Friend Class Strings
    Public Const EventCommands As String = "A Event Command allows to define a command to be executed on a defined event. Furthermore criteria can be defined to execute the command only if certain criteria is matched."
    Public Shared BatchMode As String = "Normally StaxRip performs various tasks directly after a source file is opened like demuxing, auto crop, auto resize etc. In Batch Mode all this tasks are performed when the encoding is started."
    Public Shared MacrosHelp As String = "Macros are placeholders that can be used in command lines, scripts and other places, on execution StaxRip expands/replaces the macros with the actual values." + BR2 + "GUI Macros like $browse_file$ are typically used In custom menus like the video filter profiles menu."
    Public Shared TaskDialogFooter As String = "[copymsg: Copy Message]"
    Public Shared VisitForum As String = "Please copy the message and post it to the support forum or mail it."
    Public Shared NoUnicode As String = $"Unicode filenames are not supported by AviSynth.{BR2}VapourSynth supports unicode, it can be enabled at:{BR2}Filters > Filter Setup > VapourSynth"
    Public Shared dsmux As String = "dsmux is installed by the Haali Splitter and is used to mux TS containing AVC into MKV in order to fix av sync problems, dsmux handles av sync much better then many other TS tools. dsmux can be enabled or disabled in the settings on the preprocessing tab, if no audio is present or DGDecNV/DGDecIM is used, dsmux is not necessary and skipped automatically. LAV Filters and Haali Splitter overrite each other, most people prefer LAV Filters, therefore it's recommended to install Haali first and LAV Filters last."
    Public Shared Muxer As String = "A muxer merges different video, audio and subtitle files into a single container file which is the actual output file. Using x264 or subtitles MKV or MP4 is required as container."
    Public Shared DGDecNV As String = "DGDecNV is a shareware AviSynth source filter using NVIDIA hardware acceleration. DGIndexNV can be configured at Tools > Setting > Demux. DGDecNV is not included so must be downloaded manually."
    Public Shared DGDecIM As String = "DGDecIM is a shareware AviSynth source filter using Intel powered hardware acceleration. DGIndexIM can be enabled and configured at Tools/Setting/Demux. Which file types DGIndexIM handles can be configured. DGIndexIM can demux audio with proper av sync."
    Public Shared DGMPGDec As String = "StaxRip x64 uses DGIndex only for demuxing because the AviSynth source filter does not work on Win10. DGIndex can be enabled at Tools/Settings/Demuxing. Which file types DGIndex handles can be configured. DGIndex can process TS keeping audio in sync, an alternative method to process TS is using dsmux from Haali splitter which converts TS to MKV, it does this with better AV sync then mkvmerge."
    Public Shared DonationsURL As String = "https://www.paypal.me/stax76"

    Friend Shared Intel As String = <div>
                                        <b>CBR: constant bitrate control algorithm</b>
                                        <p>Use case: streaming or recording with a constant bit rate.</p>
                                        <p>Description: Bitrate is determined by the "Max Bitrate" setting in Encoding->Video Encoding or TargetKbps if "use global Max Bitrate" in Quick Sync Encoder Settings is unchecked.</p>
                                        <p>Quality is determined by the bitrate. The higher the bitrate, the better the quality.</p>

                                        <b>VBR: variable bit rate control algorithm</b>
                                        <p>Use case: realtime, recording and streaming with a variable bitrate</p>
                                        <p>Description: Video is encoded with target bitrate TargetKbps, while allowing MaxKbps spikes that smooth out using a buffer, whose size is determined by the global buffer size or BufferSizeInKB.</p>
                                        <p>Remark: According to the SDK description, this algorithm is HRD compliant and can probably be used fine with streaming, although it is not a strict constant bit rate algorithm.</p>
                                        <p>Quality is determined by the 2 bitrate parameters. The higher the bitrate, the better the quality.</p>

                                        <b>CQP: constant quantization parameter algorithm</b>
                                        <p>Use case: recording with a variable bit rate</p>
                                        <p>Description: Video is encoded with a constant quality, regardless of motion. Bitrate is determined by the complexity of the video material. Quality is determined by QPI, QPP and QPB parameters. QPI determines intra frame quality (known as key frames), QPP determines predicted (P-) frames quality, and QPB determines h.264 B-frames (a weaker version of P-frames). You want to use the same value for all 3 parameters.</p>
                                        <p>Remark: Very good for local recording for raw footage, since quality is not restricted to bitrate if you set the bitrate high enough, while not wasting disk space on low complex scenes.</p>
                                        <p>Quality is determined by the 3 QP parameters (1..51). The lower the values, the better the quality. 0 uses SDK default. Sweet spot is probably around 20-25. My choice is 22 with a maximum bit rate of 50000, which results in an average bit rate of about 29000 while keeping high complex scenes at maximum quality. Lower values than 22 greatly increases the bitrate requirement.</p>
                                        <p>Not available in every HD Graphics.</p>

                                        <b>AVBR: average variable bit rate control algorithm</b>
                                        <p>Use case: recording with a variable bit rate.</p>
                                        <p>Description: The algorithm focuses on overall encoding quality while meeting the specified bitrate, TargetKbps, within the accuracy range Accuracy, after a Convergence period.</p>
                                        <p>Quality is determined by the bitrate. The higher the bitrate, the better the quality. Allows spikes in bitrate consumption for short high complex scenes to maintain quality. Quality of high complex scenes is determined by the Accuracy and Convergence parameters. This is not constant enough for streaming like the VBR algorithm (not HRD compliant).</p>
                                        <p>Not available in every HD Graphics.</p>

                                        <b>LA (VBR): VBR algorithm look ahead</b>
                                        <p>Use case: recording with a variable bit rate.</p>
                                        <p>Description: A better version of the VBR algorithm. Quality improvements over VBR. Huge latency and increased memory consumption due to extensive analysis of several dozen frames before the actual encoding.</p>
                                        <p>Quality is determined by the bitrate and the Look-Ahead depth (1..100). The higher the bitrate, the better the quality. The larger the Look-Ahead, the better the quality. A LA value of 0 gives the SDK default.</p>
                                        <p>Not available in every HD Graphics.</p>

                                        <b>ICQ: intelligent constant quality algorithm</b>
                                        <p>Use case: recording with a variable bit rate.</p>
                                        <p>Description: A better version of the CQP mode. Recording with a constant quality, similar to the CRF mode of x.264, that makes better usage of the bandwith than CQP mode.</p>
                                        <p>Quality is determined by the ICQQuality parameter (1..51), where 1 corresponds to the best quality. Sane values of ICQQuality are probably around 20..25 like in CQP modes.</p>
                                        <p>Not available in every HD Graphics.</p>

                                        <b>LA ICQ: intelligent constant quality algorithm with look ahead</b>
                                        <p>Use case: recording with a variable bit rate.</p>
                                        <p>Description: The same quality improvements and caveats as LA (VBR), but for the ICQ algorithm. Probably the best quality while at the same time consuming the least bandwidth of all available algorithms - if your CPU supports it.</p>
                                        <p>Quality is determined by the ICQQuality parameter (1..51), where 1 corresponds to the best quality, and the Look-Ahead depth (1..100). The larger the Look-Ahead, the better the quality, where 0 gives the SDK default. Sane values of ICQQuality are probably around 20..25 like in CQP modes.</p>
                                        <p>Not available in every HD Graphics.</p>

                                        <b>VCM: Video conferencing mode</b>
                                        <p>Use case: video conferencing. Probably low latency, low bitrate requirements, low quality.</p>
                                        <p>Description: I did not find any details about this mode, but it is probably not suitable for high quality streaming or recording but focuses on low bandwith usage and robustness of the data stream.</p>
                                        <p>Not available in every HD Graphics.</p>
                                    </div>.ToString
End Class