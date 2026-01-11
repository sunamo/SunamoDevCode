# Comprehensive script to fix variable naming issues in all identified files
param(
    [switch]$DryRun = $false
)

$projectRoot = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode"
$filesListPath = Join-Path $projectRoot ".scripts\files-needing-fixes.txt"

if (-not (Test-Path $filesListPath)) {
    Write-Error "Files list not found: $filesListPath"
    exit 1
}

$filesToFix = Get-Content $filesListPath | Where-Object { $_.Trim() -ne '' }

Write-Host "=== VARIABLE NAME FIXING SCRIPT ===" -ForegroundColor Cyan
Write-Host "Total files to process: $($filesToFix.Count)" -ForegroundColor Yellow
if ($DryRun) {
    Write-Host "DRY RUN MODE - No changes will be made" -ForegroundColor Magenta
}

$fixedCount = 0
$skippedCount = 0
$errorCount = 0

# Common single-letter replacements (excluding loop variables i, j, k)
$singleLetterReplacements = @{
    # Common patterns
    '\bstring\s+s\b' = 'string text'
    '\bvar\s+s\s*=' = 'var text ='
    '\bstring\s+v\b' = 'string value'
    '\bvar\s+v\s*=' = 'var value ='
    '\bList<string>\s+l\b' = 'List<string> list'
    '\bvar\s+l\s*=' = 'var list ='
    '\bbool\s+b\b' = 'bool result'
    '\bvar\s+b\s*=' = 'var result ='

    # Common abbreviations
    '\bvar\s+ret\b' = 'var result'
    '\bvar\s+vr\b' = 'var result'
    '\bbool\?\s+rv\b' = 'bool? returnValue'
    '\bvar\s+rv\b' = 'var returnValue'
    '\bvar\s+temp\b' = 'var temporary'
    '\bvar\s+tmp\b' = 'var temporary'
    '\bvar\s+arr\b' = 'var array'
    '\bvar\s+sb\b' = 'var stringBuilder'
    '\bStringBuilder\s+sb\b' = 'StringBuilder stringBuilder'
}

foreach ($filePath in $filesToFix) {
    Write-Host "`nProcessing: $filePath" -ForegroundColor White

    if (-not (Test-Path $filePath)) {
        Write-Host "  SKIP: File not found" -ForegroundColor Gray
        $skippedCount++
        continue
    }

    try {
        $content = Get-Content $filePath -Raw
        $originalContent = $content
        $changesMade = $false

        # Apply single-letter replacements
        foreach ($pattern in $singleLetterReplacements.Keys) {
            $replacement = $singleLetterReplacements[$pattern]
            if ($content -match $pattern) {
                $content = $content -replace $pattern, $replacement
                $changesMade = $true
                Write-Host "  - Applied: $pattern -> $replacement" -ForegroundColor Green
            }
        }

        # Fix common numbered variables (v1, v2, s1, s2)
        if ($content -match '\b([a-z])(\d+)\b') {
            $content = $content -replace '\bs1\b', 'firstString'
            $content = $content -replace '\bs2\b', 'secondString'
            $content = $content -replace '\bv1\b', 'firstValue'
            $content = $content -replace '\bv2\b', 'secondValue'
            $content = $content -replace '\bl1\b', 'firstList'
            $content = $content -replace '\bl2\b', 'secondList'
            $changesMade = $true
            Write-Host "  - Fixed numbered variables" -ForegroundColor Green
        }

        # Fix lambda parameter 'd' to 'line' or 'item'
        if ($content -match 'Where\(d\s*=>' -or $content -match 'Select\(d\s*=>') {
            $content = $content -replace '(\w+)\.Where\(d\s*=>', '$1.Where(line =>'
            $content = $content -replace '(\w+)\.Select\(d\s*=>', '$1.Select(item =>'
            $changesMade = $true
            Write-Host "  - Fixed lambda parameter 'd'" -ForegroundColor Green
        }

        # Save changes
        if ($changesMade -and -not $DryRun) {
            Set-Content -Path $filePath -Value $content -NoNewline
            $fixedCount++
            Write-Host "  FIXED: Changes saved" -ForegroundColor Green
        }
        elseif ($changesMade -and $DryRun) {
            Write-Host "  DRY RUN: Would fix this file" -ForegroundColor Yellow
        }
        else {
            Write-Host "  SKIP: No automatic fixes applicable" -ForegroundColor Gray
            $skippedCount++
        }
    }
    catch {
        Write-Host "  ERROR: $_" -ForegroundColor Red
        $errorCount++
    }
}

Write-Host "`n=== SUMMARY ===" -ForegroundColor Cyan
Write-Host "Fixed: $fixedCount" -ForegroundColor Green
Write-Host "Skipped: $skippedCount" -ForegroundColor Yellow
Write-Host "Errors: $errorCount" -ForegroundColor Red
Write-Host "Total: $($filesToFix.Count)" -ForegroundColor White

if ($DryRun) {
    Write-Host "`nThis was a DRY RUN. Run without -DryRun to apply changes." -ForegroundColor Magenta
}
