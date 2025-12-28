# [Documentation](../README.md) / [Usage](README.md) / Macros

Macros can be used almost everywhere in StaxRip and give you the ability to to set dynamic values where needed or wanted,
for example in scripts or settings. Most of them are also used for [events](Events.md) as criteria.

- [Global Macros](Macros.md#global-macros)
- [Parameter Included Macros](Macros.md#parameter-included-macros)
- [Interactive Macros](Macros.md#interactive-macros)
- [Special Macros](Macros.md#special-macros)
    - [Encoder Macros](Macros.md#encoder-macros)
    - [While Processing Macros](Macros.md#while-processing-macros)
    - [Audio Track Macros](Macros.md#audio-track-macros)
- [Function Macros](Macros.md#function-macros)
- [Appendix](Macros.md#appendix)
    - [Appendix A: DataColumn Expression Syntax](Macros.md#appendix-a-datacolumn-expression-syntax)

-----

## Global Macros

| Name | Description |
|---|---|
| `%audio_bitrate%` | Overall audio bitrate. |
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
| `%muxer_ext%` | Output extension of the active muxer. |
| `%jobs%` | Number of all jobs in Jobs List. |
| `%jobs_active%` | Number of active jobs in Jobs List. |
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
| `%source_name%` | The name of the source file without file extension. |
| `%source_par_x%` | Source pixel/sample aspect ratio. |
| `%source_par_y%` | Source pixel/sample aspect ratio. |
| `%source_seconds%` | Length in seconds of the source video. |
| `%source_temp_file%` | File located in the temp directory using the same name as the source file. |
| `%source_width%` | Image width of the source video. |
| `%source_mi_v:Format%` | Video codec of the source file. |
| `%source_mi_vc%` | Video track count of the source video. |
| `%source_mi_ac%` | Audio track count of the source video. |
| `%source_mi_tc%` | Text track count of the source video. |
| `%startup_dir%` | Directory of the application. |
| `%system_dir%` | System directory. |
| `%target_bitdepth%` | Target bit depth. |
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
| `%app_dir:name%` | Returns the directory of a given tool, it can be any type of tool found in the Apps dialog. Example: %app_dir:x265% |
| `%app_path:name%` | Returns the path of a given tool, it can be any type of tool found in the Apps dialog. Example: %app_path:x265% |
| `%app_version:name%` | Returns the version of a given tool, it can be any type of tool found in the Apps dialog. Example: %app_version:x265% |
| `%audio_bitrate:track%` | Audio bitrate of the track'th audio track. |
| `%audio_channels:track%` | Audio channels of the track'th audio track. |
| `%audio_codec:track%` | Audio codec of the track'th audio track. |
| `%audio_delay:track%` | Audio delay of the track'th audio track. |
| `%audio_file:track%` | File path of the track'th audio file. |
| `%eval:expression%` | Evaluates a PowerShell expression which may contain macros. |
| `%filter:name%` | Returns the script code of a filter of the active project that matches the specified name. |
| `%isfilteractive:name%` | Returns `1` if the filter is active, otherwise `0`. |
| `%random:digits%` | Returns a 'digits' long random number, whereas 'digits' is clamped between `1` and `10`. |
| `%source_mi_g:property%` | Returns the given MediaInfo property from the General section for the source file. |
| `%source_mi_v:property%` | Returns the given MediaInfo property from the Video section for the source file. Before `:` you can add a zero-based index for the track number of that section. |
| `%source_mi_a:property%` | Returns the given MediaInfo property from the Audio section for the source file. Before `:` you can add a zero-based index for the track number of that section. |
| `%source_mi_t:property%` | Returns the given MediaInfo property from the Text section for the source file. Before `:` you can add a zero-based index for the track number of that section. |

## Interactive Macros
Interactive macros can be used in certain menus like the filter profiles menu. They give you the ability to pop up dialogs in which you have to put values in or select an option.

| Name | Description |
|---|---|

## Special Macros

### Encoder Macros
When choosing the option to override the target file name via the encoder options, you have the possibility to extend the normal macros with encoder specific ones.  
The syntax is: `%parameter%`  
> :information_source: There are some special parameters, that might be not supported (yet). In such cases you can open a [feature request](https://github.com/staxrip/staxrip/issues/new?template=request-a-feature.md).

Examples:

| Encoder | Example | Result |
|---|---|---|
| x265 | `%source_name%_CRF%--crf%_AQ-Mode%--aq-mode%` | `%source_name%_CRF23_AQ-Mode2` |
| SvtAv1EncApp | `%source_name%_CRF%--crf%_Predict%--pred-struct%` | `%source_name%_CRF35_Predict2` |
| SvtAv1EncApp | `%source_name%_CRF%--crf%_VBoost%--enable-variance-boost%` | `%source_name%_CRF35_VBoost1` |

Encoder related parameters can be extended with some modifiers to modify the output of the values.

| Modifier | Syntax | Example | Result | Note |
|---|---|---|---|---|
| ` ` | `%parameter%`  | `%--preset%`  | `VeryFast` | No modifier, normal value without spaces |
| `D` | `%parameter_D%` | `%--preset_D%` | `True`     | `True` if the default value is set, otherwise `False` |
| `L` | `%parameter_L%` | `%--preset_L%` | `veryfast` | Value in lowercase without spaces |
| `T` | `%parameter_T%` | `%--preset_T%` | `Veryfast` | Value in Title-case (Only first letter in uppercase) without spaces |
| `U` | `%parameter_U%` | `%--preset_U%` | `VERYFAST` | Value in uppercase without spaces |
| `V` | `%parameter_V%` | `%--preset_V%` | `2`        | Returns the numeric value of the option list,<br>`1` for checked checkboxes, otherwise `0`<br>[Only available for checkboxes and option lists] |
| `Z` | `%parameter_Z%` | `%--preset_Z%` | `0`        | `1` in case it is active/visible, otherwise `0` |

> :warning: All modifier letters are **case sensitive**!

### While Processing Macros
These macros are only available while a process is running. They can be used for the `While Processing` event, for example to call an API.

| Name | Description |
|---|---|
| `%commandline%` | Returns the command line used for the running app. |
| `%progress%` | Returns the current progress as Integer value. |
| `%progressline%` | Returns the progress line received from the running app. |

### Audio Track Macros
These macros are only available in audio processing, for example inside the Audio Options.

| Name | Description |
|---|---|
| `%input%` | Audio source file. |
| `%output%` | Audio target file. |
| `%bitrate%` | Audio bitrate. |
| `%channels%` | Audio channels count. |
| `%delay%` | Audio delay. |
| `%language%` | Language name. |
| `%language_native%` | Native language name. |
| `%language_english%` | English language name. |
| `%streamid0%` | ID of the stream (starts with 0). |
| `%streamid1%` | ID of the stream (starts with 1). |

## Function Macros
Function macros are similar to *normal* macros, but they don't just replace a text with a specific value; they can perform an action like a condition.
This makes it possible to return different strings depending on the expression, especially when cascaded/nested. The following table and the examples below should give you enough explanation and ideas.

- `<if(<expression>;<true>;<false>)>`
  - **\<expression\>** corresponds to the [*Expression Syntax*](Macros.md#appendix-a-datacolumn-expression-syntax)
    - Can also include another (nested) function
  - **\<true\>** or **\<false\>** is returned depending on whether the expression is true or false
    - Can be another (nested) function, a macro or any text
    - First whitespace after `;` will be ignored
  - Examples with `--preset VeryFast`:
    - `<if('%--preset%' = 'Medium'; Default; %--preset%)>`
    - `<if('%--preset_V%' = 2; Default; %--preset%)>`
    - `<if('%--preset_V%' <> 0; %--preset%; 'PresetIs0')>`
    - `<if('%--preset_V%' IN (0,1,2); Faster than Faster; Faster or slower)>`
    - `<if('%--preset%' LIKE '*Fast*'; Includes 'Fast'; SomethingElse)>`
    - `<if(%target_height% > 1080; 4K; <if(%target_height% > 720; FullHD; SD)>)>`
    - `<if('%source_mi_g:Title%' <> ''; Original Title: %source_mi_g:Title%; <if('%source_mi_g:Movie%' <> ''; Original Title: %source_mi_g:Movie%; '')>)>`
    - `<if(%--sao%; SAO; NoSAO)>`
    - `<if(%--sao% AND NOT %--limit-sao%; FullSAO; <if(%--sao% AND %--limit-sao%; RestrictedSAO; NoSAO)>)>`
    - `<if('%source_mi_v:Framerate%' >= 48; HiFPS; '')>`
    - `<if(CONVERT(%source_mi_v:FrameRate%, 'System.Decimal') >= 48; HighFPS; '')>`
- `<lower(<input>)>`
  - **\<input\>** is any text you want to have in lower case
  - Examples:
    - `<lower(StaxRip is COOL)>` => `staxrip is cool`
- `<replace(<input>;<oldText>;<newText>)>`
  - **\<input\>** is any text you want to replace parts of
  - **\<oldText\>** is the text or macro that shall be replaced
    - First whitespace after `;` will be ignored
  - **\<newText\>** is the text or macro that shall be used for the replacement
    - First whitespace after `;` will be ignored
  - Examples:
    - `<replace(This is any text you like; you; I)>`
    - `<replace(%source_name%; 1080p; 720p)>`
- `<title(<input>)>`
  - **\<input\>** is any text you want to have in title case
  - Examples:
    - `<title(StaxRip is COOL)>` => `Staxrip Is Cool`
- `<trim(<input>)>`
  - **\<input\>** is any text you want to have all leading and trailing white-space characters removed
  - Examples:
    - `<trim(  StaxRip is cool )>` => `Staxrip is cool`
- `<upper(<input>)>`
  - **\<input\>** is any text you want to have in upper case
  - Examples:
    - `<upper(StaxRip is COOL)>` => `STAXRIP IS COOL`


## Appendix

### Appendix A: DataColumn Expression Syntax
Generally the whole *ADO.NET DataColumn Expression Syntax* is supported. Here are the most important operands and functions:
- `+`, `-`, `*`, `/`, `%`
- `<`, `<=`, `<>`, `=`, `>=`, `>`, `IN`, `LIKE`
- `AND`, `OR`, `NOT`
- `CONVERT(expression, .NET Data Type)`, `LEN(expression)`, `ISNULL(expression, replacementvalue)`, `SUBSTRING(expression, start, length)`, `TRIM(expression)`
- For more information: https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-data-datacolumn-expression
