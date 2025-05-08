# [Documentation](../README.md) / [Introduction](README.md) / Installation

StaxRip is a portable application, meaning that it has no installer.
You only need to extract StaxRip's folder from the downloaded archive, using [7-zip](https://www.7-zip.org) or
any archiver compatible with the 7-zip `7z` format. 

> :warning: For the destination you should choose a location where you,
as the user who executes StaxRip, have write permissions to avoid issues with some tools.

Once the folder is extracted, you are ready to go and start `StaxRip.exe` from inside the folder you have extracted.

StaxRip includes portable versions of AviSynth+, VapourSynth, and Python.
Therefore, installation of these tools is not required.

At this point you are done with the installation and ready for the [First Start](../Usage/First-Start.md).


> [!CAUTION]
> **Windows 7** users can use StaxRip only partly. The following tools are included and don't have
official **Windows 7** support anymore. In case you don't want to upgrade your system,
you can replace these tools with an older, compatible, version, which should work, but of course with some limited usability:
> - **StaxRip** itself
    - The *Blu-ray ISO image opening and mounting feature* requires at least **Windows 8** to work.
> - **MKVToolNix**
    - Latest working version is reported to be `v64.0`. Last (complete) working StaxRip version is
        *StaxRip v2.10.0 (2021-10-06)* including *MKVToolNix v61.0*
    - Nevertheless `mkvtoolnix-64-bit-68.0.0-revision-001-g6a55c58d2` is reported to work,
        you can download it here: https://mkvtoolnix.download/windows/continuous/64-bit/68.0.0/
> - **Python**
    - Needed for VapourSynth. Last **Windows 7** compatible version was used in *StaxRip v2.25.0 (2023-08-02)*.
        As of now using VapourSynth R71 it could be possible to downgrade Python to `v3.8.*`
        that is **Windows 7** compatible, but requires experienced users.
> 
> Alternatively you can download an old StaxRip release, but then you don't benefit from new functions and bug fixes.
> 
> :warning: **Note:** In neither case this is supported in any way.
