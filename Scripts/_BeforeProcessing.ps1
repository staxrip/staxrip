
# If the audio source file extension is flac and the
# audio encoder is qaac it sets decoding mode to Pipe.

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
