cd 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode'
$output = dotnet build 2>&1
$warnings = $output | Select-String -Pattern 'warning'
$errors = $output | Select-String -Pattern 'error'
$summary = $output | Select-String -Pattern 'Warning\(s\)|Error\(s\)|Build succeeded|Build FAILED'

Write-Host "=== BUILD SUMMARY ==="
$summary

Write-Host "`n=== UNIQUE WARNING TYPES ==="
$warnings | ForEach-Object {
    if ($_ -match '(warning CS\d+):') {
        $matches[1]
    }
} | Sort-Object -Unique | ForEach-Object {
    $warningType = $_
    $count = ($warnings | Select-String -Pattern $warningType).Count
    Write-Host "$warningType : $count occurrences"
}
