<!--
v2.4x.0 (not published yet)
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


v2.46.4 (2025-04-18)
====================

- General: Let Log window save also obfuscated log file
- General: Improve Dolby Vision Crop detection for Automatic Mode
- General: Add "No Linguistic Content" Language ([#1599](/../../issues/1599))
- General: Add "Chinese (Cantonese)" and "Chinese (Mandarin)" Languages ([#1312](/../../issues/1312))
- Audio: Fix not working cutting of FLAC files ([#1575](/../../issues/1575))
- Audio: Fix not working FLAC-to-FLAC encodes ([#1616](/../../issues/1616))
- Audio: Fix not working encodes in case source and target use same codec ([#1616](/../../issues/1616))
- Audio: Fix "Center/Speech Optimized Stereo" option ([#1617](/../../issues/1617))
- UI: Improve Template Selection
    - Add optional timeout
- UI: Fix missing script data after opening existing project
- UI: Add gaps between headers on Apps Manager
- VCEEnc: Add "Slower" Preset for H264/HEVC
- Update tools
    - DoVi_Tool v2.2.0
    - HDR10Plus_Tool v1.7.0
    - NVEncC v8.03
    - QSVEncC v7.86
    - Subtitle Edit v4.0.12
    - VCEEncC v8.35
    - vvencFFapp v1.13.1 r334-ba2f57d
    - x265 v4.1+140+19-eb0b7b0cd-.Mod-by-Patman.-x64-avx2-clang2012


v2.46.3 (2025-04-06)
====================

- General: Fix "Save as Template" overwriting the old template instead of creating a new file ([#1640](/../../issues/1640))
- UI: Fix showing empty icons on buttons


v2.46.2 (2025-04-05)
====================

- General: Skip script synchronising on project opening
- General: Fix template mix up when opening multiple files in a row, mostly visual
- General: Fix Template Selection show up when using "-LoadTemplate" parameter
- Command: Fix "AddBatchJobs" not taking quoted paths ([#1627](/../../issues/1627), [#1636](/../../issues/1636))
- UI: Fix "Crop with High Contrast"
- UI: Improve speed of Language menu buttons on Audio and Muxer windows a little bit
- UI: Improve displaying of Status on Apps Manager
- UI: Make Location, URLs and Version on Apps Manager clickable
- UI: Show Help URL on Apps Manager
- FrameServer: Fix Indexing for LWLibav ([#1619](/../../issues/1619))
- SvtAv1EncApp: Fix Long Path issue


v2.46.1 (2025-03-28)
====================

- General: Add Long Path Registry Check on Startup
- General: Extend "Convert bit depth to 10-bit" option to make it possible to choose the bit depth
- Command: Fix commands that receive an array of inputs ([#1627](/../../issues/1627))
- General: Fix wrong template saving when opening Blurays
- General: Fix opening multiple source files using the same template
- Command: Make parameters comparison passed to "-ImportVideoEncoderCommandLine" case insensitive
- Macro: Fix macros:
    - %jobs%
    - %jobs_active%
    - %player%
- Macro: Rename %source_video_format% to %source_mi_v:Format%
- Macro: Remove macros:
	- %media_info_audio:property%
	- %media_info_video:property%
- Macro: Add new macros to gather MediaInfo data from the source file:
	- %source_mi_g:property% for data from the General section
	- %source_mi_v[#]:property% for data from the Video section
		- [#] is an optional track number and can be set to get information of a certain video track number - default is 0
	- %source_mi_a[#]:property% for data from the Audio section
		- [#] is an optional track number and can be set to get information of a certain audio track number - default is 0
	- %source_mi_t[#]:property% for data from the Text section
		- [#] is an optional track number and can be set to get information of a certain text track number - default is 0
	- %source_mi_vc% to get the number of video tracks within the source file
	- %source_mi_ac% to get the number of video tracks within the source file
	- %source_mi_tc% to get the number of video tracks within the source file
- UI: Fix not closing Thumbnailer after last job when trying to close StaxRip or shutdown the system
- UI: Key down on Apps List on Apps Manager starts searching
- UI: Adjust window sizes and save them in some cases
- AviSynth: Fix Indexing for LWLibav ([#1619](/../../issues/1619))
- NVEncC: Update multiple parameters ([#1631](/../../issues/1631))
- QSVEncC: Fix advanced "--cqp" parameter ([#1624](/../../issues/1624))
- SvtAv1EncApp: Fix altered default value for "--keyint" for PSY (only visual)
- VCEEncC: Fix advanced "--cqp" parameter ([#1624](/../../issues/1624))
- x265: Fix import of quality mode based parameters
- Update tools
    - MediaInfo v25.03
    - MKVToolNix v91.0
    - NVEncC v8.00 beta7
    - QSVEncC v7.84
    - vvencFFapp v1.13.1 r323-ca1af4a
    - x265 v4.1+126+17-ba353d539-.Mod-by-Patman.-x64-avx2-clang1911
- Update Dual plugins
    - BestSource R11


v2.46.0 (2025-03-22)
====================

- General: Change way to fetch Windows Version data ([#1515](/../../issues/1515))
- General: Fix crash between job processing under rare circumstances
- General: Implement Import Command Line functions for some special parameters
- General: Add option to take over language from video source file
- General: Add option to take over title from source file
    - Before it was always taken over when title in template was empty
- General: Add "Code at bottom" option box additional to existing "Code at top" option ([#1468](/../../issues/1468))
- Audio: Preserve delay from template when opening source files ([#1614](/../../issues/1614))
- Audio: Preserve delay when changing profiles ([#1614](/../../issues/1614))
- Command: Add new "LoadTemplateWithSelectionDialog" command
- CommandLine: Add new "-LoadSourceFilesWithTemplateSelection" parameter
- CommandLine: Fix mishandling parameters in quotes
- FrameServer: Fix FFVideoSource and FFMS2 indexing for non regular temp folders ([#1610](/../../issues/1610))
- UI: Improve Command Line Highlighting ([#1598](/../../issues/1598))
- UI: Lower default number of audio tracks on main window to 4
- UI: Height adjustment for filters and encoder controls
- UI: Make some windows bigger
- UI: Increase max width of Code Editor
- UI: Add support for template subfolders ([#1449](/../../issues/1449))
    - Saving into a subfolder is currently not implemented, so you have to move/re-place them by hand
- UI: Reinvent "Show Template Selection when loading files"
    - A template selection can be shown when opening a source
    - Template selection can be restricted to 
        - Command Line parameters and/or...
        - Drag&Drop operations on main window and/or...
        - Opening Menu on main window
    - Template Subfolders are supported
    - The current (or last) template can also be selected from the list
- UI: Add "Warn on identical audio" to switchable Assistant options
- UI: Add ffmpeg Progress Highlighting
- UI: Improve error message if invalid commands shall be executed ([#1595](/../../issues/1595))
- QSVEncC: Add "--vpp-libplacebo-deband" parameter
- QSVEncC: Add "--vpp-libplacebo-shader" parameter
- QSVEncC: Add "--vpp-libplacebo-tonemapping" parameter
- SvtAv1EncApp: Add "--variance-boost-curve" parameter
- VCEEncC: Add "--screen-content-tools" parameter ([#1568](/../../issues/1568))
- VCEEncC: Add "--vpp-libplacebo-deband" parameter
- VCEEncC: Add "--vpp-libplacebo-shader" parameter
- VCEEncC: Add "--vpp-libplacebo-tonemapping" parameter
- VCEEncC: Add new value "libplacebo" for "--vpp-resize" parameter
- Update tools
    - NVEncC v8.00 beta6
    - Python v3.12.9
    - QSVEncC v7.83
    - VCEEncC v8.32


v2.44.5 (2025-03-07)
====================

- General: Fix abortion issue with suspended AviSynth piped processes ([#1589](/../../issues/1589))
- General: Recognize localized language names when identifying the language from file names
- General: Ignore existing video files in temp directory created by Chunks when opening source file again
- General: Don't delete temp files even when there is a non-active job in the queue
- General: Add ".vtt" and ".webvtt" as subtitles
- Macro: Add macro %empty%
- UI: Assign non-NumPad-Keys to cropping on Crop window at first start when no NumPad is available
- UI: Disable Download- and Explorer-buttons if app/plugin does not have related data on Apps Manager
- UI: Print more accurate status messages on Apps Manager in case an app/plugin is not found
- UI: Fix crash on VideoComparison when closing the last tab
- UI: Fix ffmpeg progress not shown
- UI: Fix rounding issue with number edits when using up/down arrow keys


v2.44.4 (2025-02-16)
====================

- Audio: Fix broken adding of preferred audio tracks when specifying track numbers
- Event: Add new event "Before Muxing When Source Opening"
    - Gives the possibility for example to add/delete/modify files in the temp folder before they are read in
- UI: Improve HDR type data display for source files
- UI: Show Website and Download URL on Apps Manager
- AviSynth: Make indexing for FFMS2 a process
- AviSynth: Alter BestSource Source Profile
- VapourSynth: Alter BestSource Source Profile
- FFmpeg: Add GoPro CineForm HD (cfhd) codec
- NVEncC: Add "--level" parameter
- NVEncC: Add individual advanced settings for "--qp-init", "--qp-max" and "--qp-min"
- QSVEncC: Add individual advanced settings for "--qp-max" and "--qp-min"
- SvtAv1EncApp: Add "--spy-rd" parameter
- VCEEncC: Add individual advanced settings for "--qp-max" and "--qp-min"
- Update tools
    - MKVToolNix v90.0
    - NVEncC v7.82
    - QSVEncC v7.79
    - Subtitle Edit v4.0.11
    - SvtAv1EncApp v2.3.0-B-0+17-b834bf13-.Mod-by-Patman.-x64-msvc1942 [SVT-AV1-PSY]
    - VCEEncC v8.28
    - x264 v0.164.3204+8-8241910-.Mod-by-Patman.-x64-gcc14.2.0
- Update VapourSynth plugins
    - D2VSource v1.3.0


v2.44.3 (2025-01-30)
====================

- General: Add demuxing of tags from MKV files
- Subtitle: Add subtitle files independently from the dialog selected languages ([#1562](/../../issues/1562))
- UI: Improve playlist selection for Blu-Ray folders
- NVEncC: Add "--vpp-libplacebo-shader" parameter
- NVEncC: Add "--log" parameter
- NVEncC: Add advanced settings for "--qp-init", "--qp-max" and "--qp-min" ([#1493](/../../issues/1493))
- QSVEncC: Add advanced settings for "--qp-max" and "--qp-min" ([#1493](/../../issues/1493))
- VCEEncC: Add advanced settings for "--qp-max" and "--qp-min" ([#1493](/../../issues/1493))
- VCEEncC: Add "--adapt-minigop" parameter ([#1568](/../../issues/1568))
- VCEEncC: Fix "--ref" parameter ([#1568](/../../issues/1568))
- VCEEncC: Unlock "--temporal-layers" for H265 codec ([#1568](/../../issues/1568))


v2.44.2 (2025-01-11)
====================

- VapourSynth: Fix wrong Resize filter addition when choosing fixed values from menu
- NVEncC: Fix SuperRes default values ([#1572](/../../issues/1572))
- x265: Fix "--merange" default preset value
- Update tools
    - MP4Box v2.5-DEV-rev1021-g49a598895-x64-msvc1942
    - NVEncC v7.81
    - QSVEncC v7.78
    - VCEEncC v8.27
    - x265 v4.1+79+12-81640d428-.Mod-by-Patman.-x64-avx2-msvc1942


v2.44.1 (2025-01-01)
====================

- General: Fix Haali Splitter package
- General: Fix Package status check
- General: Disable Dolby Vision metadata extraction for non-HEVC files ([#1553](/../../issues/1553))
- General: Add "Convert bit depth to 10-bit" option
    - Adds a filter to change the bit depth to 10-bit if the source file has a different bit depth
- Audio: Fix Normalize with Channel change for Opus ([#1569](/../../issues/1569))
- Audio: Fix hidden errors while cutting audio with mkvmerge ([#1525](/../../issues/1525))
- Audio: Override Track Name if specified in template ([#1487](/../../issues/1487))
- UI: Change source and target length format to HH:mm:ss
- UI: Fix crash and/or missing text when using Thumbnailer
- UI: Fix missing Assistant warning for no audio
- UI: Make Audio editing window taller
- MP4Box: Fix default tag for audio tracks and subtitles ([#1530](/../../issues/1530))
- NVEncC: Add "--vpp-libplacebo-deband" parameter
- NVEncC: Add "--vpp-libplacebo-tonemapping" parameter
- QSVEncC: Add new value "libplacebo" for "--vpp-resize" parameter
- SvtAv1EncApp: Add "--ss" parameter ([#1556](/../../issues/1556))
- SvtAv1EncApp: Fix "--lp" parameter ([#1556](/../../issues/1556))
- SvtAv1EncApp: Fix "--pin" parameter ([#1556](/../../issues/1556))
- SvtAv1EncApp: Add value "2" to "--enable-tf" parameter
- SvtAv1EncApp: Add "--psy-rd" parameter
- SvtAv1EncApp: Apply new default value of "1" to "--sharpness" parameter
- Update tools
    - ffmpeg v7.1-dev-N-116758-x64-gcc14.2.0
    - MKVToolNix v89.0
    - Python v3.12.8
    - Subtitle Edit v4.0.10
    - SvtAv1EncApp v2.3.0-A-0+15-5ea471e2-.Mod-by-Patman.-x64-msvc1942 [SVT-AV1-PSY]
    - vvencFFapp v1.13.0 r298-81a3b6c


v2.44.0 (2024-12-20)
====================

- Audio: Add "Center/Speech Optimized Stereo" option
- Audio: Demux audio even if all tracks are set to "No Audio" ([#1529](/../../issues/1529))
- Audio: Fix broken audio file addition of demuxed files under some circumstances
- Audio: Fix Normalize ([#1484](/../../issues/1484))
- Audio: Improve Normalize
- Macro: Fix %encoder_codec% is not shown on Macro List
- MP4Box: Add custom switches for video tracks ([#1532](/../../issues/1532))
- MP4Box: Add "Better Seeking" general preset ([#1535](/../../issues/1535))
- NVEncC: Add new value "libplacebo" for "--vpp-resize" parameter
- NVEncC: Adjust default values for "--vpp-ngx-truehdr" parameter
- NVEncC: Always show "--vbr-quality" parameter in VBR mode
- NVEncC: Remove parameters "--level", "--profile" and "--tier" for AV1 codec ([#1534](/../../issues/1534))
- Rav1e: Fix 2-pass execution ([#1531](/../../issues/1531))
- Rav1e: Fix bitrate not being synced with main window
- Rav1e: Reorganize options
- SvtAv1EncApp: Set default value for "--chroma-qm-min" to 8
- SvtAv1EncApp: Adjust default value for "--hierarchical-levels" ([#1546](/../../issues/1546))
- SvtAv1EncApp: Add value "2" to "--fast-decode" parameter
- SvtAv1EncApp: Add "--noise-norm-strength" parameter
- SvtAv1EncApp: Add "--kf-tf-strength" parameter
- SvtAv1EncApp: Rename "Logical Processors" to "Level Of Parallelism"
- x265: Fix overwriting of set up VBV values when loading a source file ([#1539](/../../issues/1539))
- x265: Adjust new default values for Medium Preset
- x265: Fix default value for "--limit-tu" for VerySlow Preset
- x265: Remove deprecated parameters ([#1544](/../../issues/1544))
- Update tools
    - aomenc v3.11.0-145-gc1bcb109d4-x64-msvc1942
    - DoVi_Tool v2.1.3
    - FFmpeg v7.2-dev-N-118074-x64-gcc14.2.0
    - HDR10Plus_Tool v1.6.1
    - MediaInfo v24.12
    - MKVToolNix v88.0
    - MP4Box v2.5-DEV-rev964-ga3c626300-x64-msvc1942
    - NVEncC v7.77
    - Python v3.12.7
    - qaac v2.83
    - QSVEncC v7.75
    - rav1e v0.7.0 (p20241015)-x64-msvc1942
    - Subtitle Edit v4.0.8
    - SvtAv1EncApp v2.3.0-1+12-7ae833c2-.Mod-by-Patman.-x64-msvc1942 [SVT-AV1-PSY]
    - VapourSynth R70
    - VCEEncC v8.24
    - vvencFFapp v1.13.0 r295-0c2c21e
    - x264 v0.164.3198+7-bf259fa-.Mod-by-Patman.-x64-gcc14.2.0
    - x265 v4.1+54+9-0c07fe364-.Mod-by-Patman.-x64-avx2-msvc1942
- Update AviSynth+ plugins
    - DePan v2.7.46
    - DePanEstimate v2.7.46
    - MVTools2 v2.7.46
- Update Dual plugins
    - BestSource R8
- Update VapourSynth plugins
    - AVISource R1


v2.42.2 (2024-11-08)
====================

- General: Add file extension to temp folder name
- General: Add file extension to log file when saved next to source file
- Macro: Add new macro %encoder_codec% ([#1520](/../../issues/1520))
- UI: Fix removing of last path separator from paths ([#1491](/../../issues/1491))
- UI: Fix not showing error messages on indexing ([#1514](/../../issues/1514))
- UI: Show crop values on main window ([#1521](/../../issues/1521))
- UI: Fix Demux Dialog ignores selection of audio and subtitle streams ([#1502](/../../issues/1502))
- FFmpeg: Fix ProRes Profile option ([#1523](/../../issues/1523))
- NVEncC: Fix parameters "--qp-init", "--qp-min" and "--qp-max"
- QSVEncC: Fix parameters "--qp-min" and "--qp-max"
- SvtAv1EncApp: Fix passing wrong width and height values to the encoder when filters changed the size
- VCEEncC: Add, fix and alter various parameters ([#1486](/../../issues/1486))


v2.42.1 (2024-09-08)
====================

- UI: Fix Demuxing Subtitles for ffmpeg ([#1466](/../../issues/1466))
- UI: Fix not saving log files ([#1467](/../../issues/1467))
- UI: Fix no time display on Crop window ([#1470](/../../issues/1470))
- UI: Disable Window Dark Mode when system colors are used ([#1470](/../../issues/1470))
- UI: Fix Demux handling for subtitles ([#1471](/../../issues/1471))
- NVEncC: Fix incompatible output file type for AV1 encodes
- QSVEncC: Fix incompatible output file type for AV1 encodes
- VCEEncC: Fix incompatible output file type for AV1 encodes


v2.42.0 (2024-09-05)
====================

- UI: Add support for Dark Mode Window Mode ([#1448](/../../issues/1448))
- UI: Enlarge Settings window
- UI: Enlarge Options window
- UI: Extend main window and make it resizeable
- UI: Make number of audio tracks on main window optional
- UI: Re-implement audio track detection and reordering
- UI: Add menu items to Crop window to jump to a specific time and frame ([#1421](/../../issues/1421))
- UI: Fix THD+AC3 Output selection on eac3to window
- UI: Add "Execute All" to Audio Tracks ([#1123](/../../issues/1123))
- UI: Improve Dolby Vision Profile 5 metadata import when no VUI metadata available
- UI: Fix Demux for user selected subtitles
- UI: Ignore existing video files in temp directory created by Compressibility Check when opening source file again
- UI: Add new event "After Source Opened"
    - Which is triggered at the end before validation of the scripts. After the validation the existing "After Source Loaded" triggers
- UI: Fix Indexing for LWLibav
- UI: Make Indexing cancelable
- UI: Add new macros ([#1431](/../../issues/1431))
    - %hdr10plus_path%, %hdrdv_path%
- UI: Enable the "Subtitle Edit" button on muxer configuration for all subtitles
- UI: Fix Audio conversion ignoring Normalize and Gain in some cases
- UI: Fix Assistant blocking message on cutting without a Dolby Vision metadata file path set ([#1454](/../../issues/1454))
- UI: Add "ImportVideoEncoderCommandLineFromTextFile" command
- UI: Improve Command Line Import
- UI: Improve handling/demuxing with attachments
- UI: Add support for covers with jpeg file extension ([#1233](/../../issues/1233))
- UI: Fix crash on Crop window opening under some circumstances ([#1462](/../../issues/1462))
- Crop: Make AutoCrop process skippable ([#1411](/../../issues/1411))
- Crop: Add Frame Interval Selection Mode
- Crop: Add Frame Consideration Mode to Options ([#1386](/../../issues/1386))
    - Makes it able to handle intros and outros that are not cropped
- Crop: Add Luminance Threshold
    - Lets the user set the max brightness of a line, that is considered to be cropped
- Crop: Improve Automatic Mode for Dolby Vision sources
- FFmpeg: Add "-movflags +faststart" to muxer command where possible ([#1408](/../../issues/1408))
- MP4Box: Add "Streaming" command line menu preset ([#1408](/../../issues/1408))
- NVEncC: Add options "RGB" and "YUVA420" to parameter "--output-csp"
- NVEncC: Fix incompatible output file type for AV1 encodes
- NVEncC: Make Dolby Vision related parameters available for AV1
- Qaac: Add "--no-delay" parameter ([#1410](/../../issues/1410))
- QSVEncC: Fix incompatible output file type for AV1 encodes
- QSVEncC: Add "--output-csp" parameter
- SvtAv1EncApp: Fix parameters for non Psy versions
    - "--qp", "--qp-max"
- SvtAv1EncApp: Add parameters
    - "--tf-strength", "--chroma-qm-min", "--chroma-qm-max", "--tune 4"
- VCEEncC: Add parameters ([#1444](/../../issues/1444))
    - "--vpp-decomb", "--vpp-fft3d", "--vpp-nlmeans", "--atc-sei", "--chromaloc", "--colorrange", "--master-display", "--max-cll"
- VCEEncC: Fix parameters ([#1444](/../../issues/1444))
    - "--pa", "--preset"
- VCEEncC: Fix mode options across all codecs ([#1444](/../../issues/1444))
- VCEEncC: Fix incompatible output file type for AV1 encodes
- x265: Add parameters
    - "--alpha", "--format", "--scc"
- x265: Fix "--aq-mode" default value across different presets
- Update tools
    - aomenc v3.9.1-419-g9ed60cc5ff-x64-msvc1940
    - AutoCrop v2.5
    - DoVi_Tool v2.1.2
    - eac3to v3.52
    - ffmpeg N-116758-x64-gcc14.2.0
    - MediaInfo v24.04
    - MKVToolNix v86.0
    - Mp4Box v2.5-DEV-rev479-gd7e1a5c15-x64-msvc1940
    - NVEncC v7.66
    - QSVEncC v7.69
    - rav1e v0.7.0 (p20240612-5-g7ab0de1)-x64-msvc1940
    - Subtitle Edit v4.0.7
    - SvtAv1EncApp v2.2.1-0+1-e6255481-.Mod-by-Patman.-x64-msvc1940 [SVT-AV1-PSY]
    - VCEEncC v8.23
    - vvencFFapp v1.12.0 r245-efb49f4
    - x265 v3.6+85+1-b5cacb584-.Mod-by-Patman.-x64-avx2-msvc1940
- Update AviSynth+ plugins
    - AvsResize R25
- Update Dual plugins
    - BestSource R6
- Update VapourSynth plugins
    - Curve r3


v2.41.7 (2024-07-09)
====================

- UI: Fix applying of wrong/default theme when selecting existing folder on first start ([#1418](/../../issues/1418))
- UI: Fix script files being also deleted when Video file types are selected
- UI: Fix crash on typing in invalid characters to the target file name
- NVEncC: Add new "--vpp-ngx-truehdr" parameter
- NVEncC: Fix crash when activating "Resize"


v2.41.6 (2024-07-03)
====================

- UI: Fix "--normalize" and "--gain" are used for qaac at the same time when Pipe is used ([#1410](/../../issues/1410))
- Internal: Fix crash on demuxing command without loaded project
- Internal: Fix rare crash when closing Processing window right before closing StaxRip ([#1414](/../../issues/1414))
- Internal: Raise event "Before Job Adding" also when adding a batch job ([#1407](/../../issues/1407))
- Internal: Improve valid Dolby Vision metadata recognition for extraction ([#1409](/../../issues/1409))
- NVEncC: Add new "--vpp-fft3d" parameter
- NVEncC: Add new value "ngx-vsr" for "--vpp-resize" parameter
- x264: Add "--aq-bias-strength" parameter ([#1412](/../../issues/1412))
- Update tools
    - MediaInfo v24.06
    - NVEncC v7.57
    - QSVEncC v7.67
    - vvencFFapp v1.12.0 r236-d57c73d


v2.41.5 (2024-06-23)
====================

- UI: Fix tolerances to Frame Mismatch for temp files deletion ([#1400](/../../issues/1400))
- Update tools
    - MKVToolNix v85.0
    - OpusEnc v0.2-34-g98f3ddc (using libopus 1.5.2-11-g2554a89)
    - SvtAv1EncApp v2.1.0-A-1+6-adc3da7b-.Mod-by-Patman.-x64-msvc1940 [SVT-AV1-PSY]


v2.41.4 (not published)
====================

- UI: Add tolerances to Frame Mismatch when it comes to delete temp files ([#1400](/../../issues/1400))
    - There are rare cases where an encoder or muxer might add a few frames to the final file
- SvtAv1EncApp: Fix default value for "--enable-variance-boost" ([#1405](/../../issues/1405))


v2.41.3 (not published)
====================

- UI: Remove VP9 as supported extension for mkvmerge (mkv and webm)
- QSVEncC: Fix VP9 encoding


v2.41.2 (2024-06-21)
====================

- UI: Add VP9 as supported extension for mkvmerge (mkv and webm)
- Internal: Fix package dependencies ([#1401](/../../issues/1401))


v2.41.1 (not published)
====================

- SvtAv1EncApp: Fix Variance Boost parameters ([#1398](/../../issues/1398))
- SvtAv1EncApp: Fix "--adaptive-film-grain" parameters default value ([#1398](/../../issues/1398))


v2.41.0 (2024-06-16)
====================

- Crop: Improve Automatic Dolby Vision Auto Crop mode
- QSVEncC: Add new "--vpp-fft3d" parameter
- SvtAv1EncApp: Add new "--adaptive-film-grain" parameter [PSY]
- SvtAv1EncApp: Add new "--hdr10plus-json" parameter [PSY]
- SvtAv1EncApp: Add new "--max-32-tx-size" parameter [PSY]
- SvtAv1EncApp: Add HDR10+ JSON file importation to VUI metadata import
- SvtAv1EncApp: Fix VUI metadata import
- VapourSynth: Fix wrong loading order of dependencies under some circumstances
- Update tools
    - AOMEnc v3.9.1-227-g49c02efb61-x64-msvc1940
    - DGMPGDec v3.0.0.0
    - ffmpeg N-115847-g7c95c7de4a-x64-gcc14.1.0
    - Mp4Box v2.5-DEV-rev273-gf2ce8bf1d-x64-msvc1940
    - QSVEncC v7.66
    - rav1e v0.7.0 (p20240612)-x64-gcc14.1.0
    - SvtAv1EncApp v2.1.0-A-1+4-52522a1a-.Mod-by-Patman.-x64-msvc1940 [SVT-AV1-PSY]


v2.40.1 (not published)
====================

- Internal: Fix path handling when passed to StaxRip as parameter value via command line ([#1390](/../../issues/1390))
- Internal: Fix package dependencies


v2.40.0 (2024-06-08)
====================

- UI: Fix path handling when passed to StaxRip via command line ([#1375](/../../issues/1375), [#1377](/../../issues/1377), [#1378](/../../issues/1378))
- UI: Fix blocking script error messages during job proceeding ([#1231](/../../issues/1231))
- UI: Add question when adding an existing and active job with similar project file path
- UI: Add new event "Before Job Adding" ([#1346](/../../issues/1346))
- UI: Add new event "After Job Added"
- UI: Add setting for frame number position on file names when saving images ([#1373](/../../issues/1373))
- UI: Improve Progress Highlighting
- Crop: Activate AutoCrop by default
    - Due to a poll on Discord where most participants crop all their videos
- Crop: Move AutoCrop settings from Settings to Project Options
- Crop: Add new "Time Interval" mode for AutoCrop
    - Analyzes a frame every x seconds, which makes it more useful throughout short and long videos
- VideoComparison: Run indexing of source files as (visible) task/job ([#1226](/../../issues/1226))
- Internal: Skip importing of scripts that are used by commented out functions (thanks to @Valdiralita ([#1318](/../../pull/1318)))
- NVEncC: Add new "--vpp-decomb" parameter
- QSVEncC: Add new "--vpp-decomb" parameter
- SvtAv1EncApp: Add long path support ([#1374](/../../issues/1374))
- Update tools
    - AutoCrop v2.4
    - MediaInfo v24.05
    - MKVToolNix v84.0
    - NVEncC v7.54
    - QSVEncC v7.65
    - Subtitle Edit v4.0.6
    - SvtAv1EncApp v2.1.0-1+1-8ba1c70f-.Mod-by-Patman.-x64-msvc1939 [SVT-AV1-PSY]
    - vvencFFapp v1.11.1 r227-77574c
    - x264 v0.164.3191+1-e3de8e8-.Mod-by-Patman.-x64-gcc14.1.0
    - x265 v3.6+13+1-726aec126-.Mod-by-Patman.-x64-avx2-msvc1939
- Update Dual plugins
    - BestSource R5
    - ffms2 v5.0


v2.39.3 (2024-05-30)
====================

- UI: Fix multiple files opening when a comma was included ([#1372](/../../issues/1372))
- FFmpeg: Concat "-af" parameters on audio conversion ([#1370](/../../issues/1370))
    - Makes it possible to add custom params when using "Normalize"
- SvtAv1EncApp: Fix VUI metadata import ([#1371](/../../issues/1371))


v2.39.2 (2024-05-27)
====================

- UI: Fix DVB subtitle extraction from TS containers throwing errors ([#1361](/../../issues/1361))
- UI: Fix long path issues on Batch file jobs ([#1051](/../../issues/1051))
- UI: Fix not able to open Batch file jobs ([#1051](/../../issues/1051))
- SvtAv1EncApp: Add VUI metadata import ([#1362](/../../issues/1362))
- VapourSynth: Fix presets ([#1359](/../../issues/1359))


v2.39.1 (2024-05-13)
====================

- SvtAv1EncApp: Fix not showing "--frame-luma-bias" parameter ([#1356](/../../issues/1356))
- Update tools
    - AutoCrop v2.3


v2.39.0 (2024-05-12)
====================

- UI: Introduce Progress Highlighting
- UI: In Main Menu move project based Folders from Tools to Project
- UI: Adjust some default settings
- UI: Show full path on tab hover on Video Comparison ([#1336](/../../issues/1336))
- UI: Show full path on Video Comparison ([#1336](/../../issues/1336))
- UI: Fix crash with some mp4 files on Video Comparison ([#1258](/../../issues/1258))
- UI: Fix Animated GIF/PNG crashing on most settings
- NVEncC: Remove "--tune" parameter
- NVEncC: Add new "--vpp-nlmeans" parameter
- QSVEncC: Add new "--vpp-nlmeans" parameter
- SvtAv1EncApp: Add new "--frame-luma-bias" parameter
- x265: Fix few missing NO parameters ([#1344](/../../issues/1344))
- AviSynth: Fix Placebo preset ([#1335](/../../issues/1335))
- VapourSynth: Fix wrong loading order of Python scripts on multiple calls ([#1340](/../../issues/1340))
- Internal: Fix Linux compatibility a bit more ([#1337](/../../issues/1337), [#1348](/../../issues/1348))
- Update tools
    - ~~AutoCrop v2.3~~
    - DeeZy v0.1.9
    - MediaInfo v24.04
    - NVEncC v7.53
    - QSVEncC v7.64
    - SvtAv1EncApp v2.0.0-A-0+4-e8223021_Mod_by_Patman_x64_msvc1939 [SVT-AV1-PSY]
- Update Dual plugins
    - BestSource R4
- Update VapourSynth plugins
    - vs-imwri r2
    - mvsfunc r10


v2.38.7 (2024-05-02)
====================

- UI: Fix import and setting of encoder parameters
- VapourSynth: Fix potential wrong loading order of Python scripts
- Internal: Small fixes, improvements, changes
- Internal: Fix crash using Auto Crop on extremely short videos with Dolby Vision ([#1327](/../../issues/1327))
- Internal: Fix ignoring of existing Dolby Vision metadata in temp folder
- Internal: Fix Linux compatibility a bit more ([#1331](/../../issues/1331))


v2.38.6 (2024-04-26)
====================

- Log: Extend logging for Dolby Vision metadata files
- UI: Show TaskDialog timeout when set
- UI: Fix Auto Crop not working properly on videos with multiple aspect ratios ([#1324](/../../issues/1324))
- Internal: Fix Linux compatibility a bit more ([#1326](/../../issues/1326), [#1329](/../../issues/1329))


v2.38.5 (2024-04-25)
====================

- UI: Adjust StaxRip Update Check to perform cascade-like
    - More frequent checks on new versions, less frequent checks the older the version gets
- Internal: Fix Linux compatibility with macros and path handling


v2.38.4 (2024-04-25)
====================

- UI: Extend max number of frames used for Auto Crop ([#1324](/../../issues/1324))
- Internal: Fix Linux compatibility that caused issues with macros


v2.38.3 (2024-04-24)
====================

- Internal: Fix Linux compatibility that caused Blu-ray folders not being found anymore


v2.38.2 (2024-04-24)
====================

- UI: Load new Startup Template when changed in Settings ([#1229](/../../issues/1229))
- UI: Improve Error Messages
- UI: Make Frame Mismatch more robust towards wrong script information ([#1315](/../../issues/1315), [#1322](/../../issues/1322))
- UI: Fix not or delayed updating target parameters ([#1315](/../../issues/1315), [#1322](/../../issues/1322), [#1323](/../../issues/1323))
- FFmpeg: Fix output file extensions when using some codecs
- NVEncC: Fix "--vmaf" parameter usage
- NVEncC: Fix "--tune" parameter
- NVEncC: Bring back "--lossless" and "--lowlatency" parameters as they are not covered by new "--tune" modes
- SvtAv1EncApp: Extend SVT-AV1-PSY-Support


v2.38.1 (not published)
====================

- Internal: Bring "Improve Linux compatibility" into master (thanks to @Valdiralita ([#852](/../../pull/852)))


v2.38.0 (2024-04-16)
====================

- UI: Add "Changelog" to Main Menu
- UI: Add "Discord Server" to Main Menu
- UI: Add "Exit" to Main Menu
- UI: Add "Launch new instance" to Main Menu
- UI: Add "Report an issue" to Main Menu
- UI: Add "Support" to Main Menu
- UI: Add "What's new" to Main Menu
- UI: Rearrange Project in Main Menu
- UI: Rearrange Encoder Profiles
- UI: Move temp file deletion from Settings to Options
- UI: Add options to separate temp file deletion based on file types ([#1236](/../../issues/1236))
- UI: Improve memory usage
- UI: Improve performance with script handling, Assistant, Preview, Code Editor and interface
- UI: Improve script update performance (thanks to @Valdiralita ([#1262](/../../pull/1262)))
- UI: Show Preview Info by default
- UI: Make Preview a normal window by default
- UI: Add "Output Mod Direction" to allow decreasing when auto cropping or correcting is applied ([#1285](/../../issues/1285))
- UI: Fix Saving of jobs on network shares ([#1308](/../../issues/1308))
- UI: Fix Import Command Line for checkboxed parameters
- UI: Output Highlighting for Pipe symbol
- UI: Add Plugin fulfillment requirements
    - If a requirement is not fulfilled, the import will be commented out
- UI: Shorten "What's new" report if needed
- UI: Remove Update question at first start
    - Update Check is enabled by default and can be disabled in the Settings
    - According to the poll on Discord
- NVEncC: Add new --lookahead-level parameter
- NVEncC: Add new --tf-level parameter
- NVEncC: Add new --tune parameter
- SvtAv1EncApp: Add SVT-AV1-PSY Support
- Update tools
    - AOMEnc v3.8.2-397-ga4420e55a8-x64-msvc1937
    - DoVi_Tool v2.1.1
    - eac3to v3.51
    - MediaInfo v24.03
    - Mp4Box v2.3-DEV-rev1042-g7a3eca90b-x64-msvc1937
    - NVEncC v7.50
    - qaac v2.82
    - rav1e v0.7.0-(p20240409)-x64-gcc13.2.0
    - Subtitle Edit v4.0.5
    - SvtAv1EncApp v2.0.0-7+6-62325458-.Mod-by-Patman.-x64-msvc1937
    - x264 v0.164.3190+7-f54976d-.Mod-by-Patman.-x64-gcc13.2.0
    - x265 v3.6+2+13-9a3dac6e5-.Mod-by-Patman.-x64-avx2-msvc1937
- Update Dual plugins
    - BestSource R2-RC4
    - L-SMASH-Works v1194
- Update VapourSynth plugins
    - mvsfunc v11 [865c748 / 2023]
    - SubText R5


v2.37.6 (2024-04-12)
====================

- UI: Add Progress for dovi_tool and hdr10plus_tool
- UI: Adjust Progress Reformatting for SvtAv1EncApp
- UI: Improve saving of Jobs List to avoid data loss on full disk
- VapourSynth: Fix missing loading of SubText for hardcoded subtitles ([#1307](/../../issues/1307))


v2.37.5 (2024-04-06)
====================

- UI: Fix asking for Project saving on Templates
- UI: Optimize ignorable Output Mod warning ([#1285](/../../issues/1285))
- SvtAv1EncApp: Update settings and parameters to v2.0.0 ([#1273](/../../issues/1273))
- vvencFFapp: Fix display for Compression Check Run ([#1284](/../../issues/1284))


v2.37.4 (2024-03-28)
====================

- UI: Trigger event "While Processing" more often, but also time limited
- UI: Make timeout for error messages on job processing optional
- UI: Convert tabs to spaces on Processing window for better looking output ([#1243](/../../issues/1243))
- UI: Fix not asking for Project saving when dropping file under some circumstances
- UI: Make Output Mod warning ignorable ([#1285](/../../issues/1285))


v2.37.3 (2024-03-23)
====================

- UI: Fix tonemapping for HDR videos for Crop window not working under some circumstances
- UI: Set Process Priority from "Idle" to "Below Normal"
- NVEncC: Fix wrong parameter settings for "--vpp-nvvfx-denoise" and "--vpp-nvvfx-artifact-reduction" ([#1283](/../../issues/1283))


v2.37.2 (2024-03-22)
====================

- UI: Extend Crop window with Time information ([#1274](/../../issues/1274))
- UI: Update and extend Show Changelog
- UI: Update and adjust Updater


v2.37.1 (2024-03-18)
====================

- UI: Fix crash on AutoCrop selection in Options on Templates ([#1276](/../../issues/1276))


v2.37.0 (2024-03-17)
====================

- UI: Enhance Auto Crop for Dolby Vision sources
- UI: Improve handling with Dolby Vision videos and manual cropping
- UI: Re-read modified Dolby Vision metadata automatically on window focus
- UI: Improve Dolby Vision metadata handling
- UI: Enhance "Frame Mismatch" error message
- UI: Let "Frame Mismatch" ignore cutted "Copy/Mux" runs ([#1234](/../../issues/1234))
- UI: Write "Media Info Source File" only if it has changed ([#1221](/../../issues/1221))
- UI: Take given HDR10+/DolbyVision metadata into account when calculate target size/bitrate ([#1254](/../../issues/1254))
- UI: Fix rounding issue with number edits when using mouse wheel ([#1253](/../../issues/1253))
- UI: Fix rare crash on Audio and Muxer window opening
- FFmpeg: Add "-hwaccel cuda" to command line when NVIDIA decoder is selected ([#1260](/../../issues/1260))
- NVEncC: Rearrange some parameters ([#1240](/../../issues/1240))
- NVEncC: Enable Dolby Vision only when H265 is selected ([#1261](/../../issues/1261))
- NVEncC: Add "--vpp-nvvfx-artifact-reduction" parameter ([#1240](/../../issues/1240))
- NVEncC: Add "--vpp-nvvfx-denoise" parameter ([#1240](/../../issues/1240))
- NVEncC: Fix "--vpp-resize" parameter setting for "nvvfx-superres"
- QSVEncC: Enable Dolby Vision only when H265 is selected
- SvtAv1EncApp: Fix Passes could also affect Quality mode ([#1267](/../../issues/1267))
- VCEEncC: Add "--vpp-denoise-dct" parameter
- VCEEncC: Fix problem with missing space before parameter call
- VCEEncC: Remove Dolby Vision support
- x265: Add "--auto-aq" parameter ([#1241](/../../issues/1241))
- Update tools
    - DeeZy v0.1.8
    - eac3to v3.50
    - MKVToolNix v83.0
    - MP4Box v2.3-DEV-rev975-ge50da0656-x64-msvc1937
    - NVEncC v7.46
    - Python v3.12.2
    - QSVEncC v7.62
    - rav1e v0.7.0-(p20240312)-x64-gcc13.2.0
    - Subtitle Edit v4.0.4
    - SvtAv1EncApp v2.0.0-1+3-58b1b010-.Mod-by-Patman.-x64-msvc1937
    - VapourSynth R66
    - VCEEncC v8.22
    - vvencFFapp v1.11.1
    - x264 v0.164.3186+8-53164ba-.Mod-by-Patman.-x64-gcc13.2.0
    - x265 v3.5+156+14-df2e4c31e-.Mod-by-Patman.-x64-avx2-msvc1937
- Update AviSynth+ plugins
    - eedi3_resize16 v3.3.16
    - JPSDR v3.3.5 (W7 AVX2)
    - TIVTC v1.0.29
- Update Dual plugins
    - ffms2 v5.0 RC3
    - L-SMASH-Works v1183 [20240317]
- Update VapourSynth plugins
    - HQDN3D v1.00 mod


v2.36.0 (2024-02-23)
====================

- UI: "Import VUI metadata" doesn't override Dolby Vision profile depending on selected mode ([#1212](/../../issues/1212))
- UI: Extend Assistant check for VBV settings when using Dolby Vision to custom parameters ([#1213](/../../issues/1213))
- UI: Fix Crop bug that caused unnecessary and sometimes wrong cropping
- UI: Set "Output Mod" default value to 2 (inspired by the poll on Discord)
- UI: Improve handling/finding with metadata files
- UI: Fix issue with dovi_tool not being able to handle paths with comma ([#1238](/../../issues/1238))
- UI: Add threshold for cropping Dolby Vision videos based on metadata
- UI: Split command "ShowHardcodedSubtitleDialog" into "[..]DialogFromLastSourceDir" and "[..]DialogFromTempDir" ([#1199](/../../issues/1199))
- UI: Add new event "While Processing" which is called once every percent of progress on video encoders ([#1197](/../../issues/1197))
- UI: Add new macros that are currently only supported by the new "While Processing" event 
    - %commandline%, %progress%, %progressline%
- UI: Add new macros
    - %jobs%, %jobs_active%
- UI: Improve filename parsing for subtitles ([#1200](/../../issues/1200)), ([#1220](/../../issues/1220))
- UI: Improve track name parsing from filenames ([#1220](/../../issues/1220))
- UI: Improve Resize filter Assistant check for Dolby Vision
- UI: Add ".mks" to valid subtitle file types ([#1227](/../../issues/1227))
- UI: Adjust resize slider on main window
- UI: Make Settings window a bit higher ([#1211](/../../issues/1211))
- NVEncC: Add support for Dolby Vision metadata handling
- NVEncC: Fix wrong "--vpp-resize" parameter options visibility
- QSVEncC: Add support for Dolby Vision metadata handling
- QSVEncC: Add "Show advanced QP settings"
- QSVEncC: Add "--vpp-denoise-dct" parameter
- VCEEncC: Add support for Dolby Vision metadata handling
- VCEEncC: Add Dolby Vision parameters to encoder options
- VCEEncC: Fix "--cqp" parameter value for HEVC codec ([#1222](/../../issues/1222))
- Update tools
    - mpv.net v7.1.1.0
    - Python v3.11.7
    - QSVEncC v7.61
    - vvencFFapp v1.11.0
- Update Dual plugins
    - ffms2 v3.0.1.0 1357+34 r1391 4fbfa13ea1 StvG
- Update VapourSynth plugins
    - libhistogram v2.0 2021-11-13


v2.35.0 (2024-02-02)
====================

- eac3to: Let eac3to extract the Enhancement Layer for metadata extraction
- eac3to: Let eac3to don't overwrite existing video files
- UI: Set "Output Mod" default value to 4
- UI: Fix eac3to window title
- UI: Fix setting Crop from/to Dolby Vision RPU files
- UI: Move "Auto Crop on opening" to Options
- UI: Extend "Auto Crop on opening" to run only when Dolby Vision metadata exists
- UI: Let "Auto Crop" use Dolby Vision metadata to set the crop values
- UI: Some small but cool stuff I have forgotten to write down
- Update tools
    - DeeZy v0.1.7
    - eac3to v3.48
    - MediaInfo v24.01


v2.34.0 (2024-01-26)
====================

- CommandLine: Add new parameter "-SetCrop" to set the crop values, best used with a source file as first parameter
- UI: Fix delay when opening and closing Muxer/Container options window
- UI: Fix Assistant displaying issues
- UI: Fix crash on sources with multiple video tracks on newer versions of eac3to
- UI: Fix Dolby Vision wrong RPU file handling ([#1190](/../../issues/1190))
- UI: When a Dolby Vision metadata file is used for "Import VUI metadata", the Crop filter will be applied
- UI: Let "HDR metadata extraction" also set the json file when an rpu file was found/set
- UI: Add option to save Preview window size ([#986](/../../issues/986))
- UI: Improve "Output Highlighting"
- Make number of audio tracks on main window optional
- NVEncC: Add --vpp-denoise-dct parameter
- x265: Add Assistant check for VBV settings when using DolbyVision
- Update tools
    - chapterEditor v1.42
    - DeeZy v0.1.6
    - eac3to v3.47
    - libFLAC v1.4.3
    - NVEncC v7.41
    - qaac v2.81
    - vvencFFapp v1.10.0 r209-8fe23c7
- Update AviSynth+ plugins
    - SMDegrain v4.5.0d
    - xy-VSFilter + XySubFilter v3.2.0.810
- Update Dual plugins
    - ffms2 v3.0.1.0 1357+36 r1393 1c0dcfa391 StvG
    - L-SMASH-Works v1167 [20240112]


v2.33.0 (2024-01-15)
====================

- UI: Audio import via container options reads the track name from the file name ([#1184](/../../issues/1184))
- UI: Enhance "HDR metadata extraction"
- UI: Extend two-letter language codes
- UI: Fix crash on "HDR metadata extraction" on some systems
- UI: Fix language recognition of subtitles from paths ([#1174](/../../issues/1174))
- UI: Fix ffmpeg not being able to extract "Timed Text" subtitles properly ([#1185](/../../issues/1185))
- UI: Improve "Output Highlighting"
- UI: Let ffmpeg Demuxer also extract all subtitles
- UI: Run "Frame Mismatch" only on video files
- UI: Better handling for "Frame Mismatch" errors


v2.32.0 (2024-01-08)
====================

- CommandLine: Add new parameter "-ExitWithoutSaving" to exit StaxRip without saving an unsaved project
- Thumbnailer: Fix crash on some video files with higher Bit Depth
- UI: Add HDR metadata extraction to project options
- UI: Add HDR metadata file to encoder settings when using "Import VUI metadata"
- UI: Search for HDR metadata file next to source file and in temp folder to let it import via "Import VUI metadata"
- UI: Add option to show an error message on "Frame Mismatch" after encoding
- UI: Add "DefaultOrFirst" to "Default Subtitles" option
- UI: Add "Close Project" to Main Menu
- UI: Change default option for "Default Subtitles" to "Default"
- UI: Enhance "Output Highlighting"
- UI: Enhance language recognition of subtitles from paths ([#1174](/../../issues/1174))
- UI: Fix subtitles not being enabled after import even if set so ([#1174](/../../issues/1174))
- UI: Fix UI response delay when opening "Audio Settings" window ([#1175](/../../issues/1175))
- UI: Fix demuxing VOBs ignores settings ([#577](/../../issues/577))
- UI: Multiple minor fixes and improvements
- Update tools
    - AOMEnc v3.8.0-188-g1cc70eeadf-x64-msvc1937
    - DoVi_Tool v2.1.0
    - HDR10Plus_Tool v1.6.0
    - MKVToolNix v82.0
    - MP4Box v2.3-DEV-rev724-g8684dfbcc-x64-msvc1937
    - QSVEncC v7.58
    - rav1e v0.7.0-(p20240102-2-g7d773ea)-x64-gcc13.2.0
    - Subtitle Edit v4.0.3
    - SvtAv1EncApp v1.8.0-3+6-0bd9640d-[Mod-by-Patman]-x64-msvc1937
    - vvencFFapp v1.10.0 r208-c2ace2a
    - x265 v3.5+153+15-813ccbff0-.Mod-by-Patman.-x64-avx2-msvc1937
- Update AviSynth+ plugins
    - QTGMC v3.385s (2023-12-28)
    - xy-VSFilter + XySubFilter v3.2.0.809
- Update Dual plugins
    - L-SMASH-Works v1164 [20240106]


v2.31.0 (2024-01-01)
====================

- UI: Fix crash at startup on non Vulkan systems ([#1158](/../../issues/1158))
- UI: Fix error message on source files with a long file path ([#1168](/../../issues/1168))
- UI: Fix recognizing and interpreting source (subtitle) languages ([#1120](/../../issues/1120), [#1152](/../../issues/1152))
- UI: Fix issues with disappearing Cutting filter and the misleading Assistant message ([#1124](/../../issues/1124))
- UI: Fix command line long path issues for qaac ([#1166](/../../issues/1166))
- UI: Fix flickering on Video Comparison when window too small to fit all tabs ([#1127](/../../issues/1127))
- UI: Shorten long file names in Video Comparison to enhance usability
- UI: Switch languages internally from 2-/3-letter codes to country-language except preferred languages options, not completely yet
- NVEncC: Fix missing Multipass parameter for QBVR mode ([#1162](/../../issues/1162))
- NVEncC: Fix minor issues with invisible parameters due to selection order
- NVEncC: Fix incomplete definition of --device parameter
- NVEncC: Fix incomplete definition of --split-enc parameter
- NVEncC: Rename QBVR mode quality setting name ([#1162](/../../issues/1162))
- vvencFFapp: Extend "Frames to be encoded" for templates ([#1161](/../../issues/1161))
- Update tools
    - AOMEnc v3.8.0-178-ge065e0fead-x64-msvc1938
    - FFmpeg N-113112-gf5f414d9c4-x64-gcc13.2.0
    - MP4Box v2.3-DEV-rev724-g8684dfbcc-x64-msvc1938
    - Python v3.11.4
    - rav1e v0.6.1 (p20231226)-x64-gcc13.2.0
    - SvtAv1EncApp v1.8.0-1+3-43997134-[Mod-by-Patman]-x64-msvc1938
    - VapourSynth R65
    - x264 v0.164.3172+8-d0a13eb-.Mod-by-Patman.-x64-gcc13.2.0
    - x265 v3.5+153+14-caf1c2580-.Mod-by-Patman.-x64-avx2-msvc1938
- Update AviSynth+ plugins
    - TDeint v1.9
    - TIVTC v1.0.28
- Update Dual plugins
    - ffms2 v3.0.1.0 1357+34 r1391 4fbfa13ea1 StvG
    - L-SMASH-Works v1160 [20231214]


v2.30.0 (2023-12-17)
====================

- Log: Fix not showing Media Info of source files in logs ([#1114](/../../issues/1114))
- UI: Add Frame Rate Mode information on main window for source files
- UI: Optimize addition of Tonemapping in Crop window
- UI: Fix Video Comparison not being able to open more files when source file is loaded
- UI: Fix progress detections for ffmpeg depending on used codec
- UI: Fix progress detections for opusenc ([#1147](/../../issues/1147))
- UI: Fix progress detections for given percentual values ([#1143](/../../issues/1143))
- UI: Add Auto-Tonemapping support for Thumbnailer
- UI: Fix ShowMessageBox Icon parameter
- UI: Fix ShowMessageBox call through Command Line leads to an exception message ([#1141](/../../issues/1141))
- UI: Add Flip filter profiles
- DeeZy: Fix audio delay handled by encoder and muxer
- FFmpeg: Add hardware AV1 support for AMD/Intel/NVidia ([#1145](/../../issues/1145))
- NVEncC: Add --qvbr parameter ([#1139](/../../issues/1139))
- NVEncC: Add --output-csp parameter
- NVEncC: Add --disable-nvml parameter
- NVEncC: Add Quiet Log Level
- NVEncC: Fix AV1 CQ parameter values ([#1153](/../../issues/1153))
- QSVEncC: Add Quiet Log Level
- QSVEncC: Add --vpp-rff parameter
- QSVEncC: Make --icq mode default encode mode
- rav1e: Fix wrong parameter spelling that crash the encode ([#1138](/../../issues/1138))
- SvtAv1EncApp: Add new SSIM tune option ([#1118](/../../issues/1118))
- SvtAv1EncApp: Extend --keyint parameter options
- SvtAv1EncApp: Fix --enable-qm parameter definition ([#1122](/../../issues/1122))
- SvtAv1EncApp: Fix chunk encoding ([#1136](/../../issues/1136))
- VCEEncC: Add AV1 support ([#1117](/../../issues/1117))
- VCEEncC: Add --vpp-rff parameter
- VCEEncC: Add --vpp-scaler-sharpness parameter
- VCEEncC: Fix some issues and codec related params
- VCEEncC: Remove --vpp-resize amf_point parameter option
- vvencFFapp: Add new presets
- x265: Fix --log-level param options
- Update tools
    - DeeZy v0.1.4
    - eac3to v3.36
    - FFmpeg N-112998-g1f56bfc986-20231216
    - MediaInfo v23.11
    - MKVToolNix v81.0
    - NVEncC v7.40
    - QSVEncC v7.57
    - Subtitle Edit v4.0.2
    - VCEEncC v8.21
    - vvencFFapp v1.10.0
- Update AviSynth+ plugins
    - avs_libplacebo v1.5.2
    - RemoveDirt v0.9.3
- Update Dual plugins
    - L-SMASH-Works v1156 [20231117]
- Update VapourSynth plugins
    - None


v2.29.0 (2023-09-06)
====================

- Add header for target file Media Info
- Add new (event) command to open blocking Preview window ([#1102](/../../issues/1102))
- Extend title name length for demuxed file names
- Log mismatch between frame count from FrameServer and target file after encode
- UI: Show Changelog at first start
- UI: Add possiblity to dismiss the PNGs saved message in VideoComparison ([#1067](/../../issues/1067))
- UI: Add proper identification for HDR tonemapping in Crop window ([#1096](/../../issues/1096))
- UI: Add Shortcut for "Save Project As"
- UI: Allow multiple file selection in muxer for audio files
- UI: Apply Crop filter when changing Frameserver
- UI: Change Crop filter addition/setting behaviour
- UI: Enhance Command Line Highlighting
- UI: Enhance Output Highlighting
- UI: Fix broken progress detection for ffmpeg ([#1109](/../../issues/1109))
- UI: Fix crash when trying to open the menu in Processing window
- Filters: Fix Vapoursynth filter resamplehq linkage ([#1101](/../../issues/1101))
- Filters: Adjust SMDegrain presets
- DeeZy: Fix issue with negative delay
- QSVEncC: Fix bitrate issue in QBVR mode ([#1106](/../../issues/1106))
- x265: Minor UI improvements
- x265: Remove deprecated params --hist-threshold and --traditional-scenecut ([#1110](/../../issues/1110))
- Update tools
    - AOMEnc v3.7.0-363-g02b419c62e-x64-msvc1937
    - chapterEditor v1.40
    - DeeZy v0.1.1
    - FFmpeg N-111918-g0adaa90d89-x64-gcc13.2.0
    - libFLAC v1.3.3
    - MP4Box v2.3-DEV-rev512-g0cdcdbaaa-x64-msvc1937
    - NVEncC v7.31
    - Subtitle Edit v4.0
    - SvtAv1EncApp v1.7.0-0+13-b922871f-[Mod-by-Patman]-x64-msvc1937
    - vvencFFapp v1.9.0 r189-19efe30
    - x265 v3.5+147+17-e8947f740-.Mod-by-Patman.-x64-avx2-msvc1937
- Update AviSynth+ plugins
    - Descale v2023-04-02 8c53f5d
    - JincResize v2.1.2
    - vsMSharpen v2.0.1
- Update VapourSynth plugins
    - ASTDR v4
    - DeCross v1
    - Descale v2023-04-02 8c53f5d
    - DFTTest2 v6
    - insaneAA v0.91
    - MotionMask v2
    - TemporalSoften2 v1


v2.28.0 (2023-08-25)
====================

- Add BackgroundColor from VideoComparison to Themes
- Make line numbers added to code lines optional via Settings
- Fix Code Preview not shown in selected windows/app
- Reloading of Preview does not change the window size any more
- DeeZy: Fix output progress detectation ([#1092](/../../issues/1092))
- SvtAv1EncApp: Fix --enable-hdr parameter printout
- SvtAv1EncApp: Fix wrong frames value
- VapourSynth: Fix extreme rare bug when opening a source which causes a crash
- vspipe: Fix deprecated parameter call
- Update tools
    - MKVToolNix v79.0
- Update VapourSynth plugins
    - SubText r4
    - VSFilterMod v5.2.7


v2.27.0 (2023-08-22)
====================

- Fix grammar (thanks to @LittleVulpix ([#1083](/../../pull/1083)))
- Fix pathing issue on VapourSynth
- Adjust SMDegrain presets


v2.26.0 (2023-08-22)
====================

- Big VapourSynth update from R54 to R63 (Big thanks to @DJATOM and @jlw4049 for contributing, @pat-e and @Patman86 for helping and testing!)
- Remove no-pipe call for x264 using VapourSynth due to incompatibility reasons
- Add .mpeg as supported input file type for DGIndex/D2VWitch
- Add DeeZy and OpusEnc as new audio encoders
- Add new Tonemap presets
- Add option to tonemap videos in Crop window
- Add option to enhance contrast in Crop window for easier cropping
- Add Reload command in Preview
- Add line numbers to Code Preview
- Let Enabling/Disabling a filter in Code Editor make refresh the script for the preview
- Fix shortcut for Preview in Code Editor
- Fix command line parameter issue with quotes
- Improve Output Highlighting a tiny bit
- Check also for negative error codes
- Show warning for Windows 7 users regarding possible incompatibility issues at first start
- Add new macros %audio_bitrate1%, %audio_bitrate2%, %audio_channels1%, %audio_channels2%, %audio_codec1%, %audio_codec2% ([#1075](/../../issues/1075))
- Rename macro %delay1% to %audio_delay1% and %delay2% to %audio_delay2%
- Fix "Auto Chroma Subsampling to 4:2:0" for VapourSynth not working on some sources ([#1033](/../../issues/1033))
- Fix internally called AutoCrop not adding/enabling Crop-filter ([#1075](/../../issues/1075))
- Fix internally called SmartAutoCrop not adding/enabling Crop-filter
- Add more resize defaults ([#1079](/../../issues/1079))
- Add possibility to reset window position when holding CTRL+SHIFT while window opens
- Adjust Resize slider minimum/maximum values
- Fix logs from processes being saved into templates ([#905](/../../issues/905))
- SvtAv1EncApp: Fix infinite app calling loop when SvtAv1EncApp encoder profile selected while opening a file ([#1068](/../../issues/1068))
- SvtAv1EncApp: Fix wrong multi-pass paremeter combination ([#1076](/../../issues/1076))
- SvtAv1EncApp: Add proper 3-pass support
- SvtAv1EncApp: Move Passes option to rate control to keep related settings together
- Update tools
    - AOMEnc v3.6.1-1047-gea674a22d0-x64-msvc1937
    - DeeZy v0.1.0
    - ffmpeg 6.0 N-111762-ga1928dff2c-x64-gcc13.2.0
    - libFLAC v1.4.3 (used by eac3to)
    - libsndfile v1.2.2 (used by qaac)
    - Mp4Box v2.3-DEV-rev478-g892852666-x64-msvc1937
    - mpv.net v6.0.4.0
    - OpusEnc v1.4.6
    - qaac v2.80
    - QSVEncC v7.48
    - rav1e v0.6.1-(p20230808)-x64-gcc13.2.0
    - SvtAv1EncApp v1.6.0-12+12-02ddf03c-[Mod-by-Patman]-x64-msvc1937
    - VapourSynth R63
    - vvencFFapp v1.9.0
    - x264 v0.164.3107+12-d987552-[Mod-by-Patman]-x64-gcc13.2.0
    - x265 v3.5+104+20-5f9a54004-[Mod-by-Patman]-x64-msvc1937
- Update AviSynth+ plugins
    - AnimeIVTC v2.389 2023-07-20 mod
    - BBorders v2023-04-28
    - LSFplus v6.0 mix
    - NNEDI3CL v1.0.8
    - ScenesPack v4.5
- Update Dual plugins
    - L-SMASH-Works v1129.0.1.0 20230806
- Update VapourSynth plugins
    - AddGrain r10
    - Bwdif r4.1
    - edi_rpow2 v2021
    - EEDI3m r4
    - fmtconv r30
    - fvsfunc v2022-10-14
    - havsfunc r33 [modified by @jlw4049 and @Dendraspis]
    - LibDedot v1.0
    - LibP2P R2
    - libvs_placebo v1.4.4
    - mcdegrainsharp v2020-11-03
    - MiscFilters R2
    - muvsfunc v0.4.0
    - mvmulti v2020-14-10
    - mvsfunc r10
    - nnedi_rpow2 v1.1.0
    - NNEDI3CL v8
    - Oyster v2021-05-16 [modified by @jlw4049 and @Dendraspis]
    - Plum v2021-03-27 [modified by @jlw4049 and @Dendraspis]
    - resamplehq v2.1.2
    - RemoveGrain r1
    - SangNom r42
    - TCanny v14
    - TimeCube v3.1
    - TTempSmooth v4.1
    - vcmod 2022-02-10.AC3
    - Vine v2020-07-12 [modified by @jlw4049 and @Dendraspis]
    - vsTAAmbk v0.8.2
    - znedi3 r2.1
    - ...and some more existing scripts updated to be compliant with VapourSynth R63


v2.25.0 (2023-08-02)
====================

- Alter some main menu shortcuts
- Reorganize main menu slightly
- Save settings when selecting new startup template ([#1057](/../../issues/1057))
- Add Reload function to Video Comparison
- Add Video Comparison support for AviSynth and VapourSynth scripts ([#1055](/../../issues/1055))
- Minor changes to progress detection
- Fix broken progress detection for ffmpeg ([#1054](/../../issues/1054))
- Add Commentary flag setting for audio tracks ([#959](/../../issues/959))
- Add Commentary flag setting for subtitle tracks ([#959](/../../issues/959))
- Add Hearing Impaired flag setting for subtitle tracks ([#960](/../../issues/960))
- SvtAv1EncApp: Fix wrong parameter for encoding frames
- SvtAv1EncApp: Add support for modified progress in @Patman86's mod
- Update tools
    - SvtAv1EncApp v1.6.0-4+7-0abb2b72-[Mod-by-Patman]-x64-msvc1936
- Update AviSynth+ plugins
    - avs_libplacebo v1.3.0
    - DPID v1.1.0
    - MasksPack v6.7
    - Resizers Functions Pack v12.0
    - TransformsPack v2.2.0
- Update Dual plugins
    - FFTW v3.3.10 (thanks to Nuihc88)


v2.24.0 (2023-07-29)
====================

- AVS: Fix InterFrame not working with SVPFlow ([#1046](/../../issues/1046))
- AOMEnc: Fix not saved target bitrate for CQ rate mode ([#1052](/../../issues/1052))
- SvtAv1EncApp: Fix deprecated URLs
- SvtAv1EncApp: Fix output path with spaces causing error ([#1045](/../../issues/1045))
- SvtAv1EncApp: Move Passes to Basic category
- SvtAv1EncApp: Add --progress parameter
- SvtAv1EncApp: Fix encoding output and crash for  "--progress 2" ([#1045](/../../issues/1045))
- Update tools
    - chapterEditor v1.39
    - MP4Box v2.3-DEV-rev472-g2a2327c3b
    - vvencFFapp v1.9.0
- Update AviSynth+ plugins
    - edi_rpow2 v1.0 mod 87
- Update Dual plugins
    - FFTW v3.3.10/v3.3.5


v2.23.0 (2023-07-24)
====================

- Add shortcut "Ctrl + E" to Event Commands
- Add vertical scrollbar to Event Commands to make it less glitchy when adding multiple criteria - still glitchy ([#1048](/../../issues/1048))
- Add new macro %encoder_profile% ([#1047](/../../issues/1047))
- Create separate category for "Add filter to convert chroma subsampling to 4:2:0" added filter, after "Source" for progressive and at the end for interlaced sources (thanks to youer-mam) ([#1021](/../../issues/1021))
- Fix crashing bug in Event Commands
- Fix typo ([#1042](/../../issues/1042))
- Rename macros %video_encoder_profile% to %encoder_profile% and %video_encoder_settings% to %encoder_settings%
- Move "Add filter to convert chroma subsampling to 4:2:0" to Project
- AOMEnc: Multiple changes to default values ([#1012](/../../issues/1012))
- AOMEnc: Fix missing target bitrate for constrained quality ([#1044](/../../issues/1044))
- QSVEncC: Add --vpp-pmd and --vpp-denoise parameters ([#1016](/../../issues/1016))
- QSVEncC: Add --vpp-perc-pre-enc parameter
- SvtAv1EncApp: Remake whole implementation and add parameters ([#893](/../../issues/893), [#1045](/../../issues/1045))
- vvencFFapp: Add basic implementation
- Update tools
    - AviSynth+ v3.7.3 (official release)
    - MediaInfo v23.07
    - MKVToolNix v78.0
    - QSVEncC v7.47
    - vvencFFapp v1.9.0-rc2 r182-3fcfd93
- Update AviSynth+ plugins
    - yadifmod2 v0.2.8
- Update Dual plugins
    - FFTW v3.3.5


v2.22.0 (2023-07-06)
====================

- Add conversion speed information relative to video framerate for x264 and x265 reformatted progress output ([#781](/../../issues/781))
- Fix stealing focus on job completion again ([#333](/../../issues/333))
- QSVEncC: Add missing parameters ([#1016](/../../issues/1016))
- Update tools
    - FFmpeg v6.0 N-111327-g695789eacc-x64-gcc13.1.0
    - Mp4Box v2.3-DEV-rev395-g98979a443-x64-msvc1936
    - qaac v2.80
    - x264 v0.164.3107+9-30c58f9-.Mod-by-Patman.-x64-gcc13.1.0
    - x265 v3.5+104+15-ba4e7a2cb-.Mod-by-Patman.-x64-msvc1936


v2.21.0 (2023-07-03)
====================

- AOMEnc: Fix "--matrix-coefficients" options ([#1023](/../../issues/1023))
- NVEncC: Fix "--lossless" being visible for H264 only
- QSVEncC: Add "--tile-col" and "--tile-row" parameters
- QSVEncC: Add "--max-framesize" parameter
- QSVEncC: Add "--hevc-gpb" parameter
- QSVEncC: Set "--tile-row 2" as default parameter/value
- QSVEncC: Fix some UI bugs
- Fix wrong channel recognition and extraction for temp audio files ([#1027](/../../issues/1027))
- Fix stealing focus on job completion ([#333](/../../issues/333))
- Fix opening multiple VOB files in multiple instances instead of running just one
- Replace app icon
- Update tools
    - AOMEnc v3.6.1-807-g7e0293d9c-x64-msvc1936
    - chapterEditor v1.38
    - MediaInfo v23.06
    - MKVToolNix v77.0
    - NVEncC v7.30
    - QSVEncC v7.46
    - rav1e v0.6.1-(p20230627-3-ge379128)-x64-gcc13.1.0
    - SvtAv1EncApp v1.6.0-x64-msvc1936
    - VCEEncC v8.16
- Update AviSynth+ plugins
    - CropResize 2023-06-02
    - ExTools v10.2
- Update Dual plugins
    - FFTW v3.3.10
    - SVPFlow1 v4.5.0.200 (Thanks to Nuihc88)
    - SVPFlow2 v4.3.0.161 (Thanks to Nuihc88)


v2.20.0 (2023-06-07)
====================

- Fix Updater due to changed GitHub pages ([#1019](/../../issues/1019))
- AVS: Fix wrong importation of dependencies
- x265: Selecting a Tune won't apply the preset settings anymore ([#894](/../../issues/894))
- Update tools
    - NVEncC v7.28
    - QSVEncC v7.44
    - VCEEncC v8.14
- Update AviSynth+ plugins
    - BWDIF v1.2.1 (due to QTGMC issues using EdiMode="BWDIF+NNEDI3")
- Update Dual plugins
    - SVPflow v4.2.0.133 (due to usage changes (red rectangle around the video frame))


v2.19.0 (2023-05-27)
====================

- Add possibility to open multiple source files. StaxRip opens each file in a new instance sequentially ([#926](/../../issues/926))
- FFmpeg: Remove "-strict experimental" from parameter call when av1 codec is used
- NVEncC: Add new "--split-enc" parameter
- QSVEncC: (better) AV1 support ([#919](/../../issues/919))
- QSVEncC: Add parameters
- Update tools
    - MP4Box v2.3-DEV-rev267-ga6ae93532
    - NVEncC v7.26
    - rav1e v0.6.6
    - VCEEncC v8.12
- Update AviSynth+ plugins
    - AvsResize vR21
    - AVSTP v1.0.4.1
    - BWDIF v1.2.5
    - ChubbyRain3
    - D2VSource v1.3.0
    - Deblock v2020-08-30
    - LSFmod v2.193 (fixed on 2022-10-21 with same version number)
    - NicAudio v2.0.6
    - nnedi3_resize16 v3.3 2023-03-27


v2.18.0 (2023-05-22)
====================

- Set timestamp extraction for VFR MKV files only as default project option ([#817](/../../issues/817), [#1006](/../../issues/1006))
- Fix minor issue with TransformsPack package definition
- Fix UI issues especially with hidden checkbox texts on some machines by adding UI Fallback setting [Settings � User Interface � UI Fallback] ([#978](/../../issues/978))
- NVEncC: Adjust some codec dependent parameters
- NVEncC: AV1 support ([#949](/../../issues/949))
- SVTAV1: Set "--key-init" to default value of -2 (thanks to samkatakouzinos ([#1011](/../../pull/1011)))
- Update tools
    - FFmpeg v6.0 N-110467-gefc2587260-g0e580806d8+2
    - rav1e v0.6.4
    - SVTAV1EncApp v1.5.0
- Update AviSynth+ plugins
    - CropResize v2022-11-19
    - JPSDR v3.3.4 (W7 AVX2)
    - SmoothD2 + SmoothD2C vA3
    - TemporalDegrain2 v2.6.6
    - TimecodeFPS v1.1.4
    - vsCnr2 v1.0.1
    - vsTTempSmooth v1.2.3
    - xy-VSFilter + XySubFilter v3.2.0.808
- Update Dual plugins
    - f3kdb Neo vR9
    - DFTTest Neo vR8
    - MiniDeen vR11
    - SVPflow v4.3.0.168
    - VSFilterMod v5.2.6


v2.17.0 (2023-05-20)
====================

- AVS: Add more frame rates to frame rate filter selections ([#995](/../../issues/995))
- AVS: Fix missing function and package definitions for TransformsPack ([#1009](/../../issues/1009))
- AVS: Prevent importing duplicates on dependencies
- FFmpeg: Fix audio (and subtitle) cutting issues caused by ffmpeg upstream ([#997](/../../issues/997)) (thanks to sheik124 ([#1005](/../../pull/1005)))
- NVEncC: Fix "--bref-mode" parameter options ([#1007](/../../issues/1007))
- Rename encoder and category/group names
- Update tools
    - AOMEnc v3.6.0-564-gea9a06fd1
    - AviSynth+ v3.7.3 r3982 Clang
    - chapterEditor v1.37
    - DGIndex v2.0.0.8
    - eac3to v3.36
    - FFmpeg v6.0 N-110665-g47430a3cb1-20230519
    - MKVToolNix v76.0
    - MP4Box v2.3-DEV-rev221-g200100727
    - mpv.net v6.0.3.2 beta
    - NVEncC v7.25
    - QSVEncC v7.41
    - Subtitle Edit v3.6.13
- Update AviSynth+ plugins
    - SMDegrain v4.4.0d
    - Zs_RF_Shared v1.161
- Update Dual plugins
    - DGMPGDec v2.0.0.8


v2.16.0 (2023-04-20)
====================

- NVEnc: Add "bicubic" option for "--vpp-resize algo" parameter
- QSVEnc: Add "--tune" parameter
- MP4Box: Can import EC3 files ([#999](/../../issues/999))
- Update tools
    - NVEncC v7.22
    - QSVEncC v7.37
- Update AviSynth+ plugins
    - GradePack v9.0
    - MasksPack v6.6
    - TransformsPack v1.6


v2.15.0 (2023-04-07)
====================

- Fix misleading language name fetch from path for subtitles ([#924](/../../issues/924))
- Add .av1 to valid input file extensions for mkvmerge ([#982](/../../issues/982))
- Add SharpenersPack functions for dependencies
- NVEncC: Add "--vpp-nnedi" fields ([#962](/../../issues/962))
- QSVEnc: Add "--output-depth" parameter ([#984](/../../issues/984))
- VCEEnc: Add missing/new options for "--vpp-resize" ([#921](/../../issues/921))
- VCEEnc: Add new "--no-deblock" parameter
- x265: Add missing/new options for "--scenecut-aware-qp" ([#891](/../../issues/891)) (thanks to sheik124 ([#917](/../../pull/917)))
- x265: Fix not working console help (thanks to sheik124 ([#917](/../../pull/917)))
- Update tools
    - ffmpeg n6.0-8-g18dde8d4cf-20230319
    - MediaInfo v23.03
    - NVEncC v7.21
    - qaac v2.79
        - libsndfile v1.2.0
        - libFLAC v1.4.2
    - QSVEncC v7.36
    - VCEEncC v8.11
- Update AviSynth+ plugins
    - QTGMC v3.384s (2022-10-17)
    - Sharpeners Pack v5.1


v2.14.0 (2023-03-12)
====================

- NVEnc: Add multiple new params
- NVEnc: Add missing/new options for "--vpp-resize" ([#921](/../../issues/921)), ([#964](/../../issues/964))
- VCEEnc: Add multiple new params
- x265: Fixed missing option for "--display-window"
- Fix AVS mClean not working properly due to Masktools update
- Fix AVS QTGMC not working properly
- Fix AVS SMDegrain not working properly due to missing dependencies ([#931](/../../issues/931))
- Update tools
    - AOMEnc v3.2.0-393-g402e264b9-x64-gcc11.3.0 Patman
    - MediaInfo v22.12
    - MKVToolNix v74.0
    - MP4Box v2.1-DEV-rev79-gdf29bc8a0-x64-gcc11.3.0 Patman
    - NVEnc v7.20
    - QSVEnc v7.35
    - Rav1e v0.5.0-(p20220426-4-gb5c76736)-x64-gcc11.3.0 Patman
    - SvtAv1EncApp v1.0.0-4-g879ba80a-x64-msvc1931 Patman
    - VCEEnc v8.10
- Update AviSynth+ plugins
    - ExTools v10.0
    - ModPlus v2020-06-26
    - Resizers Functions Pack v11.4
    - Sharpeners Pack v5.0
    - SMDegrain v4.0.0.d
- Update Dual plugins
    - DGMPGDec v2.0.0.7 (replace wrong version in v2.13.0)
    - ffms2 v3.0.1.0 1329+20 ad42af1 StvG


v2.13.0 (2022-05-10)
====================

- x265: Add "--aq-bias-strength" param for AQ Modes 3 and 5
- svt-av1: Add and update multiple params ([#863](/../../issues/863))
- Show current frame number on Crop window on status bar
- Update tools
  - NVEnc v6.01
  - VCEEnc v7.00
- Update AviSynth+ plugins
  - AddGrainC v1.8.4
  - AnimeIVTC v2.386 2022-03-20 mod
  - Average v0.95
  - AvsResize r14
  - CropResize 2022-01-28
  - D2VSource v1.2.4
  - Deblock_QED 2020.04.06 HBD
  - DeHalo_alpha 2021.04.05
  - DeHaloHmod v2.472
  - DTFTest v1.9.7 (MSVC)
  - EEDI2 v1.0.0
  - FFT3DFilter v2.10
  - hqdn3d v1.1.1
  - JPSDR v3.2.8 (W7 AVX2)
  - Masktools2 v2.2.30
  - pSharpen 2020.10.31 HBD
  - QTGMC v3.384s
  - ResizeX v1.0.1 mod 9.40
  - SMDegrain v3.5.0d
  - TComb v2.3
  - TNLMeans v1.1
  - VapourSource v0.2
  - Vinverse v0.9.4
  - Vscube v1.3
  - Zs_RF_Shared v1.158
- Update Dual plugins
  - DGIndex v2.0.0.7
  - DGMPGDec v2.0.0.7
  - ffms2 v3.0.1.0 1325+16 6ad7738 StvG
  - L-SMASH-Works v20220505


v2.12.0 (2022-04-19)
====================

- x265: Add new supported option "--aq-mode 5", which was introduced in x265 aMod (DJATOM, Patman)
- NVEnc: Add new param "--vpp-convolution3d"
- QSVEnc: Add experimental AV1 codec
- Fix not muxing attachments due to opposed working checkbox ([#847](/../../issues/847))
- Fix missing VS plugin due to G41Fun update ([#848](/../../issues/848))
- Remove unneccessary quotation marks from Command Line Audio Profile ([#853](/../../issues/853))
- Use pipe when modifying channels for qaac ([#854](/../../issues/854))
- Use existing/cached video information after muxing instead of reloading the script to prevent broken generated files ([#777](/../../issues/777))
- Update tools
  - AviSynth+ v3.7.2
  - MKVToolNix v67.0
  - NVEnc v6.00
  - QSVEnc v7.01
- Update VapourSynth plugins
  - vcm v2020-09


v2.11.0 (2022-04-02)
====================

- Fix typo in Settings Directory dialog (bdr99, [#798](/../../issues/798))
- Update parameters for AOMEnc (badcf00d, Dendraspis, [#821](/../../issues/821))
- Rearrange SMDegrain's list position and definition for VS ([#797](/../../issues/797))
- Add "Processing" to remembered window positions
- Add new macros %random:digits%, %current_date%, %current_time%, %current_time24% ([#802](/../../issues/802))
- Fix UI issues regarding mkvextract
- Fix remove confirmation on Lists showing first item name only even if multiple items are selected
- Reorganize encoder profiles
- Add NVEnc parameters --lut3d, --lut3d_interp, --dolby-vision-rpu, --dolby-vision-profile
- Add VCEEnc parameter --thread-affinity, --qvbr, --qvbr-quality ([#792](/../../issues/792))
- Add QSVEnc parameters --thread-affinity, --dhdr10-info, --dolby-vision-rpu, --dolby-vision-profile, --qvbr, --qvbr-quality
- Add x265 Dolby Vision Profile 8.4
- Fix minor mkvextract demux issue ([#833](/../../issues/833))
- Add demux support for DVBSUB subtitles ([#833](/../../issues/833))
- Add new audio command line macros %streamid0% and %streamid1% ([#842](/../../issues/842))
- Raise x265 atc-sei parameter upper limit ([#815](/../../issues/815))
- Optimize Check for Updates
- Show also the path of the project when it's not found
- Don't close Jobs window if project fails to load
- Add "Demux Attachments" and "Add Attachments to Muxer" to project options
- Add "Bitrate" to VCEEnc and QSVEnc options
- Update tools
  - AOMEnc v3.2.0-393-g402e264b9-x64-gcc11.2.0 Patman
  - ffmpeg-N v106445-g723065a346-x64-gcc11.2.0 Patman
  - MediaInfo v22.03
  - MediaInfo.NET v7.3.0.0
  - MP4Box v2.1-DEV-rev79-gdf29bc8a0-x64-gcc11.2.0 Patman
  - mpvnet v5.7.0.0
  - MKVToolNix v66.0
  - NVEnc v5.46
  - qaac v2.73
  - QSVEnc v6.10
  - Rav1e v0.5.0 (p20220322-2-gcbdf0703)-x64-gcc11.2.0 Patman
  - Subtitle Edit v3.6.5
  - SvtAv1EncApp v0.9.1-81-gdf313c62-x64-gcc11.2.0 Patman
  - VCEEnc v6.17
  - x264 v0.164.3094+13-7816202-[Mod-by-Patman]-x64-gcc11.2.0
  - x265 v3.5+37+12-4e46995bc-[Mod-by-Patman]-x64-msvc1931
- Update filters
  - TemporalDegrain2 v2.4.3
  - G41Fun v2021-09-23


v2.10.0 (2021-10-06)
====================

- New Discord Server Link: https://discord.gg/uz8pVR79Bd, if you haven't joined in yet
- Remove Extension folder from mpv.net (Dendraspis, [#770](/../../issues/770))
- ~~Replace obsolete vs.get_core() with vs.core (Dendraspis, [#787](/../../issues/787))~~
- Update tools
  - MediaInfo v21.09
  - VapourSynth R54 [back due to compatibility issues]


v2.9.0 (2021-10-05)
===================

- Fix Check for Updates (Dendraspis)
- Add --thread-affinity to NVEnc (Dendraspis)
- Add "Warn for invalid Output Mod only if video is cropped" option (Dendraspis)
- Fix --quant-b-adapt for AOMEnc (Dendraspis, [#785](/../../issues/785))
- Fix non-synced bitrate between options and main window for AOMEnc (Dendraspis, [#785](/../../issues/785))
- Update tools
  - MKVToolNix v61.0
  - NVEnc v5.40
  - Python v3.9.7
  - QSVEnc v6.03
  - Subtitle Edit v3.6.2
  - VapourSynth R56
  - VCEEnc v6.16
- Update filters
  - JincResize v2.1.0
  - ffms2 v2.40.1285+13 1c6169a StvG
  - FFT3D Neo r11
  - L-SMASH-Works v20210811


v2.8.0 (2021-08-01)
===================

- Separate checking all and required only apps on AppsForm (Dendraspis)
- Add more parameters for VCEEnc (Dendraspis, [#757](/../../issues/757))
- Add --chroma-qp-offset param for NVEnc (Dendraspis)
- Add --traditional-scenecut param for x265 (Dendraspis)
- Update tools
  - AOMEnc v3.1.2-588-gd1b830121-x64-gcc10.3.0 Patman
  - ffmpeg-N-103081-gf7958d0883-x64-gcc10.3.0 Patman
  - MP4Box v1.1.0-DEV-rev1161-g2dfbf0c7a-x64-gcc10.3.0 Patman
  - NVEnc v5.36
  - QSVEnc v5.06
  - Rav1e v0.5.0-alpha-(p20210727)-x64-gcc10.3.0 Patman
  - SVT-AV1 v0.8.7-31-g6c8c2e18-x64-gcc10.3.0 Patman
  - VCEEnc v6.13
  - x264 v0.164.3065+9-7a0e6e8-.Mod-by-Patman.-x64-gcc10.3.0
  - x265 v3.5+12+14-106329cbd-.Mod-by-Patman.-x64-gcc10.3.0
- Update filters
  - SMDegrain v3.1.2.115s by JKyle


v2.7.0 (2021-07-25)
===================

- [!!!] Changed names cause all encoder profiles to be reset. This means you have to re-set them by hand or backup and restore them after update.
- [!!!] Changed (context-)menus from earlier versions must be restored, manually adjusted or need a global settings reset in order to see and use them.
- Add Assistant option to warn if no audio in output (Dendraspis, [#709](/../../issues/709))
- Fix --vpp-pad param output for NVEnc and VCEEnc (Dendraspis, [#723](/../../issues/723))
- Fix crash using "Create job for each selection" (Dendraspis, [#727](/../../issues/727))
- Fix crash when copy error message of some exceptions (Dendraspis)
- Add and update SVT-AV1 parameters (Dendraspis, [#731](/../../issues/731))
- Add option to extract timestamps from all mkv files (Dendraspis)
- Add Assistant condition for MKV file with timestamps and changed frame rate or count (Dendraspis, [#729](/../../issues/729))
- Fix *Default* and *Forced* track settings for MP4Box (Dendraspis, [#737](/../../issues/737))
- Add setting to prefer Windows Terminal over Powershell when present (Dendraspis, [#738](/../../issues/738))
- Command Line Preview now uses the Code Preview window primarily, but is optional (Dendraspis, [#738](/../../issues/738))
- Make Main window focus after processing adjustable (Dendraspis, [#333](/../../issues/333))
- Remove 'SEI writing options'(--opts) from x264 options (deprecated) (Dendraspis, [#748](/../../issues/748))
- Add "Code Preview" to Remembered Window Positions (Dendraspis)
- Extend Crop side limits to 80% (Dendraspis)
- Use MkvExtract to extract timestamps for Command Line Demuxers (Dendraspis)
- Fix wrong TargetFile after renaming via "After Video Encoded" event (Dendraspis, [#755](/../../issues/755))
- Fix f3kdb AVS issues (Dendraspis, JKyle, 44vince44)
- Add AVS SeparateFields filter (44vince44)
- Add two missing filters to JPSDR (JKyle, [#747](/../../issues/747))
- Update RgTools URLs, description, and filter list (JKyle, [#720](/../../issues/720))
- Big filter profile update (JKyle, [#725](/../../issues/725) and more)
    - Added filters:  
    AddGrain(VS), AnimeIVTC(AVS), Bifrost(AVS/VS), Bwdif(AVS), Checkmate(AVS), ChubbyRain2(AVS), DeFlicker(AVS), DeJump(AVS), DeRainbow(AVS), DotKill(AVS/VS), ExactDeDup(AVS), FillBorders(AVS/VS), FillDrops(AVS/VS), Fix Horizontal Rainbow(AVS profile), GRunT(AVS plugin), HDRTools(AVS), Letterbox(AVS/VS), NNEDI3_rpow2(AVS/VS), ReduceFlicker(AVS), RT_Stats(AVS plugin), Santiag(AVS), Srestore(AVS plugin), TimecodeFPS(AVS), TIVTC(AVS), VFRToCFR(AVS/VS), vsCnr2(AVS)
    - Updated filter profiles:  
    AddBorders(VS), AddGrainC(AVS), Bwdif(VS), ChubbyRain2(AVS), DeRainbow(VS), DirectShowSource(AVS), DSS2(AVS), FFMS2(AVS/VS), FFVideoSource(AVS), KNLMeansCL(AVS/VS), SelectEvery(AVS/VS), SelectRangeEvery(AVS), SeparateFields(VS), TemporalDegrain2(VS), Weave(AVS/VS)
- Update tools
  - AOMEnc v3.1.2-553-g997549cf4-x64-gcc10.3.0 Patman
  - chapterEditor v1.29
  - MKVToolNix v59.0
  - MP4Box v1.1.0-DEV-rev1143-g91d397ddc-x64-gcc10.3.0 Patman
  - mpv.net v5.4.9.0
  - NVEnc v5.35
  - QSVEnc v5.05
  - Python v3.9.6
  - Rav1e v0.5.0-alpha-(p20210720-4-g0f25619a)-x64-gcc10.3.0 Patman
  - SVT-AV1 v0.8.7-30-g3af80294-x64-gcc10.3.0 Patman
  - VapourSynth R54
- Update Filters
  - AddGrain r8
  - AvsResize r9 StvG
  - Bifrost v2.1.0 (AVS) / v2.2 (VS)
  - Bwdif v1.2.1 (AVS) / r3 (VS)
  - CASm 2021-05-19
  - ChubbyRain2 2021-07-20 mod by Asd-g
  - DeFlicker v0.6
  - DeJump mod 2021-07-14 by JKyle
  - DeRainbow 2014-02-23
  - DotKill v1.0.0 (AVS) / R2 (VS)
  - ExactDedup v0.0.7Beta
  - FFT3DFilter v2.9
  - FillBorders v1.3.0 (AVS) / v2 (VS)
  - FillDrops mod 2021-07-10 by Selur (AVS) / mod 2021-07-11 by JKyle (VS)
  - flash3kyuu_deband v2.0.0-1 (Avisynth+ plugins pack r14)
  - GRunT v1.0.2
  - JPSDR v3.2.7 W7 AVX2
  - MvTools2 v2.7.45
  - Neo F3KDB r7
  - QTGMC v3.382s
  - ReduceFlicker v0.1.1
  - RgTools v1.2
  - RT_Stats v2.00Beta13
  - Srestore v2.797
  - TEMmod v0.2.2
  - TemporalDegrain2 v2.3.1
  - TimecodeFPS v1.1.1
  - VfrToCfr v1.1.1
  - vsCnr2 v1.0.0
  - VSFilterMod v5.2.5
  - Zs_RF_Shared v1.154


v2.6.0 (2021-05-22)
===================

- [ ] Might break settings from previous version, so starting with new settings is recommended
- [ ] Might break templates and jobs from previous version, so checking or renewing them is recommended
- [X] Changed (context-)menus from earlier versions must be restored, manually adjusted or need a global settings reset in order to see and use them
- Enable Timestamp Extraction for VFR MKV files by default (Dendraspis, 44vince44)
- Fix timestamp extraction setting being ignored (Dendraspis, 44vince44)
- Autoload (extracted) timestamps file (Dendraspis)
- Fix x264 --progress-header param not being shown correctly (Dendraspis, [#696](/../../issues/696))
- Add Drag'n'Drop to Video Comparison (Dendraspis, [#697](/../../issues/697))
- Video Comparison loads source file directly and also the target file, if it exists (Dendraspis)
- Add Jobs window to remembered window positions (Dendraspis)
- Add x264 --synth-lib param for non-vanilla builds only (Dendraspis, 44vince44, [#711](/../../issues/711))
- Close all Preview windows when adding a job (Dendraspis, 44vince44)
- Edit Cutting filter instead of re-creating it (Dendraspis, [#710](/../../issues/710))
- Add multiple params for VCEEnc (Dendraspis, [#551](/../../issues/551))
- Add --input-analyze param for NVEnc/VCEEnc (Dendraspis)
- Refurbish Next button (Dendraspis)
- Add settings to change modifier key behavior on Next/AddJob button (Dendraspis)
- Disable 'Err' error messages by default (Dendraspis)
- Fix MCTemporalDenoise high settings cause error (JKyle, [#690](/../../issues/690))
- Update various filter profiles for both AVS and VS (JKyle)
- Add Grayscale filter profiles for AviSynth/VapourSynth (JKyle)
- AOMEnc v3.1.0-243-g87682566c-x64-gcc10.3.0 Patman
- AVSMeter v3.0.9.0
- AVSTP v1.0.4
- CASm 2021-04-22 by JKyle
- DAA3Mod v3.63
- Dither v1.28.1
- ffmpeg N v102464-gc6ae560a18-x64-gcc10.3.0 Patman
- FineSharp mod v2020.04.12 HBD
- FrameRateConverter v1.3
- HQDering mod v1.8
- LimitedSharpenFaster v2.193
- Maa2mod v0.435
- mClean v3.2 Jmod 4 (2021-05-14)
- Mp4Box v1.1.0-DEV-rev904-g00a2e202a-x64-gcc10.3.0 Patman
- NVEnc v5.32
- Qaac-2.72-x64-msvc1928 Patman
- QSVEnc v5.02
- Rav1e v0.5.0-alpha-(p20210518-3-ge1926975)-x64-gcc10.3.0 Patman
- ResizeX v1.0.1 mod 9.39
- Subtitle Edit v3.6.1
- SVT-AV1 v0.8.7-4-g786c4dac-x64-gcc10.3.0 Patman
- TTempSmooth v0.9.4
- VCEEnc v6.11
- vsTTempSmooth v1.1.3
- x264 v0.163.3059+11-1aa8b82-.Mod-by-Patman.-x64-gcc10.3.0


v2.5.0 (2021-05-03)
===================

- [ ] Might break settings from previous version, so starting with new settings is recommended
- [ ] Might break templates and jobs from previous version, so checking or renewing them is recommended
- [x] Changed (context-)menus from earlier versions must be restored, manually adjusted or need a global settings reset in order to see and use them
- Add option to disable extraction of forced subtitles for IDX files (Dendraspis, 44vince44)
- Sort video files before they are opened and processed (Dendraspis)
- Fix app crash on opening menu between encodes (Dendraspis)
- Disable file drop in Code Editor (Dendraspis)
- Fix multiple suspensions on running processes (Dendraspis)
- Fix and enhance Thumbnailer (Dendraspis)
- Fix Autocrop showing unrounded progress percentage (Dendraspis, [#659](/../../issues/659))
- Set subtitle *forced* value if file name contains "forced" (Dendraspis)
- Add *After Job Failed* event (Dendraspis)
- Muxer and AOMEnc adjustments to handle all AOMEnc output extensions (Dendraspis, [#669](/../../issues/669))
- Fix *Create job for each selection* returning a wrong project (Dendraspis, [#676](/../../issues/676))
- Reload last project after processing jobs (Dendraspis, [#645](/../../issues/645))
- Fix not themed Menu Template Window (Dendraspis, [#681](/../../issues/681))
- Change shortcuts for Processing and Jobs windows (Dendraspis)
- Set initial shutdown timeout of 90s (44vince44)
- Add Sharpen filter profile for AviSynth/VapourSynth (JKyle)
- Add CAS filter profile for AviSynth/VapourSynth (JKyle)
- Add MDegrain3 filter profile for AviSynth (JKyle)
- Update InterFrame and YFRC filter profiles (44vince44)
- Update MDegrain2 filter profile (JKyle)
- Add MVTools 2 missing filters (JKyle)
- AOMEnc v3.0.0-375-g4d1ace0ad-x64-gcc10.2.0 Patman
- AutoCrop v2.2
- chapterEditor v1.28
- MKVToolNix v56.1.0
- Mp4Box v1.1.0-DEV-rev802-gc0ea96c7b-x64-gcc10.2.0 Patman
- NVEnc v5.30
- Python v3.9.4
- qaac v2.72
- rav1e v0.5.0-alpha-(p20210427-1-gf54b23b6)-x64-gcc10.2.0 Patman
- SVT-AV1 v0.8.6-93-g7cb05bf7 Patman
- VapourSynth R53
- VCEEnc v6.10
- x265 v3.5+10+13-7c9bc0cb14 [Mod-by-Patman]
- AnimeIVTC v2.381 2021-03-30 mod
- CAS v1.0.1
- D2VSource v1.2.3
- edi_rpow2 v1.0 mod 86
- FineDehalo v1.1 mod8.79
- GradFun2DBmod v1.5 2020.06.27 mod
- HQDN3D v1.0.1
- JPSDR v3.2.6 (W7 AVX2)
- L-SMASH-Works 20210423 HolyWu
- MCTemporalDenoise v1.4.21
- QTGMC v3.379, by Vit, 2012, 2021 mod by A.SONY
- xy-VSFilter v3.2.0.806 pfmod
- Zs_RF_Shared v1.151


v2.4.0 (2021-04-09)
===================

- [ ] Might break settings from previous version, so starting with new settings is recommended
- [X] Might break templates and jobs from previous version, so checking or renewing them is recommended
- [X] Changed menus from earlier versions must be restored, manually adjusted or need a global settings reset in order to see and use them
- Fix ffmpeg re-mux remuxes only one stream per stream type (Dendraspis, JKyle)
- Fix old StaxRip Thumbnailer (Dendraspis, [#596](/../../issues/596))
- Refurbish StaxRip Thumbnailer (Dendraspis, [#596](/../../issues/596))
- Fix adding hardcoded subtitle not working properly (Dendraspis, [#643](/../../issues/643))
- Make 'Err' error messages optional via settings (Dendraspis)
- Remove '/vfw.h' from 'Err' (stax76)
- Bring Command Line options from *Settings* sub-menu back to top level (Dendraspis)
- Improve keyboard support in apps dialog (stax76)
- Remove NVEnc encode modes cbrhq and vbrhq (Dendraspis, [#652](/../../issues/652))
- Add Auto Film Threshold for D2V files to project options (Dendraspis, [#641](/../../issues/641))
- Optimize, extend and add new AviSynth filters (44vince44)
- Typo and consistency fixes (JKyle)
- Fix AssumeBFF filter selection (JKyle)
- Removal of MTN Thumbnailer (~60MB unzipped, ~10MB zipped) (Dendraspis)
- CTMF r5
- edi_rpow2 v1.0 mod 84
- finesharp 2020-11-03
- fmtconv r22
- fvsfunc 2020-10-11
- muvsfunc v0.3.0
- mvtools v23
- MediaInfo.NET v7.1.0.0
- MKVToolNix v56.0.0
- nnedi3_resize16 v3.3


v2.3.0 (2021-03-28)
===================

- [ ] Might break settings from previous version, so starting with new settings is recommended
- [ ] Might break templates and jobs from previous version, so checking or renewing them is recommended
- [X] Changed menus from earlier versions must be restored, manually adjusted or need a global settings reset in order to see and use them
- Demux dialog attachments checkbox render bug fixed (stax76)
- Demux dialog has buttons to enable/disable all attachment demuxing (stax76)
- Fix possible crash when converting subtitles (stax76)
- Disable *eac3to* preprocessing by default and extend re-muxing with *ffmpeg* to m2ts files (Dendraspis, stax76, JKyle, 44vince44, [#632](/../../issues/632))
- Fix crash when extracting forced subtitles (Dendraspis, [#634](/../../issues/634))
- Rearrange settings sections (Dendraspis)
- Fix ffmpeg not being able to re-mux files with menus (Dendraspis)
- Fix ffmpeg demux issues (Dendraspis)
- Add missing parameters to ffmpeg calls (Dendraspis, JKyle, [#636](/../../issues/636))
- aomenc 3.0.0-205-g0a5da45c7-x64-gcc10.2.0 Patman
- ffmpeg N 101743-gcad3a5d715-x64-gcc10.2.0 Patman
- Mp4Box 1.1.0 DEV rev635 g9c51f2274-x64-gcc10.2.0 Patman
- rav1e 0.5.0 alpha (p20210323-5-ge9efcf35)-x64-gcc10.2.0 Patman
- SvtAv1EncApp v0.8.6-76-g44486d233-x64-gcc10.2.0 Patman
- JincResize v2.0.2
- JPSDR 3.2.4 (Clang W7 AVX version)
- yadifmod2 v0.2.7
- VSFilterMod r5.2.4


v2.2.0 (2021-03-26)
===================

- [X] Might break settings from previous version, so starting with new settings is recommended
- [X] Might break templates and jobs from previous version, so checking or renewing them is recommended
- [X] Changed menus from earlier versions must be restored, manually adjusted or need a global settings reset in order to see and use them
- Start of new versioning (no beta versions anymore, but stripped DEV versions)
- Check for updates updated to support new versioning (Dendraspis)
- Fix re-calculation of video bitrate on multi-pass encode and audio encodes (Dendraspis, [#580](/../../issues/580))
- New projects use Copy/Mux as audio profile (Dendraspis)
- Fix misleading audio stream delay detection (Dendraspis)
- Demuxing of video and chapters is set via Options window instead of Preprocessing (Dendraspis)
- Add option to demux subtitles without including them (Dendraspis, [#622](/../../issues/622))
- Make Checkboxes grow and shrink with UIScaleFactor (Dendraspis)
- Fix menu button graphics issue (Dendraspis, [#623](/../../issues/623))
- New *Check for updates* dialog (Dendraspis)
- Extended Settings Directory Location Selection at first run from new folder (Dendraspis)
- Fix crash on app version editing (Dendraspis, [#628](/../../issues/628))
- MTN Thumbnailer description adjusted (JJKylee)
- Fixed 'Reset Setting' feature in the Advanced menu (stax76)
- New task dialog with color theme support (stax76)
- Clicking an option in the video encoder command line preview navigates directly to the UI control (stax76, [#617](/../../issues/617))
- New font picking task dialog to choose the console font (stax76)
- DetailSharpen VapourSynth filter profile added (JJKylee, [#624](/../../issues/624))
- JPSDR 3.2.5 (Clang W7 AVX version)
- SangNom2 v0.6.0
- x265 3.5+9+14-6c69ed37d [Mod by Patman]


2.1.9.0 Beta (2021-03-21)
=========================

- [X] Might break settings from previous version, so starting with new settings is recommended
- [X] Might break templates and jobs from previous version
- [X] Changed menus that need a menu reset, manual setting or global settings reset in order to see them
- Since v2.1.8.5 StaxRip uses the SI prefix for sizes (Base 10: `1 MB = 1000 KB`)  
  Now it is possible to switch (back) to IEC prefix (Base 2: `1 MiB = 1024 KiB`) in the settings  
  *Please note, that the target size menu must be resetted or adjusted by hand after a change!* (Dendraspis, 44vince44)
- Set "Dark | Blue" as default theme (Dendraspis)
- CheckBox appearance optimized (Dendraspis)
- CodeEditor does not join "Cutting" filter (Dendraspis)
- Block size for Compressibility Check can be set in seconds (Dendraspis, [#547](/../../issues/547))
- The x264 command line preview shows all passes (Dendraspis, [#496](/../../issues/496))
- The aomenc command line preview shows all passes (Dendraspis, [#496](/../../issues/496))
- Extended option descrption for 'Temp Files Folder' if Auto-Deletion is enabled (Dendraspis, [#542](/../../issues/542))
- Progress Reformatting is a global option like Output Highlighting (Dendraspis)
- Improved usability for subtitle options (stax76, [#577](/../../issues/577))
- Show target size label including size unit (Dendraspis, VEGAX265, [#580](/../../issues/580))
- Theme "Default" is renamed to "System Colors" (Dendraspis)
- Priority for current selected process can be set via menu on Processing window (Dendraspis)
- Multiple theme related adjustments (Dendraspis, stax76)
- New shortcuts for ***O**utput Highlighting* and ***P**rogress Reformatting* on Processing window (Dendraspis)
- New shortcuts for existing actions on Jobs window (stax76)
- Fix x265 --tskip default value for Placebo preset (tkozybski, Dendraspis, [#600](/../../issues/600))
- Fix x265 Slower preset default values (tkozybski)
- Fix x265 params --atc-sei and --pic-struct wrong parameter type (Dendraspis, [#593](/../../issues/593))
- x264 default CRF value set while setting initial value to 20 (Dendraspis, [#594](/../../issues/594))
- x264 fix UltraFast preset value sync (Dendraspis, [#594](/../../issues/594))
- Fix NVEnc VBR Quality value localization (Dendraspis, [#604](/../../issues/604))
- Fix NVEnc VBR Quality applies even it's disabled (Dendraspis, [#605](/../../issues/605))
- Removal of Menu Styles (Dendraspis, [#589](/../../issues/589))
- In quality mode main dialog shows count and type of active subtitles (stax76)
- Main dialog and code editor show script info with F2 key and
  advanced script info with Ctrl+F2, requires menus to be reset manually (stax76)
- New setting to customize the code and console font (stax76)
- Video encoder context console help was fixed and improved (stax76, [#595](/../../issues/595))
- Fix audio bitrate calculation for calculation of video bitrate (Dendraspis, 44vince44, [#608](/../../issues/608))
- When toggling size prefix target size/bitrate are refreshed (Dendraspis)
- AddGrainC v1.8.3.0
- AnimeIVTC v2.34 2020-12-19 mod
- AVSMeter v3.0.8.0
- chapterEditor v1.27
- CropResize 2021-02-01
- MediaInfo.NET v7.0.0.0
- MP4Box v1.1.0 DEV rev589 gcc10.2.0 Patman
- MTN Thumbnailer v3.4
- SangNom2 v0.6.0
- xy-VSFilter v3.2.0.804


2.1.8.5 Beta (2021-03-09)
=========================

- When a video encoder default value was changed, a compatibility issue
  with old settings was happening, this is finally fixed, but could only
  be achieved by resetting all video encoder options (stax76, [#546](/../../issues/546))
- Fix issue with ffmpeg AAC encoding (stax76, [#548](/../../issues/548))
- Encoder dialogs allow using 1:1 for --sar (stax76, [#546](/../../issues/546))
- Introduce Output Highlighting in processing dialog (Dendraspis)
- Introduce Command Line Highlighting in command line preview (Dendraspis)
- Add more theme colors (Dendraspis)
- Fix Menu Item Path not being stored (Dendraspis)
- Package Name/Version is not displayed when unknown versions are accepted (Dendraspis)
- Customized progress text adjusted for new x264 Patman builds (Dendraspis)
- x265 --selective-sao default set to 0 (Dendraspis, [#546](/../../issues/546))
- x265 --qpmin, --qpmax and --qpstep boundaries set (Dendraspis)
- x265 ---psy-rd moved from Other to Analysis (Dendraspis)
- x264 --progress-header disabled for Patman build (Dendraspis)
- Customized progress text fix for new Patman build (Dendraspis)
- Fix resize values are not refreshed after changing them (Dendraspis, [#558](/../../issues/558))
- Adjustments to match the SI prefix (Base 10) for sizes instead of IEC prefixes (Base 2) (Dendraspis)
- Set minimum bitrate of 32kbps for (E)AC3 (Dendraspis, 44vince44)
- Fix ffmpeg DD+ encoding issue with high bitrates by adding 0.1 to bitrate (Dendraspis, [#566](/../../issues/566))
- Add vertical scrollbars to Code Editor code-boxes (Dendraspis, [#567](/../../issues/567))
- Code Editor does not join Source, Crop, Resize and Rotation filters automatically (Dendraspis)
- Code Editor can join (in-)active filters only [Don't forget to reset "Edit Menu" in Code Editor!] (Dendraspis)  
- Code Editor joined filters have the filter name "Misc" (Dendraspis)
- Custom checkboxes for better theme experience (Dendraspis)
- Readme.md refreshed (Dendraspis, stax76)
- Error exit codes are interpreted using Err.exe (stax76)
- arib-srd-b67 changed to arib-std-b67 in NVEnc, QSVEnc, and VCEEnc (JKyle)
- Detect forced subtitles by filename _forced and .forced. (case-insensitive) (stax76)
- The crop dialog allows to define the color used for cropping via menu (stax76)
- StaxRip passes theme colors to MediaInfo.NET via CLI (stax76)
- SMDegrain 3.1.2.111s
- mkvtoolnix 55 (mkvextract Unicode fix)
- MP4Box 1.1.0 DEV rev542 gcc10.2.0 Patman (AAC 7.1 fix)
- rav1e 0.5.0-alpha gcc10.2.0 Patman
- DGMPGDec 2.0.0.5 (large file handling fix)
- aomenc 2.0.2-1350-msvc1928 Patman
- x265 3.5_RC1+2+29-gcc10.2.0 Patman Mod
- x264 0.161.3048+25-gcc10.2.0 Patman Mod
- ffmpeg N-101392-gcc10.2.0 Patman
- MediaInfo.NET 6.9.0.0 (gets theme colors from StaxRip via CLI)
- Err 10.0.17763.1


2.1.8.4 Beta (2021-02-25)
=========================

- Customized progress text support newest x265 3.5 RC1 build by Patman (Dendraspis)
- QTGMC 3.377
- Zs_RF_Shared 1.143
- TIVTC 1.0.26
- MedianBlur2 1.1


2.1.8.3 Beta (2021-02-24)
=========================

- Introduction of Themes (Dendraspis, testing and appreciated feedback by 44vince44 and JJKylee, [#510](/../../issues/510) & [#518](/../../issues/518))
- NVEnc UI options move into new Input/Output section (stax76)
- AVS SMDegrain filter profiles refurbished (44vince44)
- x265 supports --reader-options library for Patman builds (Dendraspis)
- If the startup folder contained Turkish i character,
  QTGMC and any other avsi file failed to load (stax76)
- x264 10bit, --synth-lib, VapourSynth reader for DJATOM and Patman Mod
- Docs moved to GitHub Wiki, contributions are welcome! (stax76)
- x264 0.161.3048+17 gcc10.2 Patman Mod (improved progress display)
- x265 3.5 RC1+2+23  gcc10.2 Patman Mod (improved progress display)
- xy-VSFilter 3.2.0.802 (10bit support)
- Get-MediaInfo 3.6


2.1.8.2 Beta (2021-02-21)
=========================

- New Preview dialog feature 'Create job for each selection' (stax76, [#512](/../../issues/512))
- Fix crash in Preview dialog using 2 instances and shortcut keys (stax76, [#515](/../../issues/515))
- NVEnc options have a bitrate option (Dendraspis, [#520](/../../issues/520))
- Blocking assistant warnings are shown in red with Next button disabled (stax76)
- On systems prior Windows 10 1903 using ANSI characters, avs2pipemod is now used
  automatically because DJATOM/Asuna/Patman x265 builds have no working ANSI fallback (stax76)
- All important tools support AviSynth Unicode and Long Path on Windows 10 1903
- vspipe patched with Windows 10 Long Path manifest
- NVEnc 5.29, QSVEnc 4.13, VCEEnc 6.09 (Windows 10 AviSynth Unicode and Long Path)
- ffmpeg N-101189 gcc10.2.0 Patman (Windows 10 AviSynth Unicode and Long Path)
- MP4Box 1.1.0 rev506 gcc10.2 Patman
- Subtitle Edit 3.6.0


2.1.8.1 Beta (2021-02-13)
=========================

- aomenc options rearranged (Dendraspis)
- aomenc supports chunk encoding (Dendraspis, [#368](/../../issues/368))
- aomenc gets decoder and pipe settings (Dendraspis, [#497](/../../issues/497))
- aomenc default value for --kf-max-dist changed to 120 (Dendraspis, 44vince44, [#506](/../../issues/506))
- aomenc default value for --kf-min-dist changed to 12 (Dendraspis, 44vince44, [#506](/../../issues/506))
- aomenc sets --verbose by default (Dendraspis, 44vince44, [#506](/../../issues/506))
- Fix frame rates for AviSynth functions (44vince44)
- Replace Rapair16 with Dither_Repair16 for AVS (44vince44)
- Fix AvsResize website address and VS ConvertFormat command (JJKylee)
- aomenc 2-pass stats file extension change to '.fpf' (Dendraspis)
- Fix aomenc --webm parameter (Dendraspis)
- Force mkvmerge to write fps in case of aomenc not using IVF container (Dendraspis)
- Fix shortcut editor key default causing an issue when m is typed (stax76, [#505](/../../issues/505))
- x265 UI option for '--progress-readframes' on DJATOM builds (Dendraspis)
- nvenc 5.28 (Windows 10 long path and AviSynth Unicode support)


2.1.8.0 Stable (2021-02-09)
===========================

- Audio source file bitrate detection fix (stax76)
- New VideoFilter "ChangeFPS" for AviSynth (Dendraspis, 44vince44, [#499](/../../issues/499))
- New VideoFilter "Rotation" (Dendraspis)
- Auto-Rotation feature for supported source files/container (Dendraspis, [#390](/../../issues/390))
- BAT and PS1 scripts to install and uninstall AviSynth and VapourSynth
  portable system wide are located at Apps\FrameServer\Install (stax76)
- New feature to automatically save projects (stax76)
- New feature to automatically fix bad frame rates (stax76)


2.1.7.9 Beta (2021-02-08)
=========================

- x265 --version output is used in order to detect if it's aMod, Asuna or Vanilla (stax76)
- Menu in the code editor can be customized and is much faster (stax76, [#494](/../../issues/494))


2.1.7.8 Beta (2021-02-06)
=========================

- Support 10-bit avs input for x264 in case the x264 version name contains the keyword aMod (stax76)
- AviSynth portable mode no longer uses soft links but moves files if necessary (stax76, [#473](/../../issues/473))
- For x264 and x265 different code paths are used depending if the version name contains
  the keywords aMod or Asuna, Vanilla builds are supported as well (stax76)


2.1.7.7 Beta (2021-02-05)
=========================

- VPY files failed to import (stax76, [#486](/../../issues/486))
- Warning for non spec compliant AC3 bitrate moved to main dialog
  so it's possible to ignore the warning (stax76, JKyle)
- avs2pipemod mod with Unicode and Long Path support (stax76)
- .aac files demuxed with mkvextract are no longer converted to .m4a (stax76, [#489](/../../issues/489))
- x264 --fade-compensate --log-file --log-file-level --opts --progress-header (stax76)
- mkvtoolnix 53


2.1.7.6 Beta (2021-02-03)
=========================

- Apps dialog allows custom paths to AviSynth and VapourSynth portable (stax76)
- Installed AviSynth don't has to be located in System32, it can be installed
  anywhere using the included install.ps1 powershell script (stax76)
- NVEnc parameter fixes and option re-organisation (Dendraspis, [#468](/../../issues/468))
- NVEnc gets '--vpp-warpsharp' options and a Sharpness subsection (Dendraspis)
- x265 two parameters not set when checkbox unchecked (Dendraspis)
- Optional customized/shorter progress text while encoding for x264/x265 (Dendraspis)
- Fix Re-mux TS to MKV via ffmpeg doesn't demux AAC tracks with ADTS (Dendraspis, [#483](/../../issues/483))
- x264 portable mode uses DJATOM Mod --synth-lib (stax76)
- x265 portable mode uses DJATOM Mod --reader-options library (stax76)
- Command line preview uses scrollbar in case > 9 lines (stax76, [#481](/../../issues/481))
- x265 3.4+70-aMod-gcc10.2.1 DJATOM Mod, avs portable support,
  vs async frame requests logic overhaul ([#470](/../../issues/470))
- FFT3dGPU 0.8.6
- x264 aMod-core161-r3039+17 DJATOM (--synth-lib)
- nvenc 5.26


2.1.7.5 Beta (2021-01-23)
=========================

- x265 with AviSynth input incorrect command line (stax76, [#475](/../../issues/475))


2.1.7.4 Beta (2021-01-23)
=========================

- Make forced closing optional via settings (Dendraspis)
- NVEnc gets '--vpp-colorspace' options (Dendraspis, [#461](/../../issues/461))
- Messagebox when processed file or its project file is not found instead of crash (Dendraspis, [#460](/../../issues/460))
- x265 vpy input sets --reader-options library=path-to-vsscript.dll (stax76, [#471](/../../issues/471))
- x265 UI issue fix (stax76, [#472](/../../issues/472))
- vceenc uses --avsdll in portable mode (stax76, [#473](/../../issues/473))
- D2VSource 1.2.2
- x265 3.4+62-aMod-gcc10.2.1-hdr10-info msg7086/DJATOM
- avsresize r5


2.1.7.3 Beta (2021-01-18)
=========================

- Full AviSynth unicode and full long path support
  for users of Windows 10 1903, Windows 7 users need to enable
  avs2pipemod for x265 in order to use foreign (ANSI) characters
- Show also Source HDR Format on main window (Dendraspis)
- 3.4+62-aMod-gcc10.2.1+opt msg7086/DJATOM (avs unicode/long path/chunk encoding)


2.1.7.2 Beta (2021-01-17)
=========================

- Using neo functions caused non neo plugins being loaded without reason (stax76, [#250](/../../issues/250))
- Fix weird behaviour on Audio Settings for qaac (Dendraspis)
- x265 fixed default value for --hist-threshold (Dendraspis, [#441](/../../issues/441))
- x265 muxing won't mux chunks from recent encodes (Dendraspis, [#438](/../../issues/438))
- x265 don't use a pipline for chunks if no pipeline is set (Dendraspis, [#430](/../../issues/430))
- x265 --seek and --frames also work with chunks (Dendraspis, [#430](/../../issues/430))
- x265 Pipe option is invisible if decoder is not set to AVS/VPY (Dendraspis)
- In the portable AviSynth folder there are two PowerShell scripts
  to install and uninstall AviSynth (stax76)
- Using ffmpeg as pipe tool with VapourSynth and x264/x265 (Patman86)
- QT libraries will be found if located at Apps\Audio\qaac, Apps\Audio\qaac\QTfiles64,
  Download button points to wiki page with install instructions (stax76)
- Add 'ShowCodePreview' to commands list (Dendraspis, [#444](/../../issues/444))
- x265 options set chromaloc automatically to 2 if HDR is set to "Yes",
  to 0 if HDR is disabled or keep it untouched if "Undefined" (Dendraspis, [#319](/../../issues/319))
- "Import VUI metadata" also imports Chromaloc (Dendraspis, [#319](/../../issues/319))
- "Import VUI metadata" sets "--hdr10" (Dendraspis)
- Moved '--high-tier' to 'Basic' page for x265 (Dendraspis)
- Introduce custom quality definitions for x264 and x265 via Settings > Video (Dendraspis)
- The x265 command line preview shows both passes for two pass (stax76, [#454](/../../issues/454))
- MP4Box can import EAC3 (stax76, [#455](/../../issues/455))
- Processing form remembers its size (stax76)
- Speed control in rav1e dialog has hint text: 0 = Slowest, 10 = Fastest (stax76)
- nvenc option --timecode (stax76)
- x265 UI bug in preset VerySlow showing unnecessarily default values in command line (Dendraspis, stax76)
- AviSynth 3.7
- x265 3.4+56 aMod gcc10.2.1 msg7086/DJATOM (VapourSynth reader fixed)
- MP4Box 1.1.0-rev447-g8c190b551-gcc10.2.0 Patman (Windows 10 long path support added)
- nvenc 5.25
- rav1e 0.4.0
- DgMpgDec 2.0.0.2


2.1.7.1 Beta (2021-01-11)
=========================

- Source information shows the video stream size instead of file size,
  if not available the file size is shown in brackets (Dendraspis)
- Neo DFTTest is now also available for AviSynth. (stax76, [#426](/../../issues/426))
- Main dialog shows actual video width and height. (stax76)
- Advanced Info feature of Code Editor has new VapourSynth ClipInfo option. (stax76)
- Fix items can't be removed from or rearranged in lists (Dendraspis, [#433](/../../issues/433))
- Fix randomly appearing IOException when using x265 with chunks (Dendraspis, [#431](/../../issues/431))
- Revert current folder of process being set to video temp folder because
  it locks the folder and also because the .NET Framework allows only
  260 characters for the working directory of a process. (stax76, [#431](/../../issues/431))
- nero aac encoder binaries removed. (stax76)
- On systems with UTF-8 code page invalid d2v and idx files were created. (stax76, [#435](/../../issues/435))
- Get-MediaInfo shows Range, Primaries, Transfer, Matrix. (stax76)
- Fix wrong aspect ratio calculation in batch mode. (stax76, [#428](/../../issues/428))
- Options dialog starts faster. (stax76)
- WorkingDirectory property of ExecuteCommandLine command supports
  macro expansion, Windows Terminal menu item uses the video temp
  folder as current directory. (stax76, JJKylee [#436](/../../issues/436))


2.1.7.0 Stable (2021-01-07)
===========================

- Fixed crash using command line based audio profile. (stax76, [#423](/../../issues/423))


2.1.6.1 Beta (2021-01-07)
=========================

- Fix CLI usage causing version verification to fail. (stax76)
- RgTools 1.1
- Neo FFT3D r10


2.1.6.0 Stable (2021-01-06)
===========================

- aomenc bugfixes and UI improvements (Dendraspis, 44vince44, [#375](/../../issues/375))
- aomenc moved cq-level option to "Rate Control 1" for better usage (Dendraspis, Neltulz, [#411](/../../issues/411))
- aomenc internal improvements and updates (Dendraspis)
- aomenc 10-bit input fixed. (stax76, [#420](/../../issues/420))
- x265 --aq-mode option captions extended (stax76)
- x265 by default no longer uses a pipe tool because we use
  a modified x265 build now that supports both AviSynth and VapourSynth
  directly, people who want to replace it with an ordinary x265 build
  need to enable piping in the x265 options: Input/Output > Pipe
- New option using the source file folder as temp file folder. (stax76)
- Fix issue detecting default subtitle. (stax76, [#362](/../../issues/362))
- Fix demuxed mkv subtitles not named starting at ID 1. (stax76)
- Fix eac3to demuxed tracks not named starting at ID 1. (stax76)
- Preferred subtitles can be defined by ID. (stax76)
- Preferred audio tracks to demux can be defined by ID. (stax76)
- Preferred audio and subtitle tracks in Options can be defined per menu. (stax76)
- mkvtoolnix 52


2.1.5.5 Beta (2020-12-30)
=========================

- Multiple AviSynth.dlls were included due to bug in release script. (stax76, 44vince44 [#407](/../../issues/407))
- Apple QuickTime library removed.
- The apps dialog has a feature to copy the path to the clipboard. (stax76)
- The apps dialog has a feature to edit the user PATH environment variable. (stax76 [#305](/../../issues/305))
- VapourSynth CAS plugin added (LSFmod dependency).
- On Windows 10 Unicode for AviSynth is supported even when the default
  code page is not UTF-8. This currently works for staxrip, mpv.net and x264.
- aomenc bitrate UI fix (Dendraspis, 44vince44, [#375](/../../issues/375))
- mpv.net 5.4.8.6
- x264 M-0.161.3027-4121277-x64-gcc10.2.0 Patman
- x265 M-3.4+28 gcc10.2.0 Yuuki-Asuna/msg7086/DJATOM/Patman,
  it supports direct AviSynth and VapourSynth input which can be
  enabled in the encoder settings: Input/Output > Pipe > None


2.1.5.4 Beta (2020-12-27)
=========================

- Re-include "check also for new beta version" option in settings (Dendraspis)
- Remove links from changelog when checking for new versions (Dendraspis)
- Fix issues using multiple preview dialogs. (stax76, [#395](/../../issues/395), [#401](/../../issues/401))
- New macros %video_encoder_settings% and %app_version:name% (stax76, [#367](/../../issues/367))
- New command `AddTags` for adding mkv tags. (stax76, [#367](/../../issues/367))
- aomenc default params updated and some fixes (Dendraspis, 44vince44, [#375](/../../issues/375))
- A tool can now have multiple download URLs. (stax76, [#400](/../../issues/400))
- Apps dialog links to a new [x265 wiki page](https://github.com/staxrip/staxrip/wiki/x265). (stax76, [#400](/../../issues/400))
- aomenc 2.0.1-1118-gbe4ee75fd-x64-msvc1929 Patman
- ffmpeg N-100448-gab6a56773f-x64-gcc10.2.0 Patman
- mp4box 1.1.0-DEV-rev384-gf9e004333-x64-gcc10.2.0 Patman


2.1.5.3 Beta (2020-12-19)
=========================

- Stop saving empty log files (Dendraspis)
- aomenc options reorganized, restructured and refreshed (Dendraspis, Neltulz, [#375](/../../issues/375))
- Removed 'HDRCore-ToneMapping' because of use of DGTonemap (Dendraspis, JJKylee, 44vince44, [#369](/../../issues/369))
- aomenc options optimized (Dendraspis, Neltulz, BlueSwordM , [#361](/../../issues/361))
- aomenc dropdown controls have a properly expanded width. (stax76)
- Fix rightclick on aomenc options doesn't show help (Dendraspis, [#365](/../../issues/365))
- Expand 'Restore Profiles' to preserve possible custom profiles (Dendraspis)
- Fix bug in ListBoxEx with selection after removing items (Dendraspis)
- Command ExecutePowerShellScript expands macros. (stax76, [#308](/../../issues/308))
- Fix StaxRip not finding the (installed) VapourSynth (per user) plugin directory, this could
  lead to an error StaxRip trying to manually load plugins that were already auto loaded. (stax76, [#399](/../../issues/399))
- Cut settings when created from the code editor preview were not applied. (stax76, [#395](/../../issues/395))
- L-SMASH-Works 2020-12-16 StvG
- AviSynth 3.6.2 test6
- 7zip 19.0
- aomenc 2.0.0-1117-g7ddc21b28


2.1.5.2 Beta (2020-12-15)
=========================

- StaxRip authors in about page updated. (stax76)
- Fix progress bar not working for rav1e encoder. (stax76)
- Tool updates (stax76, hevron)
- Fix tool auto update crash in case of missing download URL. (stax76)
- AVSMeter 3.0.7.0
- D2VSource 1.2.0
- KNLMeansCL 1.1.1e
- masktools2 2.2.26
- mvtools2 2.7.44
- neo-minideen r10
- TDeint 1.8
- TIVTC 1.0.25
- VSFilterMod 5.2.3
- DFTTest r7
- L-Smash-Works 2020-07-28 HolyWu
- havsfunc r33
- x264 M-0.161.3018-db0d417-x64-gcc10.2.0
- ffmpeg N-99558-g00772ef4f7-x64-gcc10.2.0 Patman
- JPSDR 3.2.4
- rav1e 2020-12-08
- Subtitle-Edit 3.5.18
- aomenc 2.0.0-918-g75a47cc18-gcc10.2.0


2.1.5.1 Beta (2020-12-15)
=========================

- Job processing issue fix (stax76, 44vince44, [#381](/../../issues/381))
- Audio filename collision fix (stax76, Dendraspis, 44vince44, [#386](/../../issues/386), [#380](/../../issues/380), [#297](/../../issues/297))
- The apps dialog has a new auto update feature,
  works for many but not for all tools. (stax76)
- nvenc new options added (stax76)
- qsvenc new options added and reorganized (stax76)
- Removed '--analysis-reuse-level' from x265 options, because deprecated (Dendraspis)
- Fixed x265 wrong output params in 3-pass mode (Dendraspis, 44vince44, [#391](/../../issues/391))
- vceenc 6.07
- qsvenc 4.12
- ffms2 87bae19 2020-11-23 StvG
- L-Smash-Source 2020-12-11 StvG


2.1.4.9 Beta (2020-12-13)
=========================

- Set the current directory of the StaxRip process to the working.
  directory of the current project. (stax76, 44vince44, [#388](/../../issues/388))
- Video encoders expand macros in custom options. (stax76, 44vince44, [#388](/../../issues/388))
- Fix audio encoding overwriting original file. (stax76, 44vince44, [#380](/../../issues/380))
- x265 --min-vbv-fullness --max-vbv-fullness --vbv-live-multi-pass (stax76)
- ~~x265 multipass order refixed (Dendraspis, [#389](/../../issues/389))~~
  Added comments in source code to prevent further confusions (Dendraspis, 44vince44)
- x265 'Custom Nth Pass' added to options (Dendraspis, [#389](/../../issues/389))
- x265 options order slightly changed (Dendraspis)
- x265 'Analysis Save' has also a 'Browse File' button (Dendraspis)
- Fixed x265 multipass order for chunks (Dendraspis)
- Renamed x264/x265 passes for (n>2)-pass-mode (Dendraspis, 44vince44, [#389](/../../issues/389#issuecomment-743997077))
- x265 3.4+53-ge4afbd100 2020-11-02 Yuuki/qyot27
- nvenc 5.22
- chapterEditor 1.25
- SVT-AV1 0.8.6
- MediaInfo.NET 6.8
- MediaInfo 20.09
- Get-MediaInfo 3.5
- mpv.net 5.4.8.5


2.1.4.8 Beta (2020-10-07)
============

- The Apps dialog supports drag & drop for zip/7z files (stax76)
- Auto update supports mkvtoolnix (stax76)
- aomenc fixes and improvements (stax76, [#357](/../../issues/357))
- GUI options for ffmpeg -probesize -analyzeduration (stax76, [#354](/../../issues/354))
- qaac 2.71 (long path support)
- chapterEditor 1.24
- mkvtoolnix 51


2.1.4.7 Beta (2020-10-02)
============

- Fix Audio decoding method *Pipe* causes crash (Dendraspis, #340)
- Fix parameters when audio converting to AAC with FFmpeg (Dendraspis, Patman86, #341)
- 8.3 filepath fix
- Improved Opus encoder (Patman86)
- Apps dialog has a experimental Auto Update feature (stax76)
- NeroAAC removal
- Option to disable MKV subtitle compression (stax76, [#334](/../../issues/334))
- AddGrainC 1.8.2
- VCEEnc 6.05
- L-Smash-Works 2020-07-28


2.1.4.6 Beta (2020-09-13)
============

- Fix eac3to AAC quality mode incorrect bitrate estimation (stax76)
- Fix error at: Main Menu > Tools > Settings > Source Filters (stax76, #322)
- ffmpeg fdk-aac has a option and is disabled by default (stax76, #337)
- Main window focus is supressed after processing in case the last
  window activation is older than 60 seconds (stax76, #333, #298)
- fdkaac removed due to non-free license (stax76, [#292](/../../issues/292))


2.1.4.5 Beta (2020-09-13)
============

- New configuration section in log files, displaying template, profile, muxer, ... - can be extended anytime (Dendraspis, #331)
- eac3to AAC encoding gets correct default quality value and limits (Dendraspis, #328)
- Fixed x265 Preset default value for LookaheadSlices (Dendraspis, (112e150))
- Update checker respects runtime architecture (x64/x86) (Dendraspis)
- MKVToolNix 50 long path support (stax76, #226)
- The wiki was integrated into the tool help. Anybody can add
  detailed tool info and access it from StaxRip (stax76, #292, #320)
- The automatic source filter detection supports detection by format,
  the defaults were reset and use now L-Smash-Works for VP9 (stax76, #312)
- Demuxing is skipped if output files already exist, the old recreate behaviour can be restored
  in the preprocessing settings. If demuxing is canceled then unfinished files are deleted (stax76)
- Fix L-Smash-Works indexing happening multiple times (stax76)
- ffms2 0055b2d StvG


2.1.4.4 Beta (2020-09-06)
============

- eac3to demux dialog showed incorrect language default selection (stax76, #318)
- MP4Box fail to demux OGG chapters (44vince44, #310)
- MP4Box muxer supports chunks encoded with x265 chunk encoding (stax76, #316)
- MP4Box 1.0.0-rev211-g71f1d75ea-x64-gcc10.2.0 (Patman86, #310)


2.1.4.3 Beta
============

- ffmpeg uses libfdk_aac as AAC encoder (stax76, #292)
- Long path support (stax76, #226, experimental)
- fix Decoder ffmpeg DXVA2 (stax76, #290)
- ffms2 2.40-RC1
- MediaInfo.NET 6.6 (long path support)
- AviSynth+ 3.6.2 test 2 (long path support)
- VapourSynth R52


2.1.4.2 Beta
============

- Fix incorrect output mod warning shown.
- Fix incorrect syntax for nvenc/qsvenc --avsdll in avisynth portable mode.


2.1.4.1 Beta
============

- New Presets for NVEnc (P1-P7) added. Reference: https://github.com/rigaya/NVEnc/blob/master/NVEncC_Options.en.md#-u---preset
- Soft links are only created if needed by the current config,
  there is now a confirm dialog with shield icon,
  mpv.net, NVEnc and QSVEnc no longer need soft links.
- New NVEnc options --multipass --repeat-headers.
- Fix mux audio profile Extract DTS Core not working for mka files.
- x264 r3018-db0d417 x86
- mkvtoolnix 49 x64
- NVEnc 5.14
- QSVEnc 4.07
- VCEEnc 6.02
- SVT-AV1 0.8.4-26-g0af191de-msvc1926 Patman
- MP4Box 1.0.0-rev188-g2aa266dfa-gcc10.2.0 Patman
- qaac 2.69-msvc1926 Patman
- ffmpeg N-98647-gffa6072fc7-gcc10.2.0 Patman
- QTGMC 3.368s


2.1.3.9 Beta
============

- Optional mode added to use AviSynth/VapourSynth via VFW.
- All VC++ x64 binaries from abbodi1406/vcredist added.
- Danger Zone setting to disable tool verifications.
- Setting to disable manual plugin loading.
- Experimental 32 bit support.
- VapourSynth R51
- NVEnc 5.13


2.1.3.8 Beta
============

- mkvtoolnix 48
- ec3 file extension support for eac3 files.
- R210/V210 video output using ffmpeg.
- Include beta versions for update checker and show changelog (Dendraspis)
- The final assistant tip supports SHIFT key to add the job at the top of the job list and
  CTRL to prevent showing the jobs dialog. Right-click shows a menu. (Dendraspis)
- Allow to open video files with relative paths on the command line (Dendraspis)
- Create F6 shortcut for Jobs button on Processing dialog (Dendraspis)
- Create F7 shortcut for Log dialog
- 4 commands added to open each source type separately (Dendraspis)
- x265 Presets and Tunes fixes (Dendraspis)
- The Processing dialog has a feature to skip the currently running job (Dendraspis)
- Confirmation on exceptions before files/windows are opened (Dendraspis)
- A new issue template for usage questions was created on the issue tracker and
  the docs were updated to describe that the issue tracker can be used for usage questions.
- For ffmpeg h264_nvenc the command line option generated for quality
  was using -q:v instead of -cq.
- Pass --demuxer-lavf-format=vapoursynth to mpv.net in case the source is vpy.


2.1.3.7 Beta
============

- AviSynth soft link fix.
- DGIndex re-enabled by default for VOB/MPG.


2.1.3.6 Beta
============

- Incorrect command line using VapourSynth and x265.


2.1.3.5 Beta
============

- x264 encoder by default uses avs input.
- x264 encoder has pipe options for raw format.
- Flicker in Video Comparison fixed.
- New command AddBatchJob.
- AviSynth ImageSource support.
- x264 M-0.160.3009-4c9b076-gcc11 Patman
- QSVEnc 4.04
- VCEEnc 6.02
- AVSMeter 3.0.2.0


2.1.3.4 Beta
============

- New setting to use AviSynth portable even when AviSynth
  is installed, portable mode is enabled by default.
- ffmpeg EAC3 encoding support.
- Default AAC encoder changed from eac3to to qaac.
- In batch mode script error handling was fixed.
- ffmpeg muxed audio even for No Muxing profile.
- AviSynth 3.6.1
- ffmpeg N-98276-g1d5d8a30b4-g842bc312ad-gcc10.1.0 Patman
- MP4Box 1.0.0-rev16-g10dd6533a-gcc11.0.0 Patman
- SVT-AV1 0.8.3-57-gba72bc85-msvc1926 Patman
- qaac 2.69
- Bwdif r2


2.1.3.2 Beta
============

- AviSynth/VapourSynth plugin auto load folder support in portable mode (Main Menu > Folders > Plugins)
- ffmpeg muxer uses always -strict -2, it enables TrueHD in MP4
- x264 M-0.160.3009-4c9b076-gcc11 Patman
- NVEnc 5.06
- MediaInfo.NET 6.5.0.0
- Subtitle Edit 3.5.1.5
- TMM2 0.1.4
- AviSynthShader 1.6.6


2.1.3.1 Beta
============

- Main dialog target image width and height text box supports mouse wheel.
- Subtitle file drag & drop in muxer dialog was broken.
- TemporalDegrain2 integration.
- AviSynth 3.6.1 Test 8 (fixes issue with VSFilterMod).
- The docs explain how to use the filter profile editor to edit filter profiles and
  how to create new custom filter profiles for plugins not yet included in StaxRip.
- Video demuxing defined in Preprocessing settings did not work in auto mode.


2.1.3.0
=======

- Processing dialog fix for new Stop After Current Job feature.
- Web URL is included in the search of the Apps dialog.
- Numerous bugs fixed.
- The Processing dialog has a new feature: Stop After Current Job.
- D2V Witch added and enabled by default for VOB/MPG.
- Command line for some processes like ffmsindex wasn't shown while processing.
- Render, scaling and layout fixes and improvements, especially for 96 DPI.
- PowerShell script host supports events, script examples use events, better error handling.
- Improved Log File Viewer (Main Menu > Tools > Log File).
  Tip: The Log File Viewer has a context menu.
- The Preview dialog can be resized with the mouse.
- Support for character # in filenames because MP4Box was finally fixed.
- Various dialogs made resizable and remember their size.
- Maximum number of parallel processes increased from 4 to 16.
- The documentation was greatly improved (still far from perfect though).
- Muxer dialog supports Drag & Drop for subtitles, audio and attachments.
- Video Comparison has hardware render support added.
- aomenc.exe GUI re-enabled.
- Dark color theme for built-in help.
- Improved built-in F1 help.
- ffmpeg video encoder codec FFV1.
- x264 and x265 dialogs have a new Bitrate option, the default value is 0 which
  means the bitrate of the project/template in the main dialog is used.
- For file batch jobs only the file name is shown in the jobs dialog and not the full path.
- Audio encoder supports extracting DTS core using ffmpeg.
- The audio Copy/Mux profile has a Extract DTS Core feature.
- The command line audio encoder has a Default and Forced option.
- New chunk encoding feature for x265 parallel processing.
- Media info dialog replaced with MediaInfo.NET.
- The command line video and audio encoder uses cmd.exe directly without creating a bat file,
  this avoids creating a temporary bat file and adds full unicode support.
- Portable support added, no need to install anything.
- Setting to allow to use tools with wrong version,
  for this a Danger Zone tab was added in the settings.
- The auto crop feature shows progress both in the processing dialog and in the crop dialog.
- Improved issue templates on the github issue tracker.
- The ExecuteCommandLine command has a new Working Directory parameter.
- The launch button in the Apps dialog for a console tool shows its help via Windows Terminal
- Windows Terminal available in the main menu with special StaxRip environment (apps and macros).
- The video encoder dialog feature *Show Command Line* is shown using Windows Terminal.
- Execute Command Line in video encoder dialogs is shown via Windows Terminal.
- In the Apps dialog the tools can be listed using PowerShell Out-GridView.
- Shell Execute flag was added to the command ExecuteCommandLine.
- The global setting 'Add filter to convert chroma subsampling to 4:2:0'
  uses now ConvertToYUV420 instead of ConverttoYV12.
- In various command line features the path environment variable of the process has all exe tools
  added and all macros are available as environment variables.
- Check added that blocks source files with too long path or filename.
  A setting that allows to change the limit exists in the Danger Zone section.
- SVT-AV1 support with GUI.
- Media info folder view was replaced with a new powershell based (Get-MediaInfo)
  dialog that supports caching for fast startup perforance.
- When a Event Command executes it writes a log entry, this is now disabled by default
  but there is a new setting: 'Write Event Commands to log file'.
- In the Jobs dialog there is a button that shows a menu, this menu can now also be shown
  as context menu via right-click on the jobs list and it has various new features.
- The Apps dialog allows to clear custom paths.
- The Apps dialog allows to locate files via Everything.
- Check for Updates added to main menu in Help section.
- Version is shown in main dialog title bar.
- 'Main Menu > Help > Info' shows list with contributors.


2.1.3.0
=======

- processing dialog fix for new Stop After Current Job feature
- x264 M-0.160.3000-33f9e14-gcc10.0.1 Patman
- Web URL is included in the search of the Apps dialog


2.1.2.2 Beta
============

- The Processing dialog has a new feature: Stop After Current Job.
- GPL licensed DGIndex binary re-added.
- D2V Witch added and enabled by default for VOB/MPG.
- Command line for some processes like ffmsindex wasn't shown while processing.
- DFTTest VS re-added to fix dependency issue.
- MCTemporalDenoise AVS dependency issue fix.
- Render, scaling and layout fixes and improvements.


2.1.1.9 Beta
============

- The cut feature was missing the last frame.
- In case the cut feature is used, flac input for qaac is disabled
  so if necessary a w64 file is created.
- Many nvenc improvements.
- Layout and scaling fixes.
- PowerShell script host supports events, script examples use events.
- The menu in the Jobs dialog was improved.
- mvtools2 2.7.43


2.1.1.8 Beta
============

- Improved Log File Viewer (Main Menu > Tools > Log File).
  Tip: The Log File Viewer has a context menu.
- The Jobs dialog has a feature to sort the job list alphabetically.
- The Preview dialog can be resized with the mouse.
- Support for character # in filenames because MP4Box was finally fixed.
- Improved PowerShell scripting error handling.
- Many UI improvements, especially on 96 DPI.
- 'Main Menu > Help > Info' shows list with contributors I could remember.
- x265 M-3.4+6-g73f96ff39-gcc11.0.0 Patman
- nvenc 5.03
- qsvenc 4.03
- mkvtoolnix 47
- AVSMeter 3.0.0.4
- chapterEditor 1.23
- f3kdb Neo r6
- MiniDeen Neo r9
- DFTTest Neo r7
- L-Smash-Works 2020-05-31
- JPSDR 3.2.2
- mvtools2 2.7.42
- VSFilterMod 5.2.2
- TIVTC 1.0.17
- TDeint 1.5
- SMDegrain 3.12.108s
- QTGMC 3.365
- LSFmod 2.187


2.1.1.7 Beta
============

- various dialogs made resizable and remember their size
- various UI issues fixed
- the play feature in the muxer dialog for subtitles was fixed
- flash3kyuu_deband replaced by Neo f3kdb


2.1.1.6 Beta
============

- muxer dialog size issue
- low DPI menu font render issue


2.1.1.5 Beta
============

- In the video encoder dialogs the feature *Execute Command Line* failed when
  Windows Terminal is installed and paths contained spaces.
- The video encoder dialog feature *Show Command Line* is shown using a console.
- Maximum number of parallel processes increased from 4 to 16.
- On systems with restricted PowerShell execution policy several features
  like the media info folder view feature failed.
- The Tools page in the docs lists all tools in a grid view
  with columns: Name, Type, Filename, Version, Modified Date.
- Muxer dialog is resizable and remembers the size.
- Muxer dialog supports Drag & Drop for subtitles, audio and attachments.
- Video Comparison crash fixed and hardware render support added.
- aomenc.exe GUI re-enabled, executable not included
- AviSynth headers updated.
- AviSynth 3.6
- x265 M-3.3+31-g431a22e82-gcc11.0.0 Patman
- NVEnc 5.02
- ffmpeg N-97868-gaa6f38c298-g38490cbeb3-gcc10.1.0 Patman
- yadifmod2 avs 0.2.2
- AVSMeter 2.9.9.3
- KNLMeansCL avs/vs 1.1.1
- MPEG2DecPlus avs 0.1.2 (untested)
- QTGMC avs 3.364s
- masktools2 2.2.23
- DCTFilter 0.5.1
- MediaInfo.NET 6.4.0.0


2.1.1.4 Beta
============

- dark color theme for built-in help.
- improved built-in help for various dialogs.
- various docs pages improved.
- ffmpeg video encoder codec FFV1.
- x264 and x265 dialogs have a new Bitrate option, the default value is 0 which
  means the bitrate of the project/template in the main dialog is used,
  this behavior is documented in the F1 help of the x264 and x265 dialog.
- for file batch jobs only the file name is shown in the jobs dialog and not the full path.
- audio encoder supports extracting DTS core using ffmpeg, generated name is _Extract DTS Core_.
- the audio Copy/Mux profile has a _Extract DTS Core_ feature.
- the command line audio encoder has a Default and Forced option.


2.1.1.3 Beta
============

- fix avs and vpy import adding unnecessary LoadPlugin and Import calls
- new setting: 'Main Menu > Tools > Settigs > System > Use included portable VapourSynth',
  to force usage of included portable VapourSynth instead of installed VapourSynth
- new chunk encoding feature for x265 parallel processing
- DGDecNV removal, there are better open source tools,
  L-Smash-Source supports hardware decoding for NVIDIA, Intel and AMD


2.1.1.2 Beta
============

- if AviSynth or VapourSynth was installed then StaxRip will use
  the installed version instead of the included portable version.
- fix x265 --limit-modes issue.
- fix batch encoding issue.
- mpv.net 5.4.8.0
- VapourSynth r50
- x265 3.3+27-g4780a8d99-gcc11.0.0 Patman
- nvenc 5.01
- mkvtoolnix 46
- RgTools 1.0
- AVSMeter 2.9.9.1
- new docs page Features giving a comprehensive feature list


2.1.1.1 Beta
============

- mpv.net 5.4.6.0
- MediaInfo.NET 6.3.0.0
- x265 3.3+25-ga6489d2fb-gcc10.0.1 Patman
- fix invalid --vpp-tweak nvenc command line generation (Patman86)
- fix import of invalid color metadata into encoder VUI settings (Patman86)
- fix taskbar indication of values below 1 (Dendraspis)
- fix x265 preset (Dendraspis)
- fix crash doing multi select drag operation in jobs dialog
- fix showing the Apps dialog for a tool with non OK status in endless loop
- the command line audio encoder uses cmd.exe directly without creating a bat file,
  this avoids creating a temporary bat file and adds full unicode support
- portable support added and enabled by default, no need to install anything
- nvenc --vpp-transform
- setting to allow to use tools with wrong version,
  for this a Danger Zone tab was added in the settings


2.1.0.9 Beta
============

- detection of Python location improved
- VapourSynth works even if Python nor VapourSynth are in path environment variable
- new x265, NVEnc and SVT-AV1 switches added
- fixed GUI being hidden while auto crop
- DGIndex disabled by default
- mpv.net 5.4.4.3
- SVT-AV1 0.8.2
- NVEnc 5.0
- VCEEnc 6.0
- MP4Box 0.9.0-DEV-rev0-g81b4481e1-gcc10.0.1 Patman
- x265 3.3+19-gcaf9d4dbe-gcc10.0.1 Patman
- ffmpeg N-97384-gcc9ba91bec-g4457f75c65-gcc9.3.0 Patman


2.1.0.8 Beta "The Power Of Walking Away"
========================================

new
---

- VapourSynth filter spline64 added (Patman)
- DG* tools binaries and urls removed according to author request


2.1.0.7 Beta
============

new
---

- a new documentation page [Commands](https://staxrip.readthedocs.io/commands.html) was created.
- the built-in MediaInfo GUI was replaced with MediaInfo.NET which was ported to .NET Framework 4.8.
- the MediaInfo folder view powered by Get-MediaInfo.ps1 v3.0 is shown
  without starting a terminal and it has few bugs fixed.
- the issue templates on the [github issue tracker](https://github.com/staxrip/staxrip/issues/new/choose) were improved.
- `Main Menu > Tools > Advanced > Command Prompt` can be configured in
  the menu editor because it's based on the ExecuteCommandLine command.
  Only people who reset or manually config the main menu will see the change.
- the ExecuteCommandLine command has a new Working Directory parameter.


fix
---

- issue causing audio to be silently ignored instead of muxed
- since v2.1.0.5 DGDecNV usage triggered stack overflow causing staxrip to die silently


2.1.0.6 Beta
============

new
---

- the launch button in the Apps dialog for a console tool shows its help via Windows Terminal
- Main Menu > Tools > Advanced > Command Prompt and PowerShell are shown via Windows Terminal
- Execute Command Line in video encoder dialogs is shown via Windows Terminal
- in the Apps dialog the tools can be shown using PowerShell Out-GridView
- one time tip messages added to inform about otherwise unknown functionality
- a Shell Execute flag was added to the command [ExecuteCommandLine](https://staxrip.readthedocs.io/cli.html#cmdoption-executecommandline-commandline-waitforexit-showprocesswindow-useshellexecute), this is a setting in the
  process class of dotnet, it causes the usage of the ShellExecute Win32 API instead of
  CreateProcess, this APIs have different feature sets like CreateProcess accepting only exe
  files and being able to pass in environment variables and use redirection
- the [Command Line Interface doc page](https://staxrip.readthedocs.io/cli.html) was significantly improved


fix
---

- CSV file creation possibly incompatible with Excel in certain locales


2.1.0.5 Beta
============

new
---

- the global setting 'Add filter to convert chroma subsampling to 4:2:0'
  uses now ConvertToYUV420 instead of ConverttoYV12
- in custom command line based demuxers and in the ExecuteCommandLine
  command the path environment variable of the process has all exe tools
  added and all macros are available as environment variables
- AviSynth filter profile using ColorYUV function


update
------

- mkvtoolnix 45.0.0
- nvenc 4.69
- DFTTest 1.9.5 Clang
- L-Smash-Works 2020-03-22 HolyWu
- ffms2 89bd1e1 StvG

fix
---

- the MediaInfo Folder view did not work unless Get-MediaInfo was installed,
  now it should work even without being installed, please try and give feedback


2.1.0.3 Beta
------------

- new: check added that blocks source files with too long path or filename. A setting
       that allows to change the limit exists and there is also a explanation as tooltip:
       In theory Windows supports paths that are longer than 260 characters, in reality
       neither Windows, nor the .NET Framework or the used tools have full long path support.
- new: in order to support unicode the command line based encoder used by XviD uses
       now cmd.exe directly without creating a batch file. This command line based encoder
       is not only useful for XviD but can be used for any command line based encoder
- new: very basic SVT-AV1 encoder support added

- update: MediaInfo 20.03
- update: AviSynth 3.5.1
- update: ffmpeg N-97107-g33c106d411-g72be5d4661+2-gcc9.3.0 Patman

- fix: 2 reported typos

### 2.1.0.2 Beta

- new: the MediaInfo folder view was replaced with a new powershell based
       dialog that supports caching for fast startup perforance

- fix: install instructions for wrong versions in the Apps dialog were improved
- fix: UNC path issue fix (AMED)
- fix: x265 --hdr-opt renamed to --hdr10-opt

- update: x265 3.3+10-g08d895bb6-gcc9.3.0 Patman
- update: Python 3.8.2
- update: VapourSynth R49
- update: VC++ 2019 14.25.28508.3

### 2.1.0.1 Beta

- fix: MediaInfo Folder View was broken and main window
       did not show the video codec, both had the same reason

- fix: Change default values of Early Skip and Psy-RDOQ
       for various presets (tnatiuk17piano)

- new: when a Event Command executes it writes a log entry,
       this is now disabled by default but there is
       a new setting: 'Write Event Commands to log file'

- new: in the Jobs dialog there is a button that shows a menu,
       this menu can now also be shown as context menu via right-click
       on the jobs list and it has a new menu item to check the
       selected jobs

### 2.0.9.12 Beta without apps

- fix: play issue in prieview

### 2.0.9.11 Beta without apps

- fix: the app verification reported that a old tool version
       was found when actually it was a new version

- new: the Apps dialog allows to clear custom paths
- new: the Apps dialog allows to locate files via Everything

### 2.0.9.10 Beta without apps

- fix: issue with network paths

### 2.0.9.1 Beta

- new: Check for Updates added to main menu in Help section

- fix: QTGMC did not work on systems without AVX2
- fix: Window position of the main dialog was not remembered
- fix: FrameServer.dll again released as debug build because
       of a bug in the new release script

### 2.0.8.12 Beta without apps

- fix: external avs/vpy scripts that use relative instead of
       absolute paths failed to load

### 2.0.8.11 Beta without apps

- new: Version is shown in main dialog title bar

- fix: event command issue
- fix: layout and usability issue in audio dialog (Patman)

### 2.0.8.0 Stable

- fix: using Event Command After Encoded with SetTargetFile
       caused exception
- fix: if the default code page is UTF8 then staxrip
       don't warn about avisynth not supporting Unicode

- update: mkvtoolnix 44

### 2.0.7.7 Beta

- new: the apps management and verification has been improved

- fix: usage of too old VC++ 2019 runtime is now prevented

- update: qsvenc 3.33
- update: nvenc 4.68
- update: vceenc 5.04
- update: MiniDeen r6

### 2.0.7.6

- fix: format compatibility was improved with an automated test
- fix: in the preview dialog the start position for mpc
       and mpv was incorrect when cut ranges were active
- fix: chapter file not being picked up
- fix: in the filter setup profiles dialog a exception could
       happen and loading a filter setup from the menu could fail
- fix: the filter parameter menu in the code editor was adding
       parameters even if they were already existing with different casing
- fix: cutting could cause an error where staxrip generates
       a chapter file without containing a ChapterAtom

- new: the logic to create and edit cut sections in the preview
       dialog is now smarter and more flexible       
- new: the assistant tells in case of a script error that the
       full error can be shown by clicking the preview button
- new: MiniDeen avs filter added
- new: Advanced Info in the code editor was moved to the top level menu
       and AviSynth Info() can be shown directly from the Advanced Info
       without adding Info() to the script, staxrip generates the script
       with perfect font size and shows it with mpv (paused, no osc, no osd)

### 2.0.7.5 Beta

- fix: when the cut feature was used and afterwards the preview was
       opened from the code editor then the cut settings got damaged
- fix: when AviSynthShader was used StaxRip did not show
       a warning in case DirectX 9 is not installed
- fix: HE-AAC as demuxed as mka

- new: setting that allows to define how many frames
       are used for auto cropping
- new: some default settings were changed
- new: the buttons in the preview dialog look much better now, also flicker
       was eleminated and the button and trackbar size was increased,
       standard buttons were used, I don't know how it looks on Win 7...
- new: the csv file content in the apps dialog was improved
- new: auto crop feature in crop dialog finally shows progress
- new: the avisynth and vapoursynth code editor has a new
       'Advanced > Advanced Info' feature, it offers the following info:
       avs2pipemod info, avsmeter info, avsmeter benchmark and vspipe info

### 2.0.7.4

- fix: bug in crop dialog was fixed
- fix: raw format of VP8/VP9 (IVF) was undefined which
       caused a NotImplementedException
- fix: OverflowException of unknown cause
- fix: using txt chapters with mkv muxer caused xml exception
- fix: if the width was not mod 16 the crop and preview
       dialog showed a distorted image

- update: AviSynth 3.5.0

- new: the apps dialog can create a CSV file listing all tools
       with various properties like Name, Version, Modified date etc.
- new: x265 --rskip switch updated
- new: improved main menu
- new: improved app management dialog
- new: mpv and mpc started in the preview dialog start at the current
       time position of the preview instead of the beginning

### 2.0.7.3 Beta

- fix: x265 issues with --refine-ctu-distortion and --high-tier
- fix: certain avs and vpy files failed to load before, hopefully all external
       scripts can now be opened
- fix: opening source files that contain single quotes caused a VapourSynth
       script error, the fix will however only work when people reset their
       filter profiles
- fix: the local context help of x264 did often not work

- update: nvenc 4.66
- update: L-Smash-Work 20200207 HolyWu
- update: VSFilterMod R5.2.1
- update: BDSup2Sub++ 1.0.3
- update: qsvenc 3.31
- update: JPSDR 3.2.0
- update: d2vsource 1.2
- update: AVSMeter 2.9.8
- update: chapterEditor 1.21
- update: Subtitle Edit 3.5.13
- update: rav1e 0.3.0
- update: x264 2991-1771b55 Patman

- new: improved possibility to show AviSynth and VapourSynth script
       information with a new dialog and new pixel format parameter       
- new: the right click context help in the x264 and x265 dialog can now
       alternativly open the online help with ctrl or shift + right click
       to navigate directly to the online help of the right-clicked switch
- new: the F1 help in the video encoder dialogs was greatly improved
- new: in the nvenc dialog custom switches do overwrite regular switches
- new: Bwdif plugin for VapourSynth (similar to yadif)
- new: filters can be removed from the filter list view in the main dialog
       using the delete key
- new: qsvenc --colorrange --dhdr10-info --key-on-chapter --sub-source
- new: x264 --avcintra-flavor --index --input-fmt --muxer --quiet
       --verbose --video-filter
- new: the short version command line switches (like -t) are now supported
       in the x264 dialog


### 2.0.7.2 Beta

- new: showing MediaInfo for the source file works now even if
       the source file is an d2v/dgi index file

- new: improved detection to find out if all video encoder
       command line switches have a GUI implementation,
       (there is still a lot work to do)

- fix: use FrameServer.dll release build instead of debug build

### 2.0.7.1 Beta

- new: icons added in encoder dialog menus
- new: option dialog is shown in case a tool has multiple help
       resources, x265 for instance has a local help file containing
       the console help and it has a comprehensive online help.
       L-Smash-Works for instance has separate help pages
       for AviSynth and for VapourSynth
- new: medium quality for x264 was changed from crf 20 to 22
- new: medium quality for x265 was changed from crf 20 to 18
- new: %dpi% macro added, returnes the DPI value of the main dialog screen
- new: High DPI aware Info() AviSynth filter profile added to misc section
- new: the VFW interface used to access AviSynth and VapourSynth
       was replaced with a new library
- new: a dozen new x265 switches
- new: nvenc switches --colorrange, --psnr, --ssim
- new: search matches in the video encoder config dialog
       stay now permanently highlighted with bold font
- new: the search feature in the video encoder config dialog now
       also searches in option values, for instance searching
       for 'medium' will find and highlight the 'preset' switch
- new: in the video encoder config dialogs the context menu of the
       command line preview has a new menu item to search
       for the switch or string at the caret or cursor
- new: Opus format enabled for MP4Box muxer, this was requested
       but did not work in my test
- new: bmp format for cover art can be used in muxer dialog

- update: nvenc 4.65
- update: qsvenc 3.31
- update: vceenc 5.02
- update: x265 3.3+2-gbe2d82093 GCC 9.2.0 Patman
- update: ffmpeg N-96788 GCC 9.2.0 Patman
- update: mkvtoolnix 43
- update: MP4Box 0.8.0-rev178-g44c48d630 Patman

- fix: x265 three pass encoding (untested)
- fix: zoom in/out was flipped in preview dialog

### 2.0.6.2

- new: crop dialog supports hardware accelerated video rendering

- fix: info feature in preview dialog is back
- fix: when a second preview was opened the first one had broken rendering
- fix: image was vertically flipped using preview dialog with VapourSynth
- fix: main window DPI scaling issue on 96 DPI, please post a screenshot
       if you find something that doesn't look good!
       
### 2.0.6.1

- new: the preview dialog uses now a Direct2D hardware accelerated video renderer

- fix: the info tool in the code editor was not showing the correct colorspace

### 2.0.6.0 Stable

no changes since 2.0.5.3 Beta

### 2.0.5.3 Beta

- update: mkvtoolnix 39
- update: mpv.net 5.4.3
- update: NVEnc 4.55

- fix: few UI issues
- fix: avs MCTemporalDenoise/GradFun2DBmod
- fix: VUI luminance issue

### 2.0.5.1 Beta

- update: DGHDRtoSDR 1.13
- update: NVEncC 4.54
- update: VapourSynth R48
- update: x265 3.2+9-971180b100f8 Patman

- new: various UI improvements
- new: the short version of x265 and nvenc switches like -c, -f etc.
       were integrated into the search field and command line import
       feature, the search feature now first looks for a exact match
- new: x265 switches --dup-threshold, --frame-dup
- new: nvenc switches --multiref-l0, --multiref-l1
- new: StaxRip can use MediaInfo.NET instead of the built-in MediaInfo GUI,
       it requires MediaInfo.NET to be installed and started at least once

- fix: FFTW not always asked to be installed when needed

### 2.0.4.10 Beta

- update: mkvtoolnix 38.0.0
- update: x265 3.2+5-gfbe9fef31 Patman

- fix: issue using trim with multiple preview instances

### 2.0.4.9 Beta

- new: code formatting of avs/vs code was improved according to most common standard
- new: tab order changed in nvenc options

- fix: apps with custom path and unknown filename failed to launch
- fix: nvenc --aq-strength is now only visible if --aq is enabled
- fix: the right-click/context help in the encoder options failed in some cases 
- fix: a critical issue with the playback feature in the preview was fixed

### 2.0.4.8 Beta

- fix: MPC was shown as missing even for operations that do not require MPC

### 2.0.4.7 Beta

- new: to use nvenc --vbr-quality there is now a checkbox 'Constant Quality Mode'
       on the rate control tab (Patman86)
- new: --bref-mode added to nvenc
- new: after a source is loaded StaxRip automatically adds a filter
       to convert chroma subsampling to 4:2:0, this can now be
       disabled in the settings on a new Video tab
- new: MPC player integration was added to script editor and preview
       via menu and F10 key, mpv.net key was changed to F9

- fix: --aq-strength enabled for nvenc h265
- fix: there was a issue with the resize slider and resize menu using VapourSynth
- fix: in the preview the reload feature was causing an error in case
       the player was invoked before, also trim wasn't applied in the player
- fix: shortcut keys in the script editor did not work

### 2.0.4.6 Beta

- fix: main dialog was not set to quality mode using nvenc --vbr-quality

### 2.0.4.5 Beta

Thanks to Patman for providing the download links of the tools !!!

- update: JPSDR 3.1.3
- update: nvenc 4.50
- update: AVSMeter 2.9.6
- update: DGHDRtoSDR 1.12
- update: mvtools2 2.7.41
- update: FFT3dFilter 2.6.7
- update: TIVTC 1.0.14
- update: RgTools 0.98
- update: havsfunc r32
- update: DFTTest r6
- update: TCanny r12
- update: BM3D r8
- update: L-Smash Works 2019-09-17 HolyWu
- update: x265 3.2+3-fdd69a76688 Patman
- update: ffmpeg 2019-09-27 Patman

- new: x265 switch --selective-sao added
- new: L-Smash Works parameters prefer_hw 3 HW auto added to menu
- new: the way --vbr-quality works in nvenc was changed,
       if it's value is -1 the bitrate in the main dialog is used,
       if it's higher then -1 the bitrate is set to 0
- new: add support of video track title/name using MP4Box
- new: simple QP mode added to nvenc to use one instead of three QP values
- new: the menu renderer has now Win 10 style arrows for sub menu indication
- new: icons added to filter list and script editor, shortcuts added to script editor
- new: the preview can be closed by clicking in the top right corner
- new: in the script editor there are now two players available,
       mpv.net and whatever is registered for mkv, mpc-be supports vpy playback
- new: the way filters are added, replaced and inserted in the script editor
       was changed in order to improve the menu performance
- new: in the main menu a sub menu was added: Apps > Script Info,
       it contains avsmeter, vspipe and avs2pipemod for showing
       info about the currently active AviSynth or VapourSynth script,
       vspipe and avs2pipemod show parameters like bit depth, colorspace and framecount.
       In the script editor there is a new 'Script Info' menu item (Ctrl+I) doing the same.
- new: the mediainfo dialog shows the formats in the tab captions
- new: the mediainfo dialog has a new feature to move
       to the next and previous file via menu and shortcut
- fix: docs weren't built automatically
- fix: some bugs were fixed in the nvenc command line import feature

### 2.0.4.4 Beta

- fix: nfo files with non xml content caused an exception
- fix: if subtitle and audio titles contained illegal
  file system characters then these characters were replaced
  with an underscore and thus being lost, now the characters
  are escaped/unescaped and thus preserved
- fix: update x265 switch --refine-mv
- fix: in the nvenc dialog, Intel decoding options were present
  even when no Intel GPU is present
- new: a link to the web site has been added to the menu
- new: option to check online for new stable version once per day
- new: avs plugin HDRTools
- new: avs, vs plugin DGHDRtoSDR
- new: ffms2 parameter colorspace has menu support in the avs code editor

### 2.0.4.3 Beta

- new AviSynth script CropResize added
- update: x265 3.1+15-a092e82 Wolfberry
- update: MP4Box 0.8.0-rev69-5fe3ec1 Wolfberry
- update: ffmpeg 4.2.1 Wolfberry
- update: L-Smash Works 2019-09-14 HolyWu
- the filter list in the main dialog shows shorter filter names,
  for example 'BicubicResize' instead of 'Resize BicubicResize'
- SubtitleEdit is now included
- chapterEditor has been integrated into the main menu and
  into the container/muxer dialog
- a Tag File option has been added to the container/muxer dialog
- container/muxer has a Tags tab added with a grid view for editing tags
- Kodi nfo files are imported into tags

### 2.0.4.2 Beta

- new macros added: %source_dar%, %target_dar%,
  %source_par_x%, %source_par_y%, %target_par_x%, %target_par_y%
- fix: menu support of L-Smash Works parameters prefer_hw was incomplete

### 2.0.4.1 Beta

- nvenc switch --avhw updated (Patman86), --sub-source added
- qsvenc switches added: --data-copy, --adapt-ltr
- progress display support added for DGIndexNV, MP4Box and ffmpeg audio encoding
- L-Smash Works parameters prefer_hw and format have menu support in the avs/vs code editor
- the container/muxer dialog has a new tab for MKV attachments

- fix: vpy import did not work when the output variable isn't named 'clip'
- fix: a format exception was fixed, it was happening when the cut/trim feature was used

- update: nvenc 4.47
- update: L-Smash Works 2019-09-10 HolyWu
- update: MediaInfo 19.09
- update mpv.net 5.4.1.1

### 2.0.4.0 Stable

- update: ffms2 2019-08-30 StvG, successfully tested was the load time,
          memory usage and avs/vs compatibility
- update: L-Smash-Works r935+34 2019-08-29 MeteorRain, successfully
          tested was the load time, memory usage and avs/vs compatibility
- update: MediaInfo 19.07
- update: ffmpeg 4.2 Wolfberry
- update: nvenc 4.46
- update: qsvenc 3.24

### 2.0.3.1 Beta

- piping tool option added to x264 encoder
- piping with ffmpeg supports 10 bit now
- the track title wasn't detected from demuxed audio and subtitle tracks
- closing the settings dialog was setting the icon location as last source directory
- debug logging was improved, more debug info is included, it's no longer
  needed to edit the config file to enable debug logging, it's sufficient
  to enable it in the settings: Tools > Settings > General > Enable Debug Logging

### 2.0.3 Stable

- colorspace="YV12" removed from ffms2 defaults because it converts to 8 bit
- the x264 encoder uses now avs2pipemod64 because the avs input did not support 10 bit
- DTS-X is now demuxed as dtshd instead of mka
- update: mkvtoolnix 36.0.0
- update: avs FluxSmooth 1.4.7
- update: avsmeter 2.9.5

### 2.0.2.7 Beta

- added fdkaac pipe input support, in the audio settings
  go to: More > General > Decoding Mode > Pipe
- added missing icons
- file creation/write dates of apps were recovered
- update: ffms2 r1275+2-2019-08-11 HolyWu
- update: eac3to libraries libdcadec and libFLAC
- update: MP4Box v0.8.0-rev41-gb78fe5fbe Barough
- update: qaac 2.68

### 2.0.2.6 Beta

- dialogs were closing slow with 4K sources
- .NET Framework version updated to 4.8
- nvencc switch --vpp-select-every added
- fix for play menu item in filters menu of main dialog being disabled
- fix audio being not loaded by mpv.net when the avs/vs script is played
- fix a issue with the custom icon feature
- update: x265 3.1+11-de920e0 Wolfberry
- x265 --aq-mode update, new switches --hme and --hme-search added
- update: nvenc 4.44, new switches --data-copy, --nonrefp, fix --vpp-subburn
- update: L-Smash r935+31-2019-08-17 HolyWu
  added support for showing indexing progress and cachefile parameter and
  for native high bit depth, avs and vs l-smash are now contained in the same dll,
  avs and vs filter profiles have been changed to contain the the new cachefile
  parameter and high bit depth support has been added to the profiles, the profiles
  have not been reset so users have to update existing profiles manually
- update: VapourSynth R47.2, the new L-Smash update requires this
  new VapourSynth update
- update: python-3.7.4-amd64-webinstall
- update: mpv.net 5.3

### 2.0.2.3 Beta

- the progress bar in the processing window was using black as text color,
  this was changed to use SystemBrushes.ControlText which should be defined
  by the OS
- if the x265 dialog was opened before a source file was loaded, there could
  be an exception after the source file was loaded
- tooltip improvement in the muxer options

### 2.0.2.2 Beta

- new icons added and credits in about (info) dialog updated,
  thanks to: Freepik, ilko-k, nulledone, vanontom
- the progress bar in the processing window has better contrast
- video encoding with ffmpeg shows now progress in the progress bar and task bar
- if in the codec dialogs search field the enter key is pressed in order to cycle
  there is no longer an annoying sound made 
- encoding was failing when a ambersand (&) character was used in the path
- new AV1 codec option added to ffmpeg encoder
- various improvements and fixes made in the ffmpeg encoder
- new vceenc switches added
- new nvencc switches added
- new x265 switches added
- update: x265, Wolfberry build with avs input support,
  in the x265 options: Other > Piping Tool > None
- update: mpvnet
- update: ffmpeg, Wolfberry build with vpy input support
- update: nvencc, untested
- update: qsvencc, untested
- update: vceencc, untested

### 2.0.2.1 Beta

- Under 'Tools > Settings > User Interface' there is a setting to define a icon file

### 2.0.2.0 Release

- fix: downloads were using 7z extension even though it was zip instead of 7z

- update: LSMASHSource avs
- update: mkvtoolnix

### 2.0.1.3

- new: again new experimental icon, please give feedback if you like it,
  if it's not liked then it will be reverted to the classic icon, the problem
  with the classic icon is it looks outdated because it's not flat
- new: added ConvertFromDoubleWidth to AviSynth profiles, it's useful to fix the
  double width output that l-smash outputs for 10 (or more) bit sources.
  The profiles were not reset so the changes are only available
  after reseting the profiles manually
- new: in the x265 dialog it's possible to select the preferred piping tool,
  use at your own risk, not all combinations will work. In theory the included
  x265 built don't need a piping tool but I could not make it work without
  a piping tool. Options are: Automatic, None, vspipe, avs2pipemod, ffmpeg

- update: ffmpeg, unlike the built before this one supports mp3 encoding
- update: x264, x265
- update: d2v vs filter plugin

### 2.0.1.2

- fix for audio streams demuxed as mka

### 2.0.1.1

- new: nvencc --vpp-nnedi, --vpp-yadif
- new: few new x265 switches added

### 2.0.1.0

- new: experimental new icon, please give feedback if you like it,
  if it's not liked then it will be reverted to the old icon
- new: in the main menu under 'Apps > Media Info > vspipe' you can show info
  like shown [here](https://forum.doom9.org/showpost.php?p=1874254&postcount=3394).
  The main menu was not reset so it shows only if you reset it at: Tools > Edit Menu 

- change: selecting a filter in the filter menu, the names that are shown in the list were improved

- update: nvenc, x265
- update: ffmpeg, it's now possible to use vpy with ffmpeg
- update: rav1e, it's now possible to use rav1e with VapourSynth

- internal: Release() method (F11 key) added to automate the release process
- internal: Test() method updated to use relative paths

### 2.0.0.2

- there are no longer plugins that are shared between VapourSynth and AviSynth,
  ffms2 for VapourSynth was updated, the AviSynth version is 3 years old,
  ffms2 with C interface was removed because it was leaking memory
- the apps manage dialog displays the built date (using last modified date)
- on startup the main window often wasn't activated
- there was a bug that resulted in subtitles being demuxed without file extension
  and because of that staxrip couldn't find and include the subtitles, the reason
  was a breaking change in MediaInfo
- audio files were as mka demuxed due to a breaking change in MediaInfo
- webm files without mpeg-4 video format failed to load
- the navigation tree in the app manage dialog had a flicker issue
- MediaInfo was updated, most recent bugs are due to breaking changes in MediaInfo
- vslsmashsource updated
- mkvtoolnix update
- mpv.net was updated to version 3.5
- a major performance issue using AviSynth was fixed

### 2.0.0.1

- Removed Auto insert for RGB for VS and Convert Bits for AVS. Most are no longer useful due to 10Bit+ Videos are more common now.
- This is a pure Refresh Release, Mainily designed to update some of the encoding tools.
- This Release contains all the Changes from the Pipeline along with the Following.
- Updated Temp Folder Creation and Long Path Checker (Long Path Checker is only used on any OS below Window 10)
- Updated Comparison Tool It shouldn't need to index file when ffms2 is used.
- Updated Both Contact Sheet Creator and Video Comparison tool to use LWLibavVideoSource over the Standard L-Smash.
- Updated Visual Studio to 2019 RC, Anything Compiled will be using RC Build of VS 2019.
- VS: The default Resizer Code is slightly Different Compared to Base VS Resizer Code in the Context Menu.
- VS: SVPFlow Altered to Target CurrentFramte & Num / Den to achieve the Final Framerate instead of rounding. Type quality has also been Changed, aswell linear light support has been added (GPU Only).
- x265: AQmode Tweaked for the new Defaults.
- XAA Remove from AVS (Temp), due to certain Functions being alter in AVS+ core.
- VS: Updated DGIndexNV Load Function It now loads using it's native method instead of AVS load function.
- VS: G41Fun replaces hnwvsfunc
- VS: hnwvsfunc Script removed
- VS: fvsfunc functions updated
- VS: Auto-Deblock Added to Context Menu
- VS: nnedi3_rpow2 added to Script folder for Support.
- VS: Added FixTelecinedFades to Context Menu for RCR.
- Rebuilt QVSEnc Encoder Script
- x265 Has been updated to 3.0-AU With any Additional Chnages.
- Added Merge: XviD Encoding code has been Updated (Credit: jkilez)
- Added Merge: Chapter Cutting Feature to MkvMuxer (Credit: wybb)

2.0
- Brand New Update System All done within the App
- New Tab Added to x265 for Bitstream due to all the new Flags.
- Updated the default state of the x265 flags that were changed.
- x265 Flags Added:
    - Zone File
    - HRD Concat(The Flag is currently improperly marked in x265 cli)
    - Dolby Vision RPU
    - Dolby Vision Profiles
    - Tuning: Animation
    - Refine CTU Distortion    
    - hevc-aq
    - qp-adaptation-range
    - refine-mv-type renamed to refine-analysis-type    
- rav1e Flags Added:
    - Matrix
    - transfer
    - primaries
    - Min Key Int
- Added Support additional support for LongPaths 
    - There are two ways the app can name the files & Folders based on your OS & Harddrive Type
    - If One Method fails it now has a fallback method to use.
- Fixed QSVEnc key, Which was locking some users out.
- Updated the VapourSynth scripts that have been updated recently.
- Updated the filter names for Vapoursynth and removed the ones that don't exist anymore.
- Updated the switches for VCEnc
- Updated the MTModes Syntax for AVSynth
- Updated DFTTest syntax for Vapoursynth, Opt Settings is now set to AVX2(Settings it below 3 will set it to AVX or None).
- Added additional source filter when StaxRip internal thumbnail is used, some source files did not get along with ffms.
- Updated VapourSynth and Tweaked SVPFlow & Added Additional Option to use BlockFPS.
- Updated FFmpeg, MKVToolNix, NVENC, Rav1e, x265 & MediaInfo
- Python Support now includes Miniconda as well, Python 3.7.2 can be used as well.
- Included Both Old and Newer versions of MediaInfo Just incase it breaks something again.
- Re-Enabled MediaInfo Folder Function, Since The Bug in MediaInfo has been fixed.
- Added & Updated All the Output Path Options for MTN, StaxRip Thumbnailer, Gif & PNG Creators.
- The Help System for the Encoders Has been Altered. All the Help System works the Same now.
- Updated the dll files for both Avisynth & VapourSynth for filters that have been updated.
- Support for PNG has been added for VFW Saving Screenshot(Default Options are PNG & BMP, With JPG being able to be added in the Edit Menu).
- Cleaned up and Organized for File Structure inside App Folder.
- a small tweak in regards to the VFW.
- Updated mpvnet
- Plus some other changes as well.
- Plus all the Changes From Previous Two Beta Release(See Below)

2.0 Beta 2
- Updated a few of FFMpeg Flags for Audio Demuxing / Encoding.
- Moved HDR Ingest in Options Menu to Video Tab.
- Support for HLG Metadata has been Added to Ingest.
- Added Support for VUI for the Following ColorSpaces: Display P3 and DCI P3 (It still Must pass the other HDR Checks)
- Proper Master-Display data has been added for Display P3 and DCI P3.
- Support for Webm Subtitles has been Added For srt, sup, idx Files.
- Updated the VUI Import for HDR10, Due to MediaInfo Changes to Output Names.
- Changed the VUI import name for HLG, Due to MediaInfo not using proper colorspace name.
- Added Some Support for HLG to VUI import function, MediaInfo does not contain all HLG Metadata.
- Updated all the Help Files for the Encoders.
- Rav1e Encoder Has Been Added to Support AV1 codec.
    Flags:
        --tune
        --limit
        --speed
        --quantizer
        --keyint
        --low_latency
        --custom
- NVEnc Added Flags:
    -profile(h265)
    -vpp-padding(Left,Top,Right,Bottom)
    -vpp-tweak(Contrast,Gamma,Saturation,Hue,Brightness)
    -chromaloc
    -interlace tff
    -interlace bff
    -tier
    -pic-struct
    -aud
    -slices
    + Others...
- QSVEnc Added Flags:
    -vbv-bufsize
    -chromaloc    
    -vpp-scaling -> -vpp-resize
    -Filter: mctf
    -sao
    -ctu
    -tskip
    + Others...
- Removed any Switches that no longer exist in the CLI Encoders.
- Updated FFMPEG to 4.1
- ReBuilt MPVNet to work on Both older and Newer Systems(Requires Dotnet 4.8).
- Support has been added for MKV, Webm and MP4 to AV1.
- 32 Float Filters have been Added(Oyster, Plum, Vine)
- FFMPEG Shared dll Files has been Updated to 4.1
- Added mpvnet back with fully working mpv dll file.
- Re-Enabled MediaInfo Folder, It's Been Fixed to with latest MediaInfo Code.
- x264 has Been Updated to 2935
- x265 has Been Updated to 2.9+9
- Cleaned up the Config Files.
- Update Script has been moved to Python Code, instead of basic Powershell Script.
    - Site Packges Required: bs4(BeautifulSoup), Requests, win32api, tqdm, & psutil
- Added Update Script for NVEnc & QVSEnc.
- mtn has been Updated, Uses less Shared files.


2.0 Beta 1
- Temp Folder Creation has been slightly altered
- Removed some old Code, That wasn't doing anything anymore.
- AVSMeter has Been Updated to 2.8.6.
- Updated eac3to, fdkaac & qaac.
- nnedi3 for AVS has Been Updated to 0.9.4
- x264 & x265 have compiled Using GCC ToolChain instead of MVS.
- Python Search Path has been Updated to 3.7/3.7.1 & Python 3.6 has been Removed.
- Also Included Support for Anaconda3.
- The VapourSynth Script has been slightly altered to work better with Python Search Path.
- VS has been Updated to R45.
- MKVToolNix Updated to 28.2.0.7
- Added hnwvsfunc for VS & mClean to Context Menu.
- Cleaned up the AVS Filter Names.
- Added MultiSharpen Function
- Modified the Profile List for x265, Only Profiles that work with Current Selected Depth with Display now.
- Removed any 8Bit x265 Profiles that no Longer used with 2.9(Based on: https://x265.readthedocs.io).
- Moved Some Extra Functions to the System Process.
- Custom Directory Output Option has been Added to StaxRip Thumbnail Creator & MTN. It's Default Directory is the last Used Location. If none is known it defaults to C Drive(Fresh Install)
- Added support for Long Path Aware(Make Sure to Enable it If your OS Supports it)
- Fixed any Issues for very Short filenames being converted to wrong type.
- Removed MediaInfo 18.08.1 Due to Bugs it contains and Re-Added MediaInfo 18.05.
- Added More Support for HDR to Intel Encoder.
- VS Filters Added: W3FDIF, MiniDeen, IT, TDeintMod, VC* Filters & TemporalMedian.
- Updated mvtools-sf to AVX, The latest Builds only use AVX2 which not all CPU's Support.
- Added Support for RawSourcePlus for Avisynth and RawSource for VapourSynth. Default Pixel_type is set to YUV420P10 (aka P010).
- Dual Package Setup for x264 have been Removed for Single release x264(8+10Bit). The Depth menu will set the Output Bits, exactly like x265.
- Following Have Been Added to x264:
    - ColorMatrix: Added chroma-derived-nc, chroma-derived-c, and ICtCp.
    - Added Alternative Transfer
    - vbv-init has been modded to have a locked value and proper float point scaling.
- FFTW Files will auto copy to System Directory, if your PC is missing these files in the System32 Folder(Fresh Install).
- MP4Box is back to Static Once again.
- Included Setup files for Python 3.7.1 & VS R45 to make sure the proper versions are being installed for StaxRip.

1.9.0.0

- Added a new Marco to the list.
    - %Script_files%    
- Added Total System Memory to System Environment & Logging
- mpv has been updated to the latest nightly release
- FFmpeg has been updated to 4.0.2
- Mediainfo has been updated to 18.08.1
- MP4Box has been Updated and includes required dll files that are required now.
- NVEnc has been updated to 4.12
- QSVEnc has been updated to 3.09
- x264 has been updated to 2932
- x265 has been updated to 2.8+68
- MKVToolNix has been Updated to 26.0.0
- AVSMeter has been Updated to 2.8.5
- Added Two new Scripts for HDR10+ AVS & VS Version. Along with updated Script for Source load.
- Added Support Added for FFMS LoadCPlugins. FFMS2K will use the normal LoadPlugin Function.
- Avisynth and Vapousynth have Separate Menu tabs in the App Forms. Vapoursynth is no longer buried inside Avisynth Menu at the bottom.
- Removed all the dead links from Package Section and Replaced them with the most up to date URL's, If one existed.
- Added Support for PNG & Tiff when saving a screenshot of a single Frame in the Preview Window. To add or Remove a format, Everything can be done through the Edit Menu.
- StaxRip now has a Update Function through PowerShell Script(Can be Accessed Via: Tools -> Advanced -> Check For Update. It will first Shutdown the app and check for updates. If a new version is avaible, it download, extract and re-open StaxRip.
- Updated the PowerShell 5 Reference Assemblies to version 1.1.0.
- Framework has been Updated to 4.7.2.
- The main drop-down menus have been cleaned up and better organized
- The control boxes For the codecs have also been cleaned up, to create a shorter drop down.
- Subtitle Tools have been moved to Advanced -> Tools -> Subtitles.
- Added vscube, Second Scripts for Utils and some small tweaks to selected scripts for AVS+.
- Updated VapourSynth Plugins & Context menu (See Below for Context Options).
- Like with AVS+ There are allot more functions supported, Not all have been added to the context menu.
- Settings:
    - Template Folder has been Renamed to just Templates & .dat file has been Renamed to Settings.dat. All your previous Templates will still work, Just move them to the new folder.
- Internal Thumbnail Creator Has been Updated with the Following Features:
    - Added support for PNG, BMP & TIFF. 
    - It Fully supports HDR10, HDR10+ and HLG.
    - Add support for Gap (The space between each shot, default: 0).
    - The ability to disable the StaxRip Logo has been added.
    - The Font styles have been changed to DejaVu Serif which give a much clearer & cleaner look. 
    - The Default Logo Font Style has been changed to mikadan, Which gives the logo a bit more Style.
    - If for what reason you do not have DejaVu Serif or mikadan font, They have been included. Once installed They can be deleted. FontAwesome & Segoe-MDL2-Assets must remain next to .exe file.
    - Cleaned up the Header to make look more Professional like and added more video & Audio Details.
        - File: name
        - Size: Filesize in bytes / MBS, Length, and average bitrate.
        - Video: Codec, Format Profile, colorspace,colour_range, Bitrate, FrameRate, ChromaSubsampling, Height & Width
        - VideoCodec (The old code wasn't working with latest version of Mediainfo.)
        - Audio: Codecs, Channels, kHz and Bitrate
    - The Matrix for the Creator has also Been tweaked for correct colorspace.

- Added Support for New Thumbnail Creator Which can be used by it self or added into the project process. It Uses FFMPEG engine to create the Thumbnails without the need to index any file(s).
    Options Support as of Right Now:
        - Number of Columns
        - Number of Rows
        - Width of Each Shot
        - Height of Each Shot
        - Depth of Each Shot
        - Quality of Jpg

- Added Animation Support in the Form of gif & apng(png). by it self or be added into a project process. It Uses FFMPEG engine to create the animation without the need to index any file(s).
    Options for Gif:
    - Scale (Width x auto)
    - FrameRate (No Float points are Support, it must be a solid number like 60 and not 59.940.)
    - Statistics Mode
    - Diff Mode
    - Dither Options
    - Starting Time (in Seconds) - Supports Float Points like 25.2.
    - Length(in Seconds) - Supports Float points.
- Options For aPNG:
    - Starting Time (in Seconds) - Supports Float points.
    - Length(in Seconds) - Supports Float points.
    - FrameRate -  No Support for Float points.
    - Scale    (Width x auto)
    - Opt Settings (zlib,7zip & Zopfli) - zlib is the fastest with Zopfli being the slowest. 7zip is the balance between the two.

- Updated all Vapoursynth variable names for existing filters that have been changed or removed since 1.7.
- Tweaked the Job Loading function, When a previous job is loaded into StaxRip the form will now automatically close.
- Added Support for Remaining HDR10 colorspace Metadata, To make it full compatible with HDR10 Standards(MKV Only). The function can be enabled in the option section under Image -> Extras. It can also be done Manually The tool can be accessed by Apps -> MediaInfo -> MKVHDR
- Altered VUI import system to match MediaInfo changes. For HDR10 it must match BT.2020 Colorspace. It will now also enable the additional flags in VUI and Bitstream to encode to HDR10. GOP & VBV Settings will have to be set manually.
- Removed the Character Limit on the Temp Folders. It will not longer create the '...' after it hits 30 characters Which was designed for Windows 260 limit(Mainly for older Windows). If you need more then 260 characters you can alter the GroupPolicy Settings which will allow your to bypass the limit.
- Added MKVinfo App to menu to check colorspace coordinates (Which Mediainfo no longer displays), This is useful for HDR content to make sure metadate has the correct HDR metadata.
- MediaFolder Folder Option has been Disabled(It was no longer Working correctly due to latest update to Mediainfo) and has been replaced with the internal MediaInfo GUI that allows you to open any media file, If it's loaded in StaxRip or not.

- x265 Changes:
    - 12Bit Profiles Have Been Added    
    - VUI Menu has been Broken Up into two Menus now due to the Growing Number of options in the VUI section.
    - Added:
        - chromaloc
        - DHDR10-Info
        - atc-sei
        - pic-struct
        - display-window
        - opt-ref-list-length-pps
        - opt-qp-pps
        - single-sei
        - dynamic-refine
        - idr-recovery-sei
        - VBVinit, VBVend, VBV Adjustment with VBVinit being tweaked.
        - Maximum AU size Factor
        - refine-intra has been increased to Level 4
        - multi-pass-opt-analysis & multi-pass-opt-distortion have been moved to Rate Control 1 menu to make room for VBV options in Rate Control 2 Menu.
- VapourSynth Updated Context Menu:
    The List of all Included Filters:
- Source: 
    - AVISource
    - d2vsource
    - DGSource
    - ffms2
    - LibavSMASHSource
    - LWLibavSource
- FrameRate
    - AssumeFPS
    - InterFrame
    - SVPFlow
- Color
    - BitDepth
    - Matrix
    - Transfer
    - Primaries
    - Cube
    - Range
    - Tweak
    - Convert To
    - Format (Mainly Designed for HDR to SDR)
    - To 444
    - To RGB
    - To YUV
    - Dither Tools
- Line
    - MAA
    - DAA
    - Sangnom
    - TAAmbk
    - SeeSaw
    - FineSharp
    - MSharpen
    - aWarpSharpen2
    - LSFmod
    - pSharpen
    - SharpAAMcmod
- Resize
    - Resize
    - ReSample
    - Dither_Resize16
- Field
    - IVTC
    - QTGMC
    - nnedi3
    - Yadifmod
    - nnedi3cl
    - znedi3
    - Support Filters
- Noise
    - DFTTest
    - BM3D
    - HQDN3D
    - KNLMeansCL
    - MCTemporalDenoise
    - DegrainMedian
    - RemoveGrain
    - SMDegrain     
    - TTempSmooth
    - VagueDenoiser
- Misc
    - Anamorphic to Standard
    - UnSpec
    - Histogram
- Restoration
    - Deblock
    - Deblock PP7
    - Deblock_QED
    - MDeRing
    - MSmooth
    - abcxyz
    - BlindDeHalo3
    - DeHaloAlpha
    - EdgeCleaner
    - HQDering
    - YAHR
    - Vinverse
    - Vinverse 2

1.8.0.0

- All Filter are 64Bit enabled.
- Scripts and Code is designed for Avisynth+. Using older versions of Avisynth will unlikely work with everything.
- Restored the Old Icon and removed the RIP Icon.
- Added Support for Max CLL, MAX FALL and Master-Display for Nvidia H.265 Encoder.
- Added Support for Max CLL, MAX FALL and Master-Display for QSVEnc(Intel) H.265 Encoder.
- XviD has been Updated to 1.3.5
- Updated x265 to version 2.8+12(8+10+12 Bit Support)
- Add Support for AVX512 for x265 
- Updated FFMPEG to 4.0+
- Updated NVENCc to 4.07
- QSVEnc(Intel) has been Updated to 3.05
- MKVToolNix has been Updated to version 23.
- Updated MP4Box to 0.7.2
- eac3to has been Updated to 3.34
- AVSMeter has been Updated to 2.7.8
- fdkaac has been compiled for 64Bit
- Mediainfo has been Updated to 18.05
- FrameServer for VirtualDub2 and VirtualDub 64Bit works with Video. *Currently a small issue with Audio not decoding properly* (Files are Included)
- MPVnet has been Replaced with MPV. It uses the extact same code as MPVnet. MPVnet was replaced due to playback issues it was creating.
- Project X has been Removed
- Java has been Removed
- AV1 has been Removed for the Time Being (Encoder is unstable).
- mClean has been Updated to 3.2.
- awarpsharp2 has been Updated to 2.0.1
- FrameRateConventer Has been Updated to 1.2.1
- JPSDR has been Updated to 2.2.0.7
- MaskTools2 Has been Updated to 2.2.14
- MVTools2 has been Updated to 2.7.31
- QTGMC has been Updated to 3.358s
- TIVTC has been Updated to 1.0.11
- Added Color, FrameRate, Line, and Restoration context Menu.
The List of all Included Filters:
- Source: 
    - Automatic
    - AviSource
    - DirectShowSource
    - FFVideoSource 
    - Manual
    - MPEG2Source
    - DGSource
    - DGSourceIM*
    - DGSourceNV*
    - DSS2
    * = Not Freeware.
- Resizers: 
    - Dither_Resize16 
    - ResizeMT
    - Resize
    - Hardware Resize
    - JincResize
    - SuperResXBR
    - SuperRes
    - SuperXBR
    - Resize(Z) - Uses ZLib
- Crop: 
    - Dither_Crop16
    - Crop
    - Hardware Crop
- Line:
    - MCDAA3
    - DAA3Mod
    - MAA2Mod
    - XAA
    - HDRSharp
    - pSharpen
    - MSharpen
    - aWarpSharp2
    - FineSharp 
    - LSFmod (With More Syntax added for better results then Default)
- Misc:
    - MTMode (Multithreading)
    - SplitVertical (For MT) 
- Restoration: 
    - CNR2
    - Deblock    (Includes Dll for Both old and New versions, New version is enabled by Default).
    - Deblock_QED 
    - DehaloAlpha
    - FineDehalo
    - HQDeringmod
    - MipSmooth
    - SmoothD2 
- Color:
    - AutoAdjust
    - Histogram
    - ColorYUV (AutoGain)
    - Levels and the Correct color levels for each BitDepth.
    - Tweak
    - Convert(Format(ColorSpace), To(DataFormat), Bits(BitDepth)
    - Dither Tools
    - DFTTest (For Stack)
    - HDRCore
    - HDRColor*    
    - HDRNoise (Function must be Entered Manually to Use)
    - HDRMatrix
    - HDR Tone Mapping
    * = Not Freeware
- Framerate:
    - AssumeFPS
    - AssumeFPS_Source
    - InterFrame
    - ConvertFPS
    - ChangeFPS
    - SVPFlow (Scales to 59.940 and uses GPU by Default)
    - YFRC
- Field:
    - QTGMC (With additional Syntax for both Interlaced and Progressive Sources. Plus Repair option for bad interlaced sources (Includes Progressive)
    - nnedi3
    - IVTC
    - Assume (AssumeTFF & AssumeBFF)
    - EEDI3
    - FieldDeinterlace
    - yadifmod2
    - Select (SelectEven & SelectOdd)
- Noise:
    - KNLMeansCL (Added Device_type to Syntax)
    - DeNoiseMD + Histogram
    - DenoiseMF + Histogram
    - DFTTest
    - mClean
    - RemoveGrain16 
    - Repair16
    - Repair
    - HDRNoise (Syntax must be entered Manually)
    - RemoveGrain
    - FFT3DGPU 
    - HQDN3D
    - SpatialSoften (YUY2 Only)
    - TemporalSoften 
    - MCTemporalDenoise (Script has been Updated to work properly with GradFun2DB & GradFun2DBMod).
    - MCTemporalDenoisePP (Same as Above)
    - SMDGrain
    - DFTTest
    - TNLMeans 
    - VagueDenoiser 
    - xNLMeans)
Support Filters & Scripts:
    - AddGrainC
    - DeGrainMedian
    - GradFun2DB
    - GradFun2DBMod
    - HDRCore Source
    - AVSTP
    - TEMmod
    - MT Expand Multi
    - SmoothD2c
    - edi_rpow2
    - Average
    - nnedi3 rpow2
    - LUtils
    - TMM2
    - SVPFlow 1
    - SVPFlow 2
    - TTempSmooth
    - Depan
    - DepanEstimate
    - TemporalDegrain
    - AnimeIVTC
    - f3kdb
    - TComb
    - TDent
Altered:
    Tweak: Syntax has been alter for Adjusting saturation and includes both realcalc and dither_strength(Only Avisynth+ use these Functions).
    - RemoveGrain: added a secondary option for for artifact removal.    
    - KNLMeansCL: Added additional syntax for Device_type which is set to Auto. It will try to use GPU first before trying to use the CPU. 
     - Levels with correct range for each bitdepth. Level function does not autoscale based on the bitdepth.
    - added more modern Framerates to Assumefps, ConvertFPS and ChangeFPS. 144 & 240 rates have been added.
- Removed from Filter context Menu but can still works with the auto loading feature(Syntax must be Entered Manually): 
    - Clense (Part of JPSDR)
    - Checkmate
    - SangNom2
    - TComb
    - Undot
    - MedianBlurTemporal
    - FluxSmoothST
    - FluxSmoothT
    - MedianBlur
    - SangNom2
    - f3kdb
    - Vinverse 2

Filters and Software that Support AVX2: aWarpSharp2, DCTFilter, DFTTest, JPSDR, Masktools2, TMM2, TNLMeans, yadifmod2, x265
    - By default AVX or below are activate for compaiblity. 
- VapourSynth is untouched(No Changes).
