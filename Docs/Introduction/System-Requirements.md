## [Documentation](../README.md) / [Introduction](README.md) / System Requirements

### Supported OS

StaxRip only supports 64-bit versions of the following Windows operating systems:

- Windows 11 (x64)
- Windows 10 (x64)
- Windows 8.1 (x64)
- Windows 8 (x64)
- Windows 7 (x64) [***partly*** (see below)]

> :warning: **Note:** 32-bit versions are **not** supported.

#### OS limitations
**Windows 7** users can use StaxRip only partly. The following tools are included and don't have
official **Windows 7** support anymore. In case you don't want to upgrade your system,
you can replace these tools with an older, compatible, version, which should work, but of course with some limited usability:
- **StaxRip** itself
    - The *Blu-ray ISO image opening and mounting feature* requires at least **Windows 8** to work.
- **MKVToolNix**
- **Python**

For more information see: [Installation](Installation.md)
             
Alternatively you can download an old StaxRip release, but then you don't benefit from new functions and bug fixes.

> :warning: **Note:** In neither case this is supported in any way.

#### Hardware limitations

To improve the performance on almost modern CPUs, some encoders and plugins are built for CPUs that support the
AVX2 instruction set: However, StaxRip can be manually backwards-extended to non-AVX2 processors with a workaround.  
Depending on your CPU, you even might want to replace the AVX2 build with an AVX512 or more specific build that uses
all capacities of your CPU to make it even more efficient, means faster.

Hardware encoding, for example with `NVEncC`, works only on hardware, that supports the encoding for certain codecs.


### Dependencies and Runtimes
As StaxRip is as GUI application, that executes different tools and plugins, the requirements are divided into 2 categories:

- StaxRip itself, which just needs the `Microsoft .NET Framework v4.8.1`:
    - https://go.microsoft.com/fwlink/?LinkId=2085155
- Tools and filters, that have their individual requirements.
    Most of them need a more or less up to date version of `Microsoft Visual C++ Redistributable Runtimes`.
    Due to the frequent changing and replacing of tools and filters, it is impossible to provide precise details
    about the right dependency. So if StaxRip doesn't start or you get error messages due to missing runtime files or
    dependencies, we recommend to download and install the `Microsoft Visual C++ Redistributable Runtimes` from:
    - https://github.com/abbodi1406/vcredist/releases or
    - https://www.techpowerup.com/download/visual-c-redistributable-runtime-package-all-in-one

When it comes to hardware encoding with your GPU, you should make sure, that your drivers are up to date as
some settings might need them to work properly.

