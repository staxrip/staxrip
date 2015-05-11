Public Class Strings
    Public Const BatchMode As String = "Normally StaxRip performs various tasks directly after a source file is opened like demuxing, auto crop, auto resize etc. In Batch Mode all this tasks are performed when the encoding is started."
    Public Const EventCommands As String = "A Event Command allows to define a command to be executed on a defined event. Furthermore criteria can be defined to execute the command only if certain criteria is matched."
    Public Const MacrosHelp As String = "Macros are placeholders that can be used in command lines and other places, on execution StaxRip replaces the macros with the actual values."
    Public Const DonationsURL As String = "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=L7R6AKUHJQLM6&lc=EN&no_note=1&no_shipping=1&currency_code=EUR&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted"
    Public Const TaskDialogFooter As String = "[copymsg: Copy Message]"
    Public Const ParRegexPattern As String = "^([1-9]+\d*)(/|:)([1-9]+\d*)$"
    Public Const VisitForum As String = "Please copy the message and post it to the support forum or mail it."
    Public Const NoUnicode As String = "Unicode filenames are not supported by AviSynth."
    Public Const InstallManually As String = "Application not found, please click the 'Website' toolbar button above and download and install the application manually."
    Public Const ProjectX As String = "ProjectX demuxes and fixes DVB MPEG-2 captures. It's possible to disable ProjectX in the settings, in that case StaxRip uses DGMPGDec instead which normally should work well. ProjectX requires Java x64 or x86."
    Public Const dsmux As String = "dsmux is installed by the Haali Splitter and is used to mux TS containing AVC into MKV. dsmux and DGDecNV are excelent tools for keeping audio properly synced of TS files containing AVC." + CrLf2 + "If you don't use TS containing AVC you can prevent StaxRip asking to install dsmux (Haali Splitter) by disabling dsmux at Tools/Settings/Demux." + CrLf2 + "LAV Filters and Haali Splitter overrite each other, most people prefer LAV Filters, therefore it's recommended to install Haali first and LAV Filters last."
    Public Const Muxer As String = "A muxer merges different video, audio and subtitle files into a single container file which is the actual output file. Using x264 or subtitles MKV or MP4 is required as container."
    Public Const DGDecNV As String = "DGDecNV is similar to DGMPGDec, it's shareware and requires a NVIDIA GPU and it supports H264/AVC. DGIndexNV can be enabled and configured at Tools/Setting/Demux."
    Public Const DGDecIM As String = "DGDecIM is similar to DGMPGDec, it's shareware and requires a Intel GPU and it supports H264/AVC. DGIndexIM can be enabled and configured at Tools/Setting/Demux."
    Public Const DGMPGDec As String = "DGMPGDec is a AviSynth source filter plugin. DGIndex opens MPEG-2 files (VOB, TS, MPG) and creates a D2V index file which can then be opened in AviSynth with the MPEG2Source filter. DGIndex can also demux audio and video"

    Public Shared Intel As String = <div>
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