cd 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode'
$output = dotnet build 2>&1

# Extract unique "does not exist" errors
$notExist = $output | Select-String -Pattern "error CS0103.*name '([^']+)' does not exist" | ForEach-Object {
    if ($_ -match "name '([^']+)' does not exist") {
        $matches[1]
    }
} | Sort-Object -Unique

# Extract unique "does not contain a definition for" errors
$noDef = $output | Select-String -Pattern "does not contain a definition for '([^']+)'" | ForEach-Object {
    if ($_ -match "does not contain a definition for '([^']+)'") {
        $matches[1]
    }
} | Sort-Object -Unique

Write-Host "=== MISSING IDENTIFIERS (CS0103) ==="
$notExist | ForEach-Object { Write-Host "  $_" }

Write-Host "`n=== MISSING MEMBERS (CS0117/CS1061) ==="
$noDef | ForEach-Object { Write-Host "  $_" }
