$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
$net10 = $lines | Where-Object { $_ -match 'net10\.0' -and $_ -match ': warning CS' }
$fileWarnings = @{}
foreach ($line in $net10) {
    if ($line -match '^(.+?)\(\d+,\d+\): warning') {
        $file = $Matches[1]
        if ($fileWarnings.ContainsKey($file)) { $fileWarnings[$file]++ } else { $fileWarnings[$file] = 1 }
    }
}
$sorted = $fileWarnings.GetEnumerator() | Sort-Object Value -Descending | Select-Object -First 30
foreach ($entry in $sorted) {
    Write-Host "$($entry.Value) $($entry.Key)"
}
