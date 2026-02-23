$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
$devcode = @()
$seen = @{}
foreach ($line in $lines) {
    if ($line -match '(E:\\vs\\Projects\\PlatformIndependentNuGetPackages\\SunamoDevCode\\SunamoDevCode\\[^(]+\.cs)\((\d+),(\d+)\): warning (CS\d+): (.+?) \[') {
        $file = $Matches[1]
        $lineNum = $Matches[2]
        $col = $Matches[3]
        $code = $Matches[4]
        $msg = $Matches[5]
        $key = "$file|$lineNum|$code"
        if (-not $seen.ContainsKey($key)) {
            $seen[$key] = $true
            $devcode += "$code`t$file`t$lineNum`t$col`t$msg"
        }
    }
}
$devcode | Out-File 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\devcode_warnings.txt' -Encoding utf8

# Also count by category for DevCode only
$hash = @{}
foreach ($entry in $devcode) {
    $parts = $entry -split "`t"
    $c = $parts[0]
    if ($hash.ContainsKey($c)) { $hash[$c]++ } else { $hash[$c] = 1 }
}
$summary = ""
$total = 0
foreach ($kv in $hash.GetEnumerator() | Sort-Object Value -Descending) {
    $total += $kv.Value
    $summary += "$($kv.Value) $($kv.Name)`n"
}
$summary += "Total unique DevCode warnings: $total"
$summary | Out-File 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\devcode_summary.txt' -Encoding utf8
