$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
$files = @{}
foreach ($line in $lines) {
    if ($line -match '(E:\\[^(]+\.cs)\(\d+,\d+\): warning (CS\d+)') {
        $file = $Matches[1]
        $code = $Matches[2]
        $key = "$file|$code"
        if ($files.ContainsKey($key)) { $files[$key]++ } else { $files[$key] = 1 }
    }
}
$result = ""
foreach ($kv in $files.GetEnumerator() | Sort-Object Value -Descending) {
    $parts = $kv.Key -split '\|'
    $result += "$($kv.Value)`t$($parts[1])`t$($parts[0])`n"
}
Set-Content -Path 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\warnings_by_file.txt' -Value $result
