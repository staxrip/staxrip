
$activeProject = [ShortcutModule]::p

foreach ($audio in $activeProject.Audio0, $activeProject.Audio1)
{
    if ($audio.GetType().Name -eq 'GUIAudioProfile' -and
        $audio.File.EndsWith('flac') -and
        $audio.GetEncoder() -eq 'qaac')
    {
        $audio.DecodingMode = 'Pipe'
    }
}
