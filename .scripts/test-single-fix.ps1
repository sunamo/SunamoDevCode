# Test fix on a single safe file (ConstsManager.cs with pathXlfKeys)
$projectRoot = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"
$testFile = "$projectRoot\ConstsManager.cs"

Write-Host "Testing fix on: $testFile" -ForegroundColor Green
Write-Host "Member to fix: pathXlfKeys -> PathXlfKeys" -ForegroundColor Cyan
Write-Host ""

# Backup original
$backupFile = "$testFile.backup"
Copy-Item $testFile $backupFile -Force
Write-Host "Created backup: $backupFile" -ForegroundColor Yellow

try {
    # Find all usages before
    Write-Host "`nSearching for usages of 'pathXlfKeys' (before)..." -ForegroundColor Cyan
    $files = Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse
    $usagesBefore = @()
    foreach ($file in $files) {
        $matches = Select-String -Path $file.FullName -Pattern "\bpathXlfKeys\b" -AllMatches
        $usagesBefore += $matches
    }
    Write-Host "Found $($usagesBefore.Count) usages before" -ForegroundColor Yellow

    # Perform replacement
    Write-Host "`nPerforming replacement in all files..." -ForegroundColor Cyan
    $filesChanged = 0
    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw
        if ($null -eq $content) {
            continue
        }

        $pattern = "\bpathXlfKeys\b"
        $newContent = $content -replace $pattern, "PathXlfKeys"

        if (-not $content.Equals($newContent)) {
            Set-Content -Path $file.FullName -Value $newContent -NoNewline
            $filesChanged++
            Write-Host "  Updated: $($file.FullName)" -ForegroundColor Green
        }
    }

    Write-Host "`nFiles changed: $filesChanged" -ForegroundColor Yellow

    # Find remaining usages
    Write-Host "`nSearching for remaining 'pathXlfKeys' (after)..." -ForegroundColor Cyan
    $usagesAfter = @()
    foreach ($file in $files) {
        $matches = Select-String -Path $file.FullName -Pattern "\bpathXlfKeys\b" -AllMatches
        $usagesAfter += $matches
    }

    Write-Host "Remaining 'pathXlfKeys': $($usagesAfter.Count)" -ForegroundColor $(if ($usagesAfter.Count -eq 0) { "Green" } else { "Red" })

    # Find new usages
    Write-Host "`nSearching for new 'PathXlfKeys'..." -ForegroundColor Cyan
    $usagesNew = @()
    foreach ($file in $files) {
        $matches = Select-String -Path $file.FullName -Pattern "\bPathXlfKeys\b" -AllMatches
        $usagesNew += $matches
    }
    Write-Host "New 'PathXlfKeys': $($usagesNew.Count)" -ForegroundColor Green

    # Show changes in the target file
    Write-Host "`nChanges in $testFile :" -ForegroundColor Cyan
    $originalContent = Get-Content $backupFile -Raw
    $newContent = Get-Content $testFile -Raw

    Write-Host "Original contains 'pathXlfKeys': $($originalContent.Contains('pathXlfKeys'))"
    Write-Host "New contains 'pathXlfKeys': $($newContent.Contains('pathXlfKeys'))"
    Write-Host "New contains 'PathXlfKeys': $($newContent.Contains('PathXlfKeys'))"

    Write-Host "`nSUCCESS!" -ForegroundColor Green

} catch {
    Write-Host "`nERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Restoring from backup..." -ForegroundColor Yellow
    Copy-Item $backupFile $testFile -Force
}

Write-Host "`nBackup file remains at: $backupFile" -ForegroundColor Gray
Write-Host "Review changes and delete backup if everything looks good." -ForegroundColor Gray
