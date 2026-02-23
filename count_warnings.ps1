$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
$net10 = $lines | Where-Object { $_ -match 'net10\.0' -and $_ -match ': warning CS' }
Write-Host "net10.0 warnings: $($net10.Count)"
$grouped = $net10 | ForEach-Object { if ($_ -match 'warning (CS\d+)') { $Matches[1] } } | Group-Object | Sort-Object Count -Descending
foreach ($g in $grouped) { Write-Host "$($g.Name): $($g.Count)" }
