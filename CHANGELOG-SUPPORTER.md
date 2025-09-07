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
