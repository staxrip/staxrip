@echo off
setlocal disabledelayedexpansion

for /F "tokens=1,2*" %%i in ('reg query HKLM\Software\7-Zip /v Path') do (
    set "PATH=%%k;%PATH%"
)

7z >NUL
if not %errorlevel% == 0 (
    echo 7z.exe is required
    goto end
)

if not "%~1" == "" (
    set "installer=%~1"
) else if exist iTunes6464Setup.exe (
    set installer=iTunes6464Setup.exe
) else if exist iTunes64Setup.exe (
    set installer=iTunes64Setup.exe
) else if exist iTunesSetup.exe (
    set installer=iTunesSetup.exe
) else if exist QuickTimeInstaller.exe (
    set installer=QuickTimeInstaller.exe
) else (
    echo installer executable not found
    goto end
)

7z e -y "%installer%" AppleApplicationSupport.msi
7z e -y "%installer%" AppleApplicationSupport64.msi

if exist AppleApplicationSupport.msi (
    call :extract QTfiles AppleApplicationSupport.msi
) else (
    echo cannot extract AppleApplicationSupport.msi from installer
    goto end
)
if exist AppleApplicationSupport64.msi (
    call :extract QTfiles64 AppleApplicationSupport64.msi
)
goto end

:extract
mkdir %1

7z l -i!CoreAudioToolbox.* %2 | findstr CoreAudioToolbox

if errorlevel 1 (
    7z e -y -o%1 ^
             -i!*AppleApplicationSupport_ASL.dll ^
             -i!*AppleApplicationSupport_CoreAudioToolbox.dll ^
             -i!*AppleApplicationSupport_CoreFoundation.dll ^
             -i!*AppleApplicationSupport_icudt*.dll ^
             -i!*AppleApplicationSupport_libdispatch.dll ^
             -i!*AppleApplicationSupport_libicu*.dll ^
             -i!*AppleApplicationSupport_objc.dll ^
             -i!F_CENTRAL_msvc?100* ^
             %2

    pushd %1
    for %%f in (AppleApplicationSupport_*.dll) do (
        for /F "tokens=1* delims=_ " %%t in ("%%f") do move /Y %%f %%u
    )
    for %%f in (x64_AppleApplicationSupport_*.dll) do (
        for /F "tokens=2* delims=_ " %%t in ("%%f") do move /Y %%f %%u
    )

    for %%f in (F_CENTRAL_msvcr100*) do move /Y %%f msvcr100.dll
    for %%f in (F_CENTRAL_msvcp100*) do move /Y %%f msvcp100.dll
    popd
) else (
    7z e -y -o%1 ^
             -i!ASL.dll ^
             -i!CoreAudioToolbox.dll ^
             -i!CoreFoundation.dll ^
             -i!*icu*.dll ^
             -i!libdispatch.dll ^
             -i!objc.dll ^
             -i!pthreadVC2.dll ^
             %2

    mkdir %1\Microsoft.VC80.CRT

    7z e -y -oQTfiles\Microsoft.VC80.CRT ^
            -i!msvcp80.dll.* ^
            -i!msvcr80.dll.* ^
            -i!manifest.* ^
            AppleApplicationSupport.msi

    pushd %1\Microsoft.VC80.CRT

    rem strip assembly version number from filenames of msvc runtime dlls
    for %%f in (msvcr80.dll.*) do move /Y %%f msvcr80.dll
    for %%f in (msvcp80.dll.*) do move /Y %%f msvcp80.dll

    rem find needless one out of the two manifests and remove it
    for /F "delims=:" %%t in ('findstr win32-policy manifest.*') do del %%t

    rem rename manifest
    for %%f in (manifest.*) do move /Y %%f Microsoft.VC80.CRT.manifest

    popd
)
exit /b

:end
if exist AppleApplicationSupport.msi del AppleApplicationSupport.msi
if exist AppleApplicationSupport64.msi del AppleApplicationSupport64.msi
endlocal
