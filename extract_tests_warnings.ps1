$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
$tests = @()
$seen = @{}
foreach ($line in $lines) {
    if ($line -match '(E:\\vs\\Projects\\PlatformIndependentNuGetPackages\\SunamoDevCode\\(SunamoDevCode\.Tests|RunnerDevCode)\\[^(]+\.cs)\((\d+),(\d+)\): warning (CS\d+): (.+?) \[') {
        $file = $Matches[1]
        $lineNum = $Matches[3]
        $col = $Matches[4]
        $code = $Matches[5]
        $msg = $Matches[6]
        $key = "$file|$lineNum|$code"
        if (-not $seen.ContainsKey($key)) {
            $seen[$key] = $true
            $tests += "$code`t$file`t$lineNum`t$col`t$msg"
        }
    }
}
$tests | Out-File 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\tests_warnings.txt' -Encoding utf8
