# [Documentation](../README.md) / [Usage](README.md) / Macros

## Global Macros
| Name | Description |
|---|---|
| `%audio_bitrate%` | Overall audio bitrate. |
| `%audio_bitrateX%` | Audio bitrate of the X'th audio track. |
| `%audio_channelsX%` | Audio channels of the X'th audio track. |
| `%audio_codecX%` | Audio codec of the X'th audio track. |
| `%audio_delayX%` | Audio delay of the X'th audio track. |
| `%audio_fileX%` | File path of the X'th audio file. |
| `%compressibility%` | Compressibility value. |
| `%crop_bottom%` | Bottom crop value. |
| `%crop_height%` | Crop height. |
| `%crop_left%` | Left crop value. |
| `%crop_right%` | Right crop value. |
| `%crop_top%` | Top crop value. |
| `%crop_width%` | Crop width. |
| `%current_date%` | Returns the current date. |
| `%current_time%` | Returns the current time (12h). |
| `%current_time24%` | Returns the current time (24h). |
| `%dpi%` | DPI value of the main dialog. |
| `%empty%` | Empty character for special use like removing transferred values. |
| `%encoder%` | Name of the active video encoder. |
| `%encoder_codec%` | Codec that is used by the active video encoder. |
| `%encoder_ext%` | File extension of the format the encoder of the active project outputs. |
| `%encoder_out_file%` | Output file of the video encoder. |
| `%encoder_profile%` | Name of the selected video encoder profile name. |
| `%encoder_settings%` | Settings of the active video encoder. |
| `%jobs%` | Number of all jobs in Jobs List. |
| `%jobs_active%` | Number of active jobs in Jobs List. |
| `%muxer_ext%` | Output extension of the active muxer. |
| `%player%` | Path of the media player. |
| `%plugin_dir%` | AviSynth/VapourSynth plugin auto load directory. |
| `%pos_frame%` | Current preview position in frames. |
| `%pos_ms%` | Current preview position in milliseconds. |
| `%processing%` | Returns 'True' if a job is currently processing otherwise 'False'. |
| `%programs_dir%` | Programs system directory. |
| `%script_dir%` | Users PowerShell scripts directory. |
| `%script_ext%` | File extension of the AviSynth/VapourSynth script so either avs or vpy. |
| `%script_file%` | Path of the AviSynth/VapourSynth script. |
| `%sel_end%` | End position of the first selecion in the preview. |
| `%sel_start%` | Start position of the first selecion in the preview. |
| `%settings_dir%` | Path of the settings direcory. |
| `%source_dar%` | Source display aspect ratio. |
| `%source_dir%` | Directory of the source file. |
| `%source_dir_name%` | Name of the source file directory. |
| `%source_dir_parent%` | Parent directory of the source file directory. |
| `%source_ext%` | File extension of the source file. |
| `%source_file%` | File path of the source video. |
| `%source_files%` | Source files in quotes separated by a blank. |
| `%source_files_comma%` | Source files in quotes separated by comma. |
| `%source_framerate%` | Frame rate returned by the Source filter section with up to 6 decimal places, depending on digits, with no trailing zeros. |
| `%source_framerate6%` | Frame rate returned by the Source filter section with 6 fixed decimal places. |
| `%source_frames%` | Length in frames of the source video. |
| `%source_height%` | Image height of the source video. |
| `%source_mi_ac%` | Audio track count of the source video. |
| `%source_mi_tc%` | Text track count of the source video. |
| `%source_mi_v:Format%` | Video codec of the source file. |
| `%source_mi_vc%` | Video track count of the source video. |
| `%source_name%` | The name of the source file without file extension. |
| `%source_par_x%` | Source pixel/sample aspect ratio. |
| `%source_par_y%` | Source pixel/sample aspect ratio. |
| `%source_seconds%` | Length in seconds of the source video. |
| `%source_temp_file%` | File located in the temp directory using the same name as the source file. |
| `%source_width%` | Image width of the source video. |
| `%startup_dir%` | Directory of the application. |
| `%system_dir%` | System directory. |
| `%target_dar%` | Target display aspect ratio. |
| `%target_dir%` | Directory of the target file. |
| `%target_file%` | File path of the target file. |
| `%target_framerate%` | Frame rate of the target video with up to 6 decimal places, depending on digits, with no trailing zeros. |
| `%target_framerate6%` | Frame rate of the target video with 6 fixed decimal places. |
| `%target_frames%` | Length in frames of the target video. |
| `%target_height%` | Image height of the target video. |
| `%target_name%` | Name of the target file without file extension. |
| `%target_par_x%` | Target pixel/sample aspect ratio. |
| `%target_par_y%` | Target pixel/sample aspect ratio. |
| `%target_seconds%` | Length in seconds of the target video. |
| `%target_size%` | Size of the target video in kilo bytes. |
| `%target_temp_file%` | File located in the temp directory using the same name as the target file. |
| `%target_width%` | Image width of the target video. |
| `%temp_dir%` | Directory of the source file or the temp directory if enabled. |
| `%temp_file%` | File located in the temp directory using the same name as the source file. |
| `%template_name%` | Name of the template the active project is based on. |
| `%text_editor%` | Path of the application currently associated with TXT files. |
| `%version%` | StaxRip version. |
| `%video_bitrate%` | Video bitrate in Kbps. |
| `%working_dir%` | Directory of the source file or the temp directory if enabled. |

