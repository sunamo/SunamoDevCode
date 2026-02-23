$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
$filtered = @()
foreach($line in $lines) {
    if($line -match 'warning CS' -and $line -match 'net10\.0') {
        $filtered += $line
    }
}
$dict = @{}
foreach($line in $filtered) {
    if($line -match '^(.+?)\(\d') {
        $f = $matches[1].Trim()
        if($dict.ContainsKey($f)) { $dict[$f]++ } else { $dict[$f] = 1 }
    }
}
$sorted = $dict.GetEnumerator() | Sort-Object Value -Descending | Select-Object -First 30
foreach($s in $sorted) {
    Write-Host "$($s.Value)`t$($s.Key)"
}
