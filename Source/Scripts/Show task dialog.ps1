
$td = New-Object TaskDialog[string]
$td.MainInstruction = 'Select a option.'
$td.AddCommand('Option A', 'aaa')
$td.AddCommand('Option B' , 'bbb')
$result = $td.Show()
$td.Dispose()

if ($result -eq 'aaa') {
    [MainModule]::MsgInfo('Option A was selected.')
} elseif ($result -eq 'bbb') {
    [MainModule]::MsgInfo('Option B was selected.')
} else {
    [MainModule]::MsgInfo('No option was selected.')
}
