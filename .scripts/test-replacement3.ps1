# Test why comparison fails
$testFile = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\ConstsManager.cs"

Write-Host "Testing why comparison fails" -ForegroundColor Green

$content = Get-Content $testFile -Raw

Write-Host "`nOriginal content info:" -ForegroundColor Cyan
Write-Host "  Length: $($content.Length)"
Write-Host "  Contains 'pathXlfKeys': $($content.Contains('pathXlfKeys'))"
Write-Host "  Type: $($content.GetType().FullName)"

Write-Host "`nPerforming replacement..." -ForegroundColor Cyan
$pattern = "pathXlfKeys"
$newContent = $content -replace $pattern, "PathXlfKeys"

Write-Host "`nNew content info:" -ForegroundColor Cyan
Write-Host "  Length: $($newContent.Length)"
Write-Host "  Contains 'pathXlfKeys': $($newContent.Contains('pathXlfKeys'))"
Write-Host "  Contains 'PathXlfKeys': $($newContent.Contains('PathXlfKeys'))"
Write-Host "  Type: $($newContent.GetType().FullName)"

Write-Host "`nComparison:" -ForegroundColor Cyan
Write-Host "  content -eq newContent: $($content -eq $newContent)"
Write-Host "  content.Equals(newContent): $($content.Equals($newContent))"

# Character-by-character comparison for first difference
Write-Host "`nLooking for first difference..." -ForegroundColor Cyan
$foundDiff = $false
for ($i = 0; $i -lt [Math]::Min($content.Length, $newContent.Length); $i++) {
    if ($content[$i] -ne $newContent[$i]) {
        Write-Host "  First difference at position $i" -ForegroundColor Yellow
        Write-Host "  Original: '$($content[$i])' (char code: $([int]$content[$i]))"
        Write-Host "  New: '$($newContent[$i])' (char code: $([int]$newContent[$i]))"

        # Show context
        $start = [Math]::Max(0, $i - 20)
        $end = [Math]::Min($content.Length, $i + 20)
        Write-Host "  Context (original): '$($content.Substring($start, $end - $start))'"
        Write-Host "  Context (new): '$($newContent.Substring($start, $end - $start))'"

        $foundDiff = $true
        break
    }
}

if (-not $foundDiff) {
    if ($content.Length -ne $newContent.Length) {
        Write-Host "  Strings have different lengths but no character difference found in common part"
    } else {
        Write-Host "  No differences found - strings are identical!"
    }
}
