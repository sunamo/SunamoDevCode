param([string]$FilePath)
$lines = Get-Content 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\build_output.txt'
foreach($line in $lines) {
    if($line -match 'warning CS' -and $line -match 'net10\.0' -and $line -match [regex]::Escape($FilePath)) {
        Write-Host $line
    }
}
