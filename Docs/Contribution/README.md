[ :leftwards_arrow_with_hook: Back to Documentation](../README.md)

# Contributing

Contributions should be small, focused, and easy to verify. Search existing issues and pull requests before starting substantial work. Open an issue first when a change affects project compatibility, external-tool behavior, persisted settings, or release packaging.

## Build scope

The procedure below was verified for StaxRip `v2.52.5` on Windows with the `x64` platform. StaxRip supports x64 builds only, so this guide does not cover the x86 configurations that remain in the project files. It produces a source build. It does not recreate the complete portable distribution because the repository does not contain the full ignored `Source/bin/Apps` runtime tree.

The verification used:

- Visual Studio Build Tools 2022 with MSBuild 17.14;
- MSVC v143 and Windows SDK 10.0.26100.0;
- .NET Framework 4.8 reference assemblies;
- the three package versions in `Source/packages.config`;
- the three VapourSynth API headers from the official VapourSynth R79 source tag.

Other compatible Visual Studio 2022 and Windows SDK revisions may work, but they were not part of this check.

## Requirements

Install Git and either Visual Studio 2022 or Visual Studio Build Tools 2022 with these components:

- .NET desktop build tools;
- .NET Framework 4.8 SDK or developer pack;
- Desktop development with C++;
- MSVC v143 C++ build tools;
- a Windows 10 or Windows 11 SDK.

Run the following commands from a Developer PowerShell for Visual Studio so `msbuild` is available.

## Restore the managed packages

From the StaxRip repository root, restore the exact versions declared in `Source/packages.config`:

```powershell
msbuild Source\StaxRip.sln -t:Restore -p:RestorePackagesConfig=true -p:RestoreSources=https://api.nuget.org/v3/index.json
```

The packages are restored into the ignored `Source/packages` directory. If the build reports missing DirectN or ManagedCuda types, confirm that this directory contains the three declared package folders.

## Stage the VapourSynth headers

`FrameServer.vcxproj` expects `VSScript4.h`, `VSHelper4.h`, and `VapourSynth4.h` under the ignored `Source/bin` tree. The repository does not currently pin a VapourSynth source tag. R79 was used for the `v2.52.5` verification.

Clone the official source and copy only those headers:

```powershell
$vapourSynthSource = Join-Path (Split-Path (Get-Location) -Parent) 'vapoursynth-R79'
git clone --depth 1 --branch R79 https://github.com/vapoursynth/vapoursynth.git $vapourSynthSource

$vapourSynthInclude = Join-Path $vapourSynthSource 'include'
$staxRipInclude = 'Source\bin\Apps\FrameServer\VapourSynth\sdk\include\vapoursynth'
New-Item -ItemType Directory -Force -Path $staxRipInclude | Out-Null

Copy-Item (Join-Path $vapourSynthInclude 'VSScript4.h') $staxRipInclude
Copy-Item (Join-Path $vapourSynthInclude 'VSHelper4.h') $staxRipInclude
Copy-Item (Join-Path $vapourSynthInclude 'VapourSynth4.h') $staxRipInclude
```

If `Source/bin` is removed, stage the headers again before rebuilding FrameServer.

## Build the main solution

For normal development, rebuild the `Debug|x64` solution:

```powershell
msbuild Source\StaxRip.sln -t:Rebuild -p:Configuration=Debug -p:Platform=x64 -m
```

This builds `StaxRip.exe` and `FrameServer.dll` into the ignored `Source/bin` directory.

## Build AutoCrop

AutoCrop has a separate solution and is not built by `Source/StaxRip.sln`:

```powershell
msbuild Source\Tools\AutoCrop\AutoCrop.sln -t:Rebuild -p:Configuration=Debug -p:Platform=x64 -m
```

The Debug x64 build writes `AutoCrop.exe` to `Source/bin/Apps/Support/AutoCrop`. Its Release x64 configuration instead writes to `Source/Tools/AutoCrop/bin/x64/Release`.

The Release output directory contains tracked binaries. A Release rebuild modifies `AutoCrop.exe` and `AutoCrop.pdb`. Check `git status` afterward and do not commit those binary changes unless the contribution intentionally updates them.

## Configuration behavior

The solution mappings are significant:

| Solution configuration | StaxRip project | FrameServer project |
|---|---|---|
| `Debug|x64` | `Debug|x64` | `Debug|x64` |
| `Release|x64` | `Debug|x64` | `Release|x64` |

Therefore, a solution-level Release build does not use the StaxRip project's Release configuration. The StaxRip Debug configuration defines `DEBUG` and `TRACE` and emits full debug information. The FrameServer Release configuration uses optimized native compilation and `NDEBUG`.

Do not change this mapping as part of an unrelated contribution. Its maintainer intent and release contract should be established separately.

## Runtime limitation

A successful compile does not make `Source/bin` a complete runnable StaxRip distribution. Encoders, frame-server runtimes, plugins, Python, and other applications normally shipped under `Apps` are external to the tracked source tree. Do not report a source build as runtime-verified unless the exact prepared Apps tree and exercised workflow are also recorded.

## Pull requests

Before opening a pull request:

1. Rebase or merge the current upstream `master` branch.
2. Keep the branch limited to one change or one closely related set of changes.
3. Run the smallest relevant build or check and record its exact configuration and platform.
4. State what was not tested, especially runtime tools, packaging, and release behavior.
5. Do not include build outputs, restored packages, personal settings, projects, media, or logs. Check for modified tracked AutoCrop binaries after a Release build.
6. Explain changes to command generation, persisted data, tool selection, cleanup, concurrency, or native interfaces in the pull-request description.

Do not run or modify `Source/BuildAndPack.ps1` or `Source/Release.ps1` unless the contribution specifically targets release tooling and the maintainer has agreed on the scope.
