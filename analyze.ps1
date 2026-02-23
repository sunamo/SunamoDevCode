$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
$hash = @{}
foreach ($line in $lines) {
    if ($line -match 'warning (CS\d+)') {
        $code = $Matches[1]
        if ($hash.ContainsKey($code)) { $hash[$code]++ } else { $hash[$code] = 1 }
    }
}
$result = ""
foreach ($kv in $hash.GetEnumerator() | Sort-Object Value -Descending) {
    $result += "$($kv.Value) $($kv.Key)`n"
}
Set-Content -Path 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\warnings_summary.txt' -Value $result
