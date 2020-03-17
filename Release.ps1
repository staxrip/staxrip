
$ErrorActionPreference = 'Stop'

$include = @(
    '*.config',
    '*.cpp',
    '*.h',
    '*.md',
    '*.ps1',
    '*.rc',
    '*.resx',
    '*.sln',
    '*.vb',
    '*.vbproj'
)

$files = Get-ChildItem -Path $PSScriptRoot -Recurse -File -Include $include

foreach ($file in $files)
{
    if ($file.FullName.Contains('\Apps\'))
    {
        continue
    }

    $lines = Get-Content $file

    foreach ($line in $lines)
    {
        foreach ($char in $line.ToCharArray())
        {
            $codePoint = [int]$char

            if ($codePoint -gt 127)
            {
                throw "Non ASCII char $char in file '$($file.FullName)' in line: $line"
            }
        }
    }
}

$exeFile     = $PSScriptRoot + '\bin\StaxRip.exe'
$versionInfo = [Diagnostics.FileVersionInfo]::GetVersionInfo($exeFile)
$vsDir       = 'C:\Program Files (x86)\Microsoft Visual Studio\2019'
$msBuild     = $vsDir + '\Community\MSBuild\Current\Bin\MSBuild.exe'

& $msBuild ($PSScriptRoot + '\StaxRip.sln') -t:Rebuild -p:Configuration=Release -p:Platform=x64

if ($LastExitCode)
{
    throw $LastExitCode
}

if ($versionInfo.FilePrivatePart -gt 9)
{
    $releaseType = 'beta-without-apps'
}
elseif ($versionInfo.FilePrivatePart -ne 0)
{
    $releaseType = 'beta'
}
else
{
    $releaseType = 'stable'
}

$desktopDir  = [Environment]::GetFolderPath('Desktop')
$targetDir   = $desktopDir + '\StaxRip-x64-' + $versionInfo.FileVersion + '-' + $releaseType

if (Test-Path $targetDir)
{
    Remove-Item $targetDir -Recurse
}

Copy-Item ($PSScriptRoot + '\bin') $targetDir -Recurse

$patterns = @(
    '*\_StaxRip.log',
    '*\AVSMeter.ini',
    '*\chapterEditor.ini',
    '*\Debug.log',
    '*\DGIndex.ini',
    '*\eac3to\log.txt',
    '*\FrameServer.exp',
    '*\FrameServer.ilk',
    '*\FrameServer.lib',
    '*\FrameServer.pdb',
    '*\mkvtoolnix.ini',
    '*\mkvtoolnix-gui.ini',
    '*\StaxRip.vshost.exe',
    '*\StaxRip.vshost.exe.config',
    '*\StaxRip.vshost.exe.manifest',
    '*\StaxRip.vshost.sln',
    '*_pycache_*'
)

foreach ($item in (Get-ChildItem $targetDir -Recurse))
{
    foreach ($pattern in $patterns)
    {
        if ($item.FullName -like $pattern -and (Test-Path $item.FullName))
        {
            Write-Host delete $item.FullName
            Remove-Item $item.FullName -Recurse
        }
    }
}

Get-ChildItem $targetDir -Filter *.ini -Recurse | foreach { throw $_ }

if ($versionInfo.FilePrivatePart -gt 9)
{
    Remove-Item ($targetDir + '\Apps') -Recurse
}

$7z = 'C:\Program Files\7-Zip\7z.exe'
& $7z a -t7z -mx9 "$targetDir.7z" -r "$targetDir\*"

if ($LastExitCode)
{
    throw $LastExitCode
}

if ($releaseType -eq 'beta' -or $releaseType -eq 'beta-without-apps')
{
    if (Test-Path 'D:\Projekte\VB\StaxRip')
    {
        $outputDirectories = 'C:\Users\frank\Dropbox\public\StaxRip\Builds',
                             'C:\Users\frank\OneDrive\StaxRip\Builds'

        foreach ($outputDirectory in $outputDirectories)
        {
            if (-not (Test-Path $outputDirectory))
            {
                throw $outputDirectory
            }

            $targetFile = $outputDirectory + '\' + (Split-Path "$targetDir.7z" -Leaf)

            if (Test-Path $targetFile)
            {
                throw $targetFile
            }

            Copy-Item "$targetDir.7z" $targetFile
            Invoke-Item $outputDirectory
        }
    }
}

Set-Clipboard ($versionInfo.FileVersion + "`n`n" +
    'https://github.com/staxrip/staxrip/blob/master/Changelog.md' + "`n`n" +
    'https://staxrip.readthedocs.io/intro.html#download')

Write-Host 'successfully finished' -ForegroundColor Green
