# Test replacement logic
$testFile = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\ConstsManager.cs"

Write-Host "Testing replacement in: $testFile" -ForegroundColor Green

$content = Get-Content $testFile -Raw

Write-Host "`nSearching for 'pathXlfKeys'..." -ForegroundColor Cyan
$pattern = "\bpathXlfKeys\b"
$matches = [regex]::Matches($content, $pattern)
Write-Host "Found $($matches.Count) matches"

if ($matches.Count -gt 0) {
    Write-Host "`nMatches:" -ForegroundColor Yellow
    for ($i = 0; $i -lt [Math]::Min(5, $matches.Count); $i++) {
        $match = $matches[$i]
        $lineNum = ($content.Substring(0, $match.Index) -split "`n").Count
        $start = [Math]::Max(0, $match.Index - 30)
        $end = [Math]::Min($content.Length, $match.Index + 30)
        $context = $content.Substring($start, $end - $start)
        Write-Host "  Line $lineNum : ...$context..."
    }

    Write-Host "`nTesting replacement..." -ForegroundColor Cyan
    $newContent = $content -replace $pattern, "PathXlfKeys"

    if ($content -eq $newContent) {
        Write-Host "ERROR: Content didn't change!" -ForegroundColor Red
        Write-Host "Original length: $($content.Length)"
        Write-Host "New length: $($newContent.Length)"
    } else {
        Write-Host "SUCCESS: Content would be changed" -ForegroundColor Green
        Write-Host "Original length: $($content.Length)"
        Write-Host "New length: $($newContent.Length)"

        # Show first few changes
        Write-Host "`nFirst change:" -ForegroundColor Yellow
        $newMatches = [regex]::Matches($newContent, "\bPathXlfKeys\b")
        if ($newMatches.Count -gt 0) {
            $match = $newMatches[0]
            $start = [Math]::Max(0, $match.Index - 30)
            $end = [Math]::Min($newContent.Length, $match.Index + 40)
            $context = $newContent.Substring($start, $end - $start)
            Write-Host "  ...$context..."
        }
    }
}
