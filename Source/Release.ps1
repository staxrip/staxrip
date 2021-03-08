
$output32bit = $false

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

if ($output32bit) {
    & $msBuild ($PSScriptRoot + '\StaxRip.sln') -t:Rebuild -p:Configuration=Release -p:Platform=x86

    if ($LastExitCode)
    {
        throw $LastExitCode
    }
}

& $msBuild ($PSScriptRoot + '\StaxRip.sln') -t:Rebuild -p:Configuration=Release -p:Platform=x64

if ($LastExitCode)
{
    throw $LastExitCode
}

if ($versionInfo.FilePrivatePart -gt 9)
{
    $releaseType = 'beta-without-apps'
    $downloadURL = 'https://staxrip.readthedocs.io/introduction.html#beta'
}
elseif ($versionInfo.FilePrivatePart -ne 0)
{
    $releaseType = 'Beta'
    $downloadURL = 'https://staxrip.readthedocs.io/introduction.html#beta'
}
else
{
    $releaseType = 'Stable'
    $downloadURL = 'https://staxrip.readthedocs.io/introduction.html#stable'
}

$tempDir = 'D:\Work'

if (-not (Test-Path $tempDir))
{
    $tempDir = [Environment]::GetFolderPath('Desktop')
}

$targetDir     = $tempDir + '\StaxRip-x64-' + $versionInfo.FileVersion + '-' + $releaseType
$targetDir32   = $tempDir + '\StaxRip-x86-' + $versionInfo.FileVersion + '-' + $releaseType + "-Experimental"

if (Test-Path $targetDir)
{
    Remove-Item $targetDir -Recurse
}

if (Test-Path $targetDir32)
{
    Remove-Item $targetDir32 -Recurse
}

Copy-Item ($PSScriptRoot + '\bin') $targetDir -Recurse

if ($output32bit) {
    Copy-Item ($PSScriptRoot + '\bin-x86') $targetDir32 -Recurse
}

$patterns = @(
    '*\_StaxRip.log',
    '*\AVSMeter.ini',
    '*\AVSMeter64.ini',
    '*\chapterEditor.ini',
    '*\d2vwitch.ini'
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

foreach ($dir in $targetDir, $targetDir32)
{
    foreach ($item in (Get-ChildItem $dir -Recurse))
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
}

Get-ChildItem $targetDir   -Filter *.ini -Recurse | foreach { throw $_ }
Get-ChildItem $targetDir32 -Filter *.ini -Recurse | foreach { throw $_ }

if ($versionInfo.FilePrivatePart -gt 9)
{
    Remove-Item ($targetDir   + '\Apps') -Recurse
    Remove-Item ($targetDir32 + '\Apps') -Recurse
}

$7z = 'C:\Program Files\7-Zip\7z.exe'
& $7z a -t7z -mx9 "$targetDir.7z" -r "$targetDir\*"

if ($LastExitCode)
{
    throw $LastExitCode
}

if ($output32bit) {
    & $7z a -t7z -mx9 "$targetDir32.7z" -r "$targetDir32\*"

    if ($LastExitCode)
    {
        throw $LastExitCode
    }
}

if ($releaseType -eq 'Beta' -or $releaseType -eq 'beta-without-apps')
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

            $targetFile   = $outputDirectory + '\' + (Split-Path "$targetDir.7z"   -Leaf)
            $targetFile32 = $outputDirectory + '\' + (Split-Path "$targetDir32.7z" -Leaf)

            if (Test-Path $targetFile)
            {
                throw $targetFile
            }

            if ($output32bit) {
                if (Test-Path $targetFile32) {
                    throw $targetFile32
                }
            }

            Copy-Item "$targetDir.7z"   $targetFile

            if ($output32bit) {
                Copy-Item "$targetDir32.7z" $targetFile32
            }

            Invoke-Item $outputDirectory
        }
    }
}

Set-Clipboard ('[B]' + $versionInfo.FileVersion + $releaseType + "[/B]`n`n`nChangelog`n`n" +
    'https://github.com/staxrip/staxrip/blob/master/Changelog.md' +
    "`n`n`nDownload`n`n" + $downloadURL)

Write-Host 'successfully finished' -ForegroundColor Green
