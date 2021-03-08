
$ErrorActionPreference = 'Stop'

Register-ObjectEvent -InputObject ([ShortcutModule]::g) -EventName AfterSourceLoaded -Action {
    $activeProject = [ShortcutModule]::p
    $scanType = [MediaInfo]::GetVideo($activeProject.FirstOriginalSourceFile, 'ScanType')

    if ($scanType -eq 'Interlaced')
    {
        $activeProject.Script.SetFilter('Field', 'QTGMC Interlaced', 'QTGMC(Preset = "Medium", InputType=0)')
    }
    elseif ($scanType -eq 'Progressive')
    {
        $activeProject.Script.SetFilter('Field', 'QTGMC Progressive', 'QTGMC(Preset = "Medium", InputType=1)')
    }
}
