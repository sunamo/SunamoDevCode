# Analyzes all .cs files for variable naming issues
# Returns list of files that need fixing

$projectRoot = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode"
$sourceRoot = Join-Path $projectRoot "SunamoDevCode"

# Get all .cs files (excluding obj/bin)
$allFiles = Get-ChildItem -Path $sourceRoot -Filter "*.cs" -Recurse | Where-Object {
    $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\'
}

Write-Host "Total .cs files found: $($allFiles.Count)"

$filesNeedingFixes = @()
$enumFiles = @()
$emptyFiles = @()
$okFiles = @()

foreach ($file in $allFiles) {
    $content = Get-Content $file.FullName -Raw

    # Check if it already has the OK comment
    if ($content -match '^//\s*variables\s+names:\s+ok') {
        $okFiles += $file.FullName
        continue
    }

    # Check if it's an enum-only file
    if ($content -match 'enum\s+\w+' -and $content -notmatch 'class\s+\w+' -and $content -notmatch 'interface\s+\w+' -and $content -notmatch 'struct\s+\w+') {
        $enumFiles += $file.FullName
        continue
    }

    # Check if file is essentially empty (only namespace/usings)
    $linesWithCode = ($content -split "`n" | Where-Object {
        $_.Trim() -ne '' -and
        $_ -notmatch '^\s*using\s+' -and
        $_ -notmatch '^\s*namespace\s+' -and
        $_ -notmatch '^\s*//' -and
        $_ -notmatch '^\s*\{' -and
        $_ -notmatch '^\s*\}'
    }).Count

    if ($linesWithCode -lt 3) {
        $emptyFiles += $file.FullName
        continue
    }

    # Check for potential issues
    $hasIssues = $false

    # Check for single-letter variable names (excluding i, j, k in for loops)
    if ($content -match '\b([a-hm-z]|l)\s+[a-z]\s*[=;,)]' -or
        $content -match 'this\s+\w+\s+[a-z]\b' -or
        $content -match '\(.*,\s*[a-z]\s*\)' -or
        $content -match 'Func<.*>\s+[a-z]\b') {
        $hasIssues = $true
    }

    # Check for numbered variables (v1, v2, s1, s2, etc.)
    if ($content -match '\b[a-z]\d+\b') {
        $hasIssues = $true
    }

    # Check for common bad variable names
    $badNames = @('ret\b', 'vr\b', 'temp\b', 'tmp\b', 'arr\b', '\bci\b', '\bsb\b', '\bts\b')
    foreach ($badName in $badNames) {
        if ($content -match $badName) {
            $hasIssues = $true
            break
        }
    }

    if ($hasIssues) {
        $filesNeedingFixes += $file.FullName
    }
}

Write-Host "`n=== ANALYSIS RESULTS ===" -ForegroundColor Cyan
Write-Host "Files already OK: $($okFiles.Count)" -ForegroundColor Green
Write-Host "Enum-only files: $($enumFiles.Count)" -ForegroundColor Yellow
Write-Host "Empty/minimal files: $($emptyFiles.Count)" -ForegroundColor Gray
Write-Host "Files needing fixes: $($filesNeedingFixes.Count)" -ForegroundColor Red

# Save lists to files for processing
$filesNeedingFixes | Out-File (Join-Path $projectRoot ".scripts\files-needing-fixes.txt")
$enumFiles | Out-File (Join-Path $projectRoot ".scripts\enum-files.txt")
$emptyFiles | Out-File (Join-Path $projectRoot ".scripts\empty-files.txt")

Write-Host "`nFile lists saved to .scripts\ directory"
Write-Host "`nFirst 20 files needing fixes:"
$filesNeedingFixes | Select-Object -First 20 | ForEach-Object { Write-Host "  $_" }
