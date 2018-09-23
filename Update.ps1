function Check-7z {
    $7zdir = (Get-Location).Path + "\7z"
    if (-not (Test-Path ($7zdir + "\7za.exe")))
    {
        $download_file = (Get-Location).Path + "\7z.zip"
        Write-Host "Downloading 7z" -ForegroundColor Green
        Invoke-WebRequest -Uri "http://download.sourceforge.net/sevenzip/7za920.zip" -UserAgent [Microsoft.PowerShell.Commands.PSUserAgent]::FireFox -OutFile $download_file
        Write-Host "Extracting 7z" -ForegroundColor Green
        Add-Type -AssemblyName System.IO.Compression.FileSystem
        [System.IO.Compression.ZipFile]::ExtractToDirectory($download_file, $7zdir)
        Remove-Item -Force $download_file
    }
    else
    {
        Write-Host "7z already exist. Skipped download" -ForegroundColor Green
    }
}

function Obtain_Path{
$staxrip_path = (get-location).Path + "\StaxRip.exe"
return $staxrip_path
}

function Close_Application{
Get-Process StaxRip
Stop-Process -Name "StaxRip"
#get-process StaxRip | %{ $_.closemainwindow() Clean Exit Only.  }
}

function Start_Application($Path) {
Start-Process $Path 
}

function Clean_Up {
    $Path = Obtain_Path
    $Replace = $Path -replace "StaxRip.exe", "StaxRip.7z" 
    $Replace2 = $Path -replace "StaxRip.exe", "7z" 
	$Replace3 = $Path -replace "StaxRip.exe", "Update.bat"
    Remove-Item $Replace
    Remove-Item $Replace2
	Remove-Item $Replace3
}

function Clean_Up_Bat_File {
    $Path = Obtain_Path   
	$Replace = $Path -replace "StaxRip.exe", "Update.bat"
    Remove-Item $Replace
}

function Check-PowershellVersion {
    $version = $PSVersionTable.PSVersion.Major
    Write-Host "Checking Windows PowerShell version -- $version" -ForegroundColor Green
    if ($version -le 2)
    {
        Write-Host "Using Windows PowerShell $version is unsupported. Upgrade your Windows PowerShell." -ForegroundColor Red
        throw
    }
}

function Check-staxrip {
    $staxrip = (get-location).Path + "\StaxRip.exe"
    $is_exist = Test-Path $staxrip
    return $is_exist
}

function Extract-staxrip  {    
    $Links = Invoke-WebRequest -Uri "https://github.com/Revan654/staxrip/releases/latest" –UseBasicParsing -UserAgent [Microsoft.PowerShell.Commands.PSUserAgent]::FireFox
    $pattern = "([0-9].[0-9].[0-9].[0-9]).x64.zip"    
    $bool = $Links -match $pattern
    $FileName = $matches[1] + ".x64.7z"
    $7za = (Get-Location).Path + "\7z\7za.exe"
    Write-Host "Extracting" $FileName -ForegroundColor Green
    & $7za x -y $FileName
}

function Download-staxrip {
    Write-Host "Downloading StaxRip" -ForegroundColor Green
    $Links = Invoke-WebRequest -Uri "https://github.com/Revan654/staxrip/releases/latest" –UseBasicParsing -UserAgent [Microsoft.PowerShell.Commands.PSUserAgent]::FireFox
    $pattern = "([0-9].[0-9].[0-9].[0-9]).x64.zip"    
    $bool = $Links -match $pattern
    $Build = "https://github.com/Revan654/staxrip/releases/download/" + $matches[1] + "/" + $matches[1] + ".x64.zip"
    Invoke-WebRequest -Uri $Build -UserAgent [Microsoft.PowerShell.Commands.PSUserAgent]::"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:62.0) Gecko/20100101 Firefox/62.0" -OutFile "Staxrip.7z"
}

function Get-Latest-staxrip {  
    $Links = Invoke-WebRequest -Uri "https://github.com/Revan654/staxrip/releases/latest" –UseBasicParsing -UserAgent [Microsoft.PowerShell.Commands.PSUserAgent]::FireFox
    $pattern = "([0-9].[0-9].[0-9].[0-9]).x64.zip"    
    $bool = $Links -match $pattern
    $Build = "https://github.com/Revan654/staxrip/releases/download/" + $matches[1] + "/" + $matches[1] + ".x64.zip"
    return $Build
}

