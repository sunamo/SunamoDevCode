# Find files with variable naming issues
$filesToCheck = Get-Content "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\.scripts\files-to-check.txt"

$singleLetterPattern = '\b(this\s+\w+\s+[a-z])\b|\bFunc<[^>]+>\s+[a-z]\b|\bAction<[^>]+>\s+[a-z]\b|\b(string|int|bool|List<[^>]+>|Dictionary<[^>]+>)\s+([a-z])\b(?!\s*=\s*0)'
$abbreviations = @('ret', 'vr', 'arr', 'sb', 'ts', 'ci', 'mal', 'yy')
$czechWords = @('pocet', 'datum', 'cas', 'indexMezery', 'indexTecky', 'indexCiarky', 'slozkyKVytvoreni', 'kopia')

$filesWithIssues = @()

foreach ($file in $filesToCheck) {
    if (Test-Path $file) {
        $content = Get-Content $file -Raw

        # Skip if already has comment
        $firstLine = (Get-Content $file -First 1)
        if ($firstLine -eq "// variables names: ok") {
            continue
        }

        $hasIssues = $false
        $issues = @()

        # Check for single-letter parameters (excluding i, j, k in for loops)
        if ($content -match $singleLetterPattern) {
            $hasIssues = $true
            $issues += "Single-letter parameters"
        }

        # Check for abbreviations
        foreach ($abbr in $abbreviations) {
            if ($content -match "\b$abbr\b") {
                $hasIssues = $true
                $issues += "Abbreviation: $abbr"
                break
            }
        }

        # Check for Czech words
        foreach ($czech in $czechWords) {
            if ($content -match "\b$czech\b") {
                $hasIssues = $true
                $issues += "Czech word: $czech"
                break
            }
        }

        if ($hasIssues) {
            $filesWithIssues += [PSCustomObject]@{
                File = $file
                Issues = $issues -join ", "
            }
        }
    }
}

Write-Host "Files with variable naming issues:" -ForegroundColor Yellow
Write-Host ""
$filesWithIssues | Format-Table -AutoSize
Write-Host ""
Write-Host "Total: $($filesWithIssues.Count) files with issues"