## Parameter Included Macros
| Name | Description |
|---|---|
| `%app:name%` | Returns the path of a given tool, it can be any type of tool found in the Apps dialog. Example: %app:x265% |
| `%app_dir:name%` | Returns the directory of a given tool, it can be any type of tool found in the Apps dialog. Example: %app_dir:x265% |
| `%app_path:name%` | Returns the path of a given tool, it can be any type of tool found in the Apps dialog. Example: %app:x265% |
| `%app_version:name%` | Returns the version of a given tool, it can be any type of tool found in the Apps dialog. Example: %version:x265% |
| `%eval:expression%` | Evaluates a PowerShell expression which may contain macros. |
| `%filter:name%` | Returns the script code of a filter of the active project that matches the specified name. |
| `%random:digits%` | Returns a 'digits' long random number, whereas 'digits' is clamped between 1 and 10. |
| `%source_mi_a:property%` | Returns the given MediaInfo property from the Audio section for the source file. Before ':' you can add a zero-based index for the track number of that section. |
| `%source_mi_g:property%` | Returns the given MediaInfo property from the General section for the source file. |
| `%source_mi_t:property%` | Returns the given MediaInfo property from the Text section for the source file. Before ':' you can add a zero-based index for the track number of that section. |
| `%source_mi_v:property%` | Returns the given MediaInfo property from the Video section for the source file. Before ':' you can add a zero-based index for the track number of that section. |

## Interactive Macros
Interactive macros can be used in certain menus like the filter profiles menu. They give you the ability to pop up dialogs in which you have to put values in or select an option.
| Name | Description |
|---|---|
| `$browse_file$` | Filepath returned from a file browser. |
| `$enter_text$` | Text entered in an input box. |
| `$enter_text:prompt$` | Text entered in an input box. |
| `$select:param1;param2;...$` | String selected from dropdown, to show an optional message the first parameter has to start with msg: and to give the items optional captions use caption|value. Example: $select:msg:hello;caption1|value1;caption2|value2$ |

## Special Macros

### Encoder Macros
When choosing the option to override the target file name via the encoder options, you have the possibility to extend the normal macros with encoder specific ones.  
The syntax is: `%parameter%`  
**Note**: There are some special parameters, that might be not supported (yet). In such cases you can open a feature request.

Examples:
| Encoder | Example | Result |
|---|---|---|
| x265 | `%source_name%_CRF%--crf%_AQ-Mode%--aq-mode%` | `%source_name%_CRF23_AQ-Mode2` |
| SvtAv1EncApp | `%source_name%_CRF%--crf%_Predict%--pred-struct%` | `%source_name%_CRF35_Predict2` |
| SvtAv1EncApp | `%source_name%_CRF%--crf%_VBoost%--enable-variance-boost%` | `%source_name%_CRF35_VBoost1` |

### While Processing Macros
These macros are only available while a process is running. They can be used for the `While Processing` event, for example to call an API.
| Name | Description |
|---|---|
| `%commandline%` | Returns the command line used for the running app. |
| `%progress%` | Returns the current progress as Integer value. |
| `%progressline%` | Returns the progress line received from the running app. |
