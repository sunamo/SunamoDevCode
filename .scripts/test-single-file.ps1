# Test script for a single file
$filePath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\FileFormats\TransUnit.cs"

$content = Get-Content $filePath -Raw

Write-Host "Testing regex patterns on: $filePath" -ForegroundColor Green
Write-Host ""

# Regex patterns for public/internal camelCase members
$patterns = @{
    "Fields" = '(?m)^\s*(public|internal)\s+(?:static\s+)?(?:readonly\s+)?(?:const\s+)?[\w<>\[\]?]+\s+([a-z]\w*)\s*[;=]'
    "Properties" = '(?m)^\s*(public|internal)\s+(?:static\s+)?(?:virtual\s+)?(?:override\s+)?[\w<>\[\]?]+\s+([a-z]\w*)\s*\{'
    "Methods" = '(?m)^\s*(public|internal)\s+(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:async\s+)?[\w<>\[\]?]+\s+([a-z]\w*)\s*\('
}

foreach ($patternName in $patterns.Keys) {
    Write-Host "Pattern: $patternName" -ForegroundColor Cyan
    Write-Host "Regex: $($patterns[$patternName])" -ForegroundColor Gray

    $matches = [regex]::Matches($content, $patterns[$patternName])

    if ($matches.Count -gt 0) {
        Write-Host "Found $($matches.Count) matches:" -ForegroundColor Yellow
        foreach ($match in $matches) {
            $memberName = $match.Groups[2].Value
            $lineNum = ($content.Substring(0, $match.Index) -split "`n").Count
            $pascalCase = $memberName.Substring(0, 1).ToUpper() + $memberName.Substring(1)
            Write-Host "  Line $lineNum : $memberName -> $pascalCase" -ForegroundColor White
            Write-Host "    Full match: $($match.Value.Trim())" -ForegroundColor DarkGray
        }
    } else {
        Write-Host "  No matches found" -ForegroundColor Gray
    }

    Write-Host ""
}
