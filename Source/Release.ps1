$solution               = $PSScriptRoot + '\StaxRip.sln'
$binDirectory           = $PSScriptRoot + '\bin'
$appExe                 = $binDirectory + '\StaxRip.exe'
$7zDirectory            = ''
$7zExe                  = $7zDirectory + '7z.exe'
$msBuildDirectory       = ''
$msBuildExe             = $msBuildDirectory + 'MSBuild.exe'
$destinationDirectory   = 'A:\StaxRip-Releases'
$targetDirectory         = $destinationDirectory + '\StaxRip'   # is extended after solution is build
$includeProjectFiles    = @('*.config', '*.cpp', '*.h', '*.md', '*.ps1', '*.rc', '*.resx', '*.sln', '*.vb', '*.vbproj')
$excludeBinPatterns = @(
    '^\\[^\\]*Settings'
    '\\log\d+\.txt$',
    '.*recovery\.srip$',
    '.*\.log$',
    '.*(?<!eac3to)\.ini$',
    '.*help\.txt$',
    '.*\\eac3to\\log\.txt$',
    '.*\\qaac\\QTfiles.*',
    '.*\\FrameServer\.exp$',
    '.*\\FrameServer\.ilk$',
    '.*\\FrameServer\.lib$',
    '.*\\FrameServer\.pdb$',
    '.*\\ManagedCuda\.pdb$',
    '.*\\ManagedCuda\.xml$',
    '.*\\StaxRip\.vshost.*',
    '.*\\System\.Management\.Automation\.xml$',
    '.*_pycache_.*'
)
$excludeBinPatternsRegEx = '(?i)' + (($excludeBinPatterns | foreach {$_}) -join '|') + ''

#   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

$projectFiles = Get-ChildItem -Path $PSScriptRoot -Recurse -File -Include $includeProjectFiles
foreach( $file in $projectFiles ) {
    if( $file.FullName.Contains('\Apps\') ) { continue }
    if( $file.FullName.EndsWith('.vb') ) { continue }
    if( $file.FullName.EndsWith('.md') ) { continue }

    $lines = Get-Content $file

    foreach( $line in $lines ) {
        foreach( $char in $line.ToCharArray() ) {
            $codePoint = [int]$char

            if( $codePoint -gt 127 ) {
                throw "Non ASCII char '$char' in file '$($file.FullName)' in line: $line"
            }
        }
    }
}

#   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

& $msBuildExe "$solution" -t:Rebuild -p:Configuration=Release -p:Platform=x64
if( $LastExitCode ) { throw $LastExitCode }

#   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

$versionInfo = [Diagnostics.FileVersionInfo]::GetVersionInfo($appExe)
if( $versionInfo.FileBuildPart -ne 0 ) {
    $targetDirectory += "-v$($versionInfo.FileVersion)-x64"
}
else {
    $targetDirectory += "-v$($versionInfo.FileVersion)-x64"
}

Write-Host $versionInfo -ForegroundColor Green

#   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

if( Test-Path $targetDirectory ) { Remove-Item $targetDirectory -Recurse }

Get-ChildItem -Path $binDirectory -Recurse | where { $excludeBinPatterns -eq $null -or $_.FullName.Replace($binDirectory, '') -notmatch $excludeBinPatternsRegEx} | Copy-Item -Destination {
    if( $_.PSIsContainer ) {
        $dest = Join-Path $targetDirectory $_.Parent.FullName.Substring($binDirectory.length)
        $dest
    } 
    else {
        $dest = Join-Path $targetDirectory $_.FullName.Substring($binDirectory.length)
        $destFileInfo = [System.IO.FileInfo]$dest
        $destDirectoryName = $destFileInfo.DirectoryName
        if( -not (Test-Path $destDirectoryName) ) { New-Item -Path $destDirectoryName -ItemType Directory > $null }
        $dest
    }
} -Force

#   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

$targetFile = "$targetDirectory.7z"

if( Test-Path $targetFile ) { Remove-Item $targetFile -Recurse }

& $7zExe a -t7z -mx9 -m0=LZMA2 -md1024m -mfb256 -mmt3 -sse "$targetFile" -r "$targetDirectory"
if( $LastExitCode ) { throw $LastExitCode }

#   - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

Write-Host 'Successfully finished' -ForegroundColor Green
