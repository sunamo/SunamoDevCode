# Rename methods with underscores to meaningful names

$ErrorActionPreference = "Stop"

Write-Host "Renaming methods with underscores..." -ForegroundColor Cyan

# Get all .cs files in SunamoDevCode (excluding Tests)
$files = Get-ChildItem -Path "SunamoDevCode" -Filter "*.cs" -Recurse | Where-Object { $_.FullName -notmatch "Tests" }

$totalReplacements = 0

foreach ($file in $files) {
    $content = Get-Content -Path $file.FullName -Raw -Encoding UTF8
    $originalContent = $content

    # 1. Rename: EnterOutputOfPowershellGit_ChangeDialogResult -> ProcessPowershellGitOutput
    $content = $content -replace '\bEnterOutputOfPowershellGit_ChangeDialogResult\b', 'ProcessPowershellGitOutput'

    # 2. Rename: W_Changed -> OnFileSystemChanged
    $content = $content -replace '\bW_Changed\b', 'OnFileSystemChanged'

    # Save if changed
    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8 -NoNewline
        Write-Host "  ✓ $($file.Name)" -ForegroundColor Green
        $totalReplacements++
    }
}

Write-Host "`nCompleted! Modified $totalReplacements file(s)." -ForegroundColor Green
Write-Host "`nWARNING: ApsHelper1.cs:276 had infinite recursion bug - please review the renamed method!" -ForegroundColor Yellow
