# Test different replacement approaches
$testFile = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\ConstsManager.cs"

Write-Host "Testing replacement approaches" -ForegroundColor Green

$content = Get-Content $testFile -Raw

Write-Host "`nApproach 1: Simple -replace with word boundaries" -ForegroundColor Cyan
$pattern1 = "\bpathXlfKeys\b"
$new1 = $content -replace $pattern1, "PathXlfKeys"
Write-Host "Changed: $($content -ne $new1)"

Write-Host "`nApproach 2: -replace without escape" -ForegroundColor Cyan
$pattern2 = "pathXlfKeys"
$new2 = $content -replace $pattern2, "PathXlfKeys"
Write-Host "Changed: $($content -ne $new2)"
Write-Host "Original 'pathXlfKeys' count: $(([regex]::Matches($content, 'pathXlfKeys')).Count)"
Write-Host "New 'PathXlfKeys' count: $(([regex]::Matches($new2, 'PathXlfKeys')).Count)"
Write-Host "Remaining 'pathXlfKeys' count: $(([regex]::Matches($new2, 'pathXlfKeys')).Count)"

Write-Host "`nApproach 3: [regex]::Replace with word boundaries" -ForegroundColor Cyan
$pattern3 = "\bpathXlfKeys\b"
$new3 = [regex]::Replace($content, $pattern3, "PathXlfKeys")
Write-Host "Changed: $($content -ne $new3)"
Write-Host "Original 'pathXlfKeys' count: $(([regex]::Matches($content, 'pathXlfKeys')).Count)"
Write-Host "New 'PathXlfKeys' count: $(([regex]::Matches($new3, 'PathXlfKeys')).Count)"
Write-Host "Remaining 'pathXlfKeys' count: $(([regex]::Matches($new3, 'pathXlfKeys')).Count)"

if ($content -ne $new3) {
    Write-Host "`nShowing first change:" -ForegroundColor Yellow
    $match = [regex]::Match($new3, "PathXlfKeys")
    if ($match.Success) {
        $start = [Math]::Max(0, $match.Index - 30)
        $end = [Math]::Min($new3.Length, $match.Index + 40)
        $context = $new3.Substring($start, $end - $start)
        Write-Host "  ...$context..."
    }
}
