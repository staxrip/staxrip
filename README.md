# StaxRip x64

Multiformat video encoding application.

# Features

- Clean user interface with High DPI support
- Context sensitive and dynamic help
- Handles almost any input format and allows to config how files are loaded and processed
- x264, x265, AMD VCE H.264 encoding, Intel Quick Sync H.264/H.265/MPEG-2 encoding, NVIDIA H.264/H.265 encoding, VP9, XVID
- Output Formats: AVI, MP4, MKV, WEBM
- Output Codecs: XVID, H.264, H.265, VP9, MP3, AC3, AAC, VORBIS, DTS, FLAC, OPUS
- Batch Processing including support for processing sub-directories recursively
- Powerful AviSynth and VapourSynth editor
- Video comparison tool for making it very easy to compare codec and filter options
- Searchable MediaInfo dialog
- MediaInfo folder view showing a table with MediaInfo of a complete folder
- Minimal user interaction required due to rich configuration and automation features
- Full manual control over important processing steps such as a convenient dialog for demuxing and choosing one of many available source filters
- copy modes for audio and video for plain remuxing
- Cut/Trim/Edit feature
- A large amount of macros can be used in scrips and command lines allowing to change all settings with the GUI at any time which is much more convenient then a serialized step by step approach
- Scripable with C# and PowerShell scripts
- External AviSynth filters and command line tools can be integrated
- Rich project, project template and profiles support
- Job processing

# Requirements

* [.NET 4.6.1](https://www.microsoft.com/en-us/download/details.aspx?id=49981)
* [Intel Skylake CPU](https://en.wikipedia.org/wiki/Skylake_%28microarchitecture%29) for HEVC/H.265 hardware encoding
* [NVIDIA Maxwell gen2 card](https://en.wikipedia.org/wiki/Maxwell_%28microarchitecture%29#Second_generation_Maxwell_.28GM20x.29) for HEVC/H.265 hardware encoding
* [Java](https://java.com/en/download) for ProjectX in case MPEG-2 files are opened
* AviSynth+ x64, the installer is bundled with StaxRip x64
* [VapourSynth x64](https://github.com/vapoursynth/vapoursynth/releases) as optional AviSynth+ alternative
* [Python x64](https://www.python.org/downloads/windows) in case VapourSynth x64 is used

# Documentation

https://stax76.gitbooks.io/staxrip-handbook/content/

# Download

https://github.com/stax76/staxrip/releases

https://github.com/stax76/staxrip/blob/master/md/test-build.md