function Get-Arch { #Not needed at This Time
    # Reference: http://superuser.com/a/891443
    $FilePath = [System.IO.Path]::Combine((Get-Location).Path, 'StaxRip.exe')
    [int32]$MACHINE_OFFSET = 4
    [int32]$PE_POINTER_OFFSET = 60

    [byte[]]$data = New-Object -TypeName System.Byte[] -ArgumentList 4096
    $stream = New-Object -TypeName System.IO.FileStream -ArgumentList ($FilePath, 'Open', 'Read')
    $stream.Read($data, 0, 4096) | Out-Null

    # DOS header is 64 bytes, last element, long (4 bytes) is the address of the PE header
    [int32]$PE_HEADER_ADDR = [System.BitConverter]::ToInt32($data, $PE_POINTER_OFFSET)
    [int32]$machineUint = [System.BitConverter]::ToUInt16($data, $PE_HEADER_ADDR + $MACHINE_OFFSET)

    $result = "" | select FilePath, FileType
    $result.FilePath = $FilePath

    switch ($machineUint)
    {
        0      { $result.FileType = 'Native' }
        0x014c { $result.FileType = 'i686' } # 32bit
        0x0200 { $result.FileType = 'Itanium' }
        0x8664 { $result.FileType = 'x86_64' } # 64bit
    }

    $result
}

function ExtractVersionFromFile {    
    $Version = (Get-ItemProperty ./staxrip.exe).VersionInfo.FileVersion
    return "$Version"
}

function ExtractVersionFromURL {
    $filename = Get-Latest-staxrip
    $pattern = "([0-9].[0-9].[0-9].[0-9]).x64"    
    $bool = $filename -match $pattern
    return $matches[1]
}

function Test-Admin
{
    $user = [Security.Principal.WindowsIdentity]::GetCurrent();
    (New-Object Security.Principal.WindowsPrincipal $user).IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)
}

function Upgrade-staxrip {
    $need_download = $false    
    Write-Host "Fetching URL Data for StaxRip Builds" -ForegroundColor Green                  
    $localVersion = ExtractVersionFromFile
    $remoteVersion = ExtractVersionFromURL 
    if ((ExtractVersionFromFile) -ge (ExtractVersionFromURL)) {
        Write-Host "You are Already Using Latest Build." -ForegroundColor Red
        Write-Host "Current Build: $remoteVersion" -ForegroundColor Red
            $need_download = $false  
				Clean_Up_Bat_File }    
    else {
        Write-Host "There is a Newer Build Available" -ForegroundColor Green
        Write-Host "New Build: $remoteVersion" -ForegroundColor Green
            $need_download = $true
          }      
    
    if ($need_download) {
        Download-staxrip
        Check-7z        
        Extract-staxrip -Wait   
        Clean_Up     
    }

}

function Read-KeyOrTimeout ($prompt, $key){
    $seconds = 9
    $startTime = Get-Date
    $timeOut = New-TimeSpan -Seconds $seconds

    Write-Host "$prompt " -ForegroundColor Green

    # Basic progress bar
    [Console]::CursorLeft = 0
    [Console]::Write("[")
    [Console]::CursorLeft = $seconds + 2
    [Console]::Write("]")
    [Console]::CursorLeft = 1

    while (-not [System.Console]::KeyAvailable) {
        $currentTime = Get-Date
        Start-Sleep -s 1
        Write-Host "#" -ForegroundColor Green -NoNewline
        if ($currentTime -gt $startTime + $timeOut) {
            Break
        }
    }
    if ([System.Console]::KeyAvailable) {
        $response = [System.Console]::ReadKey($true).Key
    }
    else {
        $response = $key
    }
    return $response.ToString()
}

#
# Main script entry point
#
if (Test-Admin) {
    Write-Host "Running Script with Administrator Privileges" -ForegroundColor Yellow
}
else {
    Write-Host "Running Script Without Administrator Privileges" -ForegroundColor Red
}

try {
    Check-PowershellVersion
    # Sourceforge only support TLS 1.2
    [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
    $Path = Obtain_Path
    Close_Application
    Upgrade-staxrip    
    Start_Application $Path
    Write-Host "Operation Complete" -ForegroundColor Magenta
}
catch [System.Exception] {
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
