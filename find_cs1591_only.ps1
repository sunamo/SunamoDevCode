$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
$filtered = @()
foreach($line in $lines) {
    if($line -match 'warning CS' -and $line -match 'net10\.0') {
        $filtered += $line
    }
}
# Group by file and check if all warnings are CS1591
$fileWarnings = @{}
foreach($line in $filtered) {
    if($line -match '^(.+?)\(\d') {
        $f = $matches[1].Trim()
        if(-not $fileWarnings.ContainsKey($f)) { $fileWarnings[$f] = @() }
        if($line -match 'warning (CS\d+)') {
            $fileWarnings[$f] += $matches[1]
        }
    }
}
# Find files where ALL warnings are CS1591 and there are at least 10
foreach($entry in $fileWarnings.GetEnumerator()) {
    $allCs1591 = ($entry.Value | Where-Object { $_ -ne 'CS1591' }).Count -eq 0
    if($allCs1591 -and $entry.Value.Count -ge 10) {
        Write-Host "$($entry.Value.Count)`t$($entry.Key)"
    }
}
