<!--
v2.5x.0 (not published yet)
====================

- ...
- Update tools
    - ...
- Update AviSynth+ plugins
    - ...
- Update Dual plugins
    - ...
- Update VapourSynth plugins
    - ...
-->


v2.53.0 (2025-10-08)
====================

- General: Let "Check for Updates" also check for update releases
    - Changelog is not fetched anymore
- General: Handle frame server exceptions when opening scripts, that can potentially shut down StaxRip
- General: Fix cover images not being added to the container ([#1829](/../../issues/1829))
- General: Add separate process priorities for encoders and tools
- General: Add some more custom languages
- Assistant: Fix Assistant not being executed when filters are (de-)activated
- Audio: Don't add audio streams when demuxing with eac3to
- Audio: Fix bit rate calculation when data is missing
- Audio: Extend "Center/Speech optimized Stereo" with 7.1 configuration
- Log: Fix wrong Configuration written after first job
- UI: Add "Macros" to Tools-Advanced main menu
- UI: Remove UI Fallback for checkboxes
    - Only relevant for Windows 7 and Linux/WINE users
- UI: Improve Template Selection visually
- UI: Improve Encoder Editing Controls
- UI: Rearrange Project Options
- UI: Fix misalignment of some controls
- UI: Set `flac` as default output format for PCM audio tracks when demuxing Blu-rays ([#1736](/../../issues/1736))
- NVEncC: Add Editing Control to main window
- x264: Unlock direct read out / no piping again
- VapourSynth: Alter BM3DCUDA filter profile
- Update tools
    - eac3to v3.53
    - DeeZy v1.3.6
    - MediaInfo v25.09
    - mpv.net v7.1.1.4-beta
    - VCEEncC v9.01
    - x264 v0.165.3222+15-3f389cb-[Mod-by-Patman]-x64-gcc15.2.0
- Add VapourSynth plugins
    - VSHIP 3.1.0
- Update VapourSynth plugins
    - VapourSynth-BM3DCUDA R2.15


v2.51.6 (2025-09-30)
====================

- General: Fix Target File Name setting if target already exists
- General: Fix Container Configuration not opening for some old templates
    - Old templates with wrong data will be fixed and overwritten
- General: Fix crash when processing files inside temp folder, that is the root folder
- General: Fix "SetTargetFile" command and disable Target File Name Override when using it
- General: Optimize custom languages
- General: Better compatibility in case WMI service is not available ([#1817](/../../issues/1817))
- Audio: Add "pcm" to valid audio file types
- Audio: Fix audio delay issue due when setting multiple files after each other on one audio track
- Event: Fix possible crash when using old settings with old event criteria ([#1818](/../../issues/1818))
- Subtitle: Enhance subtitle format recognition ([#1820](/../../issues/1820))
- Subtitle: Identify unknown subtitle files as "srt"
- UI: Add "Target Name Override" control to encoder controls on main window
- UI: Fix opening of audio settings resets bitrate under some circumstances
- UI: Fix and change (menu) symbols
- SvtAv1EncApp-PSYEX: Update "--noise-adaptive-filtering" parameter
    - "2: Off" and "0: Default Tune Behavior" switched positions
- VCEEncC: Fix Dolby Vision RPU file field visibility and function ([#1819](/../../issues/1819))
- VCEEncC: Fix multiple codec H265 related parameters are not visible
- x264: Add levels "6", "6.1" and "6.2"
- Update tools
    - aomenc v3.13.1-57-g2a70ad7bee-x64-msvc1944
    - DeeZy v1.3.5
    - ffmpeg v8.1-dev-N-121257-x64-gcc15.2.0
    - MP4Box v2.5-DEV-rev1823-g551273021-x64-msvc1944
    - rav1e v0.8.0-(p20250624-1-gb7bf390)-x64-msvc1944
    - SvtAv1EncApp v3.1.2-126+50-5e797ec9-.Mod-by-Patman.-x64-clang21.1.1 [SVT-AV1]
    - SvtAv1EncApp v3.1.0-18+18-c3ffb9b9-.Mod.by.Patman.-x64-clang21.1.1 [SVT-AV1-HDR]
    - SvtAv1EncApp v3.0.2-B-4+19-a27a68a70-.Mod-by-Patman.-x64-clang21.1.1 [SVT-AV1-PSYEX]
    - x264 v0.165.3215+11-6113017-.Mod-by-Patman.-x64-gcc14.2.0
        - Revert to old build because of possible performance issues


v2.51.5 (2025-09-23)
====================

- General: Update Target File Name when bitrate is changed on Main window
- General: Fix Tool AutoUpdate feature ([#1808](/../../issues/1808))
- General: Fix wild behavior when "Copy/Mux" is selected along with "No Muxing" ([#1805](/../../issues/1805))
- General: Improve Override Target File Name in terms of extensions
- Log: Always write Configuration section when starting a job
- Log: Extend Configuration section with audio tracks
- Macro: Remove `%app:name%`
- Macro: Extend encoder macros with a new modifier:
    - `%parameter_Z%`: Returns a `1` in case it is active/visible, otherwise `0`
        - Example: `%--preset_Z%` -> `1`
    - Documentation: https://github.com/staxrip/staxrip/blob/master/Docs/Usage/Macros.md#encoder-macros
- Macro: Add `%isfilteractive:%` macro
    - Expects the name of the filter you want to check
    - Returns `1` in case the filter is active, otherwise `0`
    - Example: `%isfilteractive:DFTTest%` -> `1`
- UI: Optimize Search-ComboBox on Encoder Options even more ([#1797](/../../issues/1797))
- NVEncC: Improve supported codec check before encoding
- QSVEncC: Improve supported codec check before encoding
- SvtAv1EncApp: Fix "--qp-scale-compress-strength" parameter ([#1812](/../../issues/1812))
- SvtAv1EncApp: Update parameters and defaults
- SvtAv1EncApp-Essential: Fix "--qp-scale-compress-strength" parameter
- SvtAv1EncApp-PSYEX: Update parameters and defaults
- VCEEncC: Improve supported codec check before encoding
- Update tools
    - aomenc v3.13.1-50-gd459fa9018-x64-msvc1944
    - DeeZy v1.3.2
    - ffmpeg v8.1-dev-N-121146-x64-gcc15.2.0
    - MP4Box v2.5-DEV-rev1818-g5145187d2-x64-msvc1944
    - NVEncC v9.03
    - rav1e v0.8.0-(p20250624)-x64-msvc1944
    - SvtAv1EncApp v3.1.2-113+46-7786086f-.Mod-by-Patman.-x64-clang21.1.1 [SVT-AV1]
    - SvtAv1EncApp v3.1.2-Essential-2+17-548cdd45-.Mod-by-Patman.-x64-clang21.1.1 [SVT-AV1-Essential]
    - SvtAv1EncApp v3.0.2-A-5+16-15894686-.Mod-by-Patman.-x64-clang21.1.1 [SVT-AV1-PSYEX]
- Add Dual plugins
    - EEDI2CUDA v2021


v2.51.4 (2025-09-15)
====================

- General: Optionally save video encoder profiles additionally in a separate file
    - `VideoEncoderProfiles.dat` in the `Settings` folder
- General: Optionally save audio profiles additionally in a separate file
    - `AudioProfiles.dat` in the `Settings` folder
- General: Make saving of events in a separate file also optional
- Macro: Add `%target_bitdepth%`
- Macro: Fix some source file related macros when using File Batch ([#1799](/../../issues/1799))
- Macro: Reformat encoder macro modifiers:
    - `%parameterD%` => `%parameter_D%`
    - `%parameterL%` => `%parameter_L%`
    - `%parameterT%` => `%parameter_T%`
    - `%parameterU%` => `%parameter_U%`
    - `%parameterV%` => `%parameter_V%`
- UI: Optimize Search-ComboBox on Encoder Options ([#1797](/../../issues/1797))
- UI: Improve size of main window
- UI: Fix crash on Processing window when opening the menu when having processes at RealTime priority ([#1751](/../../issues/1751))
- VapourSynth: Add "BM3Dv2" filter name to VapourSynth-BM3DCUDA packages
- VapourSynth: Fix "AssumeFPS" profile ([#1807](/../../issues/1807))
- SvtAv1EncApp-Essential: Fix UI crash under some circumstances
- SvtAv1EncApp-Essential: Alter `--speed` and `--preset` parameters and the encoder control
- SvtAv1EncApp-HDR: Remove deprecated `--rmv` parameter ([#1797](/../../issues/1797))
- SvtAv1EncApp-PSYEX: Adjust parameters and their defaults ([#1796](/../../issues/1796))
- Update tools
    - MKVToolNix v95.0
    - NVEncC v9.02
    - SvtAv1EncApp v3.1.2-95+43-325066ce-.Mod-by-Patman.-x64-clang21.1.1 [SVT-AV1]
    - SvtAv1EncApp v3.1.2-Essential-2+15-c518181d-.Mod-by-Patman.-x64-clang21.1.1 [SVT-AV1-Essential]
    - SvtAv1EncApp v3.1.0-16+15-07ebe8ad-.Mod.by.Patman.-x64-clang21.1.1 [SVT-AV1-HDR]
    - SvtAv1EncApp v3.0.2-A-1+13-59a94dc9-.Mod-by-Patman.-x64-clang21.1.1 [SVT-AV1-PSYEX]
    - vvencFFapp v1.13.1 r491-c802434
    - x264 v0.165.3222+13-b815e33-.Mod-by-Patman.-x64-gcc15.2.0
    - x265 v4.1+191+33-61d0a57b3-.Mod-by-Patman.-x64-avx2-clang2111
- Update AviSynth+ plugins
    - JPSDR v4.0.0 (clang W7 AVX2)


v2.51.3 (2025-09-08)
====================

- General: Fix broken File Batch mode
- General: Optimize Long Path Prefix check and usage with quotes
- Audio: Fix audio demuxing of PCM and DTS tracks ([#1780](/../../issues/1780))
- CommandLine: Fix command line parameter parsing ([#1788](/../../issues/1788))
- Macro: Extend encoder macros with some modifiers:
    - `%parameter%`: Normal value without spaces
        - Example: `%--preset%` -> `VeryFast`
    - `%parameterD%`: `True` if the default value is set, otherwise `False`
        - Example: `%--presetD%` -> `True`
    - `%parameterL%`: Value in lowercase without spaces
        - Example: `%--presetL%` -> `veryfast`
    - `%parameterT%`: Value in Title-case (First letter only in uppercase) without spaces
        - Example: `%--presetT%` -> `Veryfast`
    - `%parameterU%`: Value in uppercase without spaces
        - Example: `%--presetU%` -> `VERYFAST`
    - `%parameterV%`: Returns the numeric value
        - Only available for checkboxes and option lists
        - Example: 
            - `%--preset%` -> `VeryFast`
            - `%--presetV%` -> `2`
        - Example: 
            - `%--open-gop%` -> `True`
            - `%--open-gopV%` -> `1`
- UI: Adjust size of TaskDialog
- UI: Multiple improvements
- UI: Improve Template Selection by marking the current and startup template differently
- UI: Improve restoring of window positions on multi-display systems ([#1724](/../../issues/1724))
- UI: Improve vertical ScrollBars next to Trees/Lists ([#1782](/../../issues/1782))
- UI: Fix disabling of Target File Name Override when target file is manually modified
- UI: Fix Override Target File Name giving the wrong file name under rare circumstances
- SvtAv1EncApp-Essential: Block Assistant, if bit-depth is not 10-bit 
- VvencFFapp: Fix encoder control causing crash on preset selection
- Update tools
    - NVEncC v9.01
    - SvtAv1EncApp v3.1.2-69+31-bb6d9b6d-.Mod.by.Patman.-x64-clang21.1.0 [SVT-AV1]
    - SvtAv1EncApp v3.1.2-Essential-2+11-03ac7823-.Mod.by.Patman.-x64-clang21.1.0 [SVT-AV1-Essential]
    - SvtAv1EncApp v3.1.0-14+7-51647873-.Mod.by.Patman.-x64-clang21.1.0 [SVT-AV1-HDR]
    - SvtAv1EncApp v3.0.2-A-1+8-63e8a0a4-.Mod.by.Patman.-x64-clang21.1.0 [SVT-AV1-PSYEX]
    - VCEEncC v9.00
    - vvencFFapp v1.13.1 r484-a169666
- Update Dual plugins
    - Neo_f3kdb r10
- Update VapourSynth plugins
    - VapourSynth-BM3DCUDA R2.16
    - VS-DFTTest2 v9


v2.51.2 (2025-09-02)
====================

- General: Save events additionally in a separate file in the Settings folder
- General: Add support to auto crop only horizontal/vertical bars ([#1746](/../../issues/1746))
- CommandLine: Improve command line parameter parsing even more
- CommandLine: Add "-AddSubtitle" command line parameter ([#1755](/../../issues/1755))
- Event: Add parameter macros to the list of available macros ([#1733](/../../issues/1733))
- Event: Add criteria input text field for parameter macros ([#1733](/../../issues/1733))
    - Gives you the opportunity to add a criteria like  
        `MediaInfo Video Property` | `FrameRate` | `Is` | `25.000`
- Macro: Make some macros proper parameter macros
    - Rename `%audio_bitrateX%` to `%audio_bitrate:X%`
    - Rename `%audio_channelX%` to `%audio_channel:X%`
    - Rename `%audio_codecX%` to `%audio_codec:X%`
    - Rename `%audio_delayX%` to `%audio_delay:X%`
    - Rename `%audio_fileX%` to `%audio_file:X%`
- UI: Disable Target Override when target file is manually modified
- UI: Hide data when no item is chosen on Apps Manager
- UI: Colorize Version on Apps Manager if wrong version is detected
- UI: Set `pcm` as default output format for PCM audio tracks when demuxing Blu-rays ([#1736](/../../issues/1736))
- Update tools
    - FFmpeg v7.2-dev-N-119327-x64-gcc14.2.0
    - NVEncC v9.00
    - QSVEncC v8.00
    - SvtAv1EncApp v3.1.2-57+24-7b58d2b7-.Mod.by.Patman.-x64-clang21.1.0 [SVT-AV1]
    - SvtAv1EncApp v3.1.2-Essential-.Mod.by.Patman.-x64-clang21.1.0 [SVT-AV1-Essential]
    - SvtAv1EncApp v3.1.0-206+9-d3c264f4-.Mod.by.Patman.-x64-clang21.1.0 [SVT-AV1-HDR]
    - SvtAv1EncApp v3.0.2-A-1+41-b621481b8-.Mod.by.Patman.-x64-clang20.1.8 [SVT-AV1-PSYEX]
- Update AviSynth+ plugins
    - JPSDR v3.9.0 (W7 AVX2)


v2.51.1 (2025-08-25)
====================

- General: Improve Windows 7 compatibility ([#1749](/../../issues/1749))
- General: Add a few custom languages
- CommandLine: Fix "-AddAttachments" command line parameter
- CommandLine: Add "-AddChaptersFile" command line parameter ([#1754](/../../issues/1754))
- CommandLine: Add "-AddSubtitles" command line parameter ([#1755](/../../issues/1755))
- CommandLine: Add "-DisableEvents" command line parameter ([#1750](/../../issues/1750))
- CommandLine: Add "-EnableEvents" command line parameter ([#1750](/../../issues/1750))
- CommandLine: Add "-DisableFilterNames" command line parameter ([#1753](/../../issues/1753))
- CommandLine: Add "-EnableFilterNames" command line parameter ([#1753](/../../issues/1753))
- CommandLine: Add "-SetTitle" command line parameter ([#1756](/../../issues/1756))
- CommandLine: Add "-SetVideoTrackLanguage" command line parameter ([#1758](/../../issues/1758))
- CommandLine: Add "-SetVideoTrackName" command line parameter ([#1757](/../../issues/1757))
- CommandLine: Improve command line parameter parsing ([#1752](/../../issues/1752))
- Demux: Add "-analyzeduration" to ffmpeg demuxer command line ([#1769](/../../issues/1769))
- Thumbnailer: Fix wrong position of timestamps
- UI: Fix crash on Thumbnailer font selection
- UI: Add custom fonts for Thumbnailer
- UI: Some small improvements and fixes
- SvtAv1EncApp-Essential: Add "--quality" parameter
- SvtAv1EncApp-Essential: Add "--speed" parameter
- Update tools
    - AviSynth+ v3.7.5
    - DoVi_Tool v2.3.1
    - NVEncC v8.11
    - Python v3.13.6
    - VapourSynth R72
- Add VapourSynth plugins
    - VapourSynth-BM3DCUDA R2.15
- Update VapourSynth plugins
    - VS-DFTTest2 v8
    - VS-Placebo v3.3.1


v2.51.0 (2025-08-08)
====================

- General: Supporter-Releases get their own Changelog
- General: Add possibility to override Target File Name via Encoder
    - If enabled it will override the default naming and the *Default Target Name* setting from Project Options
    - General macros are supported
    - Extended encoder related macros are available in form of `%parameter%`
    - Preview with expanded macros is available - `%source_name%` is ignored for readability reasons
    - Examples:
        - x265: `%source_name%_CRF%--crf%_AQ-Mode%--aq-mode%`
            - Result: `%source_name%_CRF23_AQ-Mode2`
        - SvtAv1EncApp: `%source_name%_CRF%--crf%_Predict%--pred-struct%_VBoost%--enable-variance-boost%`
            - Result: `%source_name%_CRF35_Predict2_VBoost1`
    - Documentation: https://github.com/staxrip/staxrip/blob/master/Docs/Usage/Macros.md#encoder-macros
- General: Strengthen file savings
    - Should avoid error messages when saving files, that are in use by another process
- General: Improve language extraction from file names
- Audio: Add bit-depth settings for FLAC codec ([#880](/../../issues/880), [#1735](/../../issues/1735))
- Audio: Alter bit-depth settings for W64/WAV codecs ([#880](/../../issues/880), [#1693](/../../issues/1693), [#1735](/../../issues/1735))
    - Add "Original" to take over the bit depth from the source file
    - Add "32-bit (single)" and "32-bit (float)"
- Audio: In case of the WAVE codec as intermediate file, the bit depth can be chosen via the Project Options ([#1693](/../../issues/1693))
- CommandLine: Add "-AddAttachments" command line parameter
- CommandLine: Add "-AddTagFile" command line parameter
- UI: Improve keyboard navigation on Task Dialog
- UI: Fix minor displaying issues on Apps Manager
- UI: Make Encoder Settings window bigger
- UI: Re-arrange Resize panel on main window
- UI: Adjust TreeView item height
- UI: Improve Treeview entries visually
- UI: Multiple improvements
- UI: Overhaul the "What's new" presentation
- AviSynth: Comment out dependencies
    - Like it was introduced for VapourSynth a while ago
- FFmpeg: Add piping as option
- SvtAv1EncApp: Fix "--variance-octile" default for the PSY fork ([#1739](/../../issues/1739))
- SvtAv1EncApp: Separate the Mainline and the 3 supported forks, so they can be used side by side
    - SVT-AV1-Essential
    - SVT-AV1-HDR
    - SVT-AV1-PSYEX
- x264: Add Chunks possibility
- Update tools
    - MediaInfo v25.07
    - MKVToolNix v94.0
    - NVEncC v8.10
    - Subtitle Edit v4.0.13
    - SvtAv1EncApp v3.1.0-6+19-bff0edd3-.Mod.by.Patman.-x64-clang20.1.8 [SVT-AV1]
    - SvtAv1EncApp v3.1.0-Essential-.Mod.by.Patman.-x64-clang20.1.8 [SVT-AV1-Essential]
    - SvtAv1EncApp v3.0.2-A-1+41-b621481b8-.Mod.by.Patman.-x64-clang20.1.8 [SVT-AV1-PSYEX]
    - SvtAv1EncApp v3.1.0-196+30-878b2673-.Mod.by.Patman.-x64-clang20.1.8 [SVT-AV1-HDR]
    - x265 v4.1+190+31-9e7f7ad13-.Mod-by-Patman.-x64-avx2-clang2018
