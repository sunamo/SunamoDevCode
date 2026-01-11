cd 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode'
$output = dotnet build 2>&1

$errors = $output | Select-String -Pattern 'error CS' | ForEach-Object {
    if ($_ -match '([^\\]+\.cs)\((\d+),(\d+)\): error (CS\d+):') {
        [PSCustomObject]@{
            File = $matches[1]
            Line = $matches[2]
            Error = $matches[4]
            Message = $_
        }
    }
}

Write-Host "=== FILES WITH MOST ERRORS ==="
$errors | Group-Object File | Sort-Object Count -Descending | Select-Object -First 15 | ForEach-Object {
    Write-Host "$($_.Count) errors in $($_.Name)"
}

Write-Host "`n=== SAMPLE ERRORS ==="
$errors | Select-Object -First 20 | ForEach-Object {
    Write-Host "$($_.File):$($_.Line) - $($_.Error)"
}
