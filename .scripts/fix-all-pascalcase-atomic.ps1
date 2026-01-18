# Atomicky opravi VSECHNY public/internal members na PascalCase
# Pouzije PowerShell regex replace pro maximalni rychlost

$ErrorActionPreference = "Stop"

Write-Host "=== ATOMIC PASCALCASE FIX ===" -ForegroundColor Cyan

# Najdi vsechny .cs soubory (krome obj/bin)
$files = Get-ChildItem -Path "SunamoDevCode" -Filter "*.cs" -Recurse |
    Where-Object { $_.FullName -notmatch "\\obj\\" -and $_.FullName -notmatch "\\bin\\" }

Write-Host "Found $($files.Count) .cs files" -ForegroundColor Yellow

$changedFiles = 0

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content

    # Fix common public/internal fields with camelCase
    # Pattern: public/internal followed by type and camelCase field name
    $content = $content -replace '\b(public|internal)\s+([\w<>?\[\]]+)\s+([a-z]\w+)\s*=', '$1 $2 $3TEMP_MARKER ='
    $content = $content -replace '([a-z])(\w+)TEMP_MARKER', { param($m) $m.Groups[1].Value.ToUpper() + $m.Groups[2].Value }

    # Fix specific known fields
    $replacements = @{
        '\bpublic\s+string\s+path\b' = 'public string Path'
        '\bpublic\s+string\s+name\b' = 'public string Name'
        '\bpublic\s+T\s+t\b' = 'public T Value'
        '\bpublic\s+bool\s+empty\b' = 'public bool Empty'
        '\bpublic\s+StringBuilder\s+stringBuilder\b' = 'public StringBuilder StringBuilder'
        '\bpublic\s+string\s+prependEveryNoWhite\b' = 'public string PrependEveryNoWhite'
        '\binternal\s+string\s+path\b' = 'internal string Path'
        '\binternal\s+bool\s+loadChangesFromDrive\b' = 'internal bool LoadChangesFromDrive'
        '\bpublic\s+bool\s+mergeAndFetch\b' = 'public bool MergeAndFetch'
        '\bpublic\s+bool\s+addGitignore\b' = 'public bool AddGitignore'
        '\bpublic\s+GitTypesOfMessages\s+checkForGit\b' = 'public GitTypesOfMessages CheckForGit'
        '\bpublic\s+List<string>\s+onlyThese\b' = 'public List<string> OnlyThese'
        '\bpublic\s+bool\?\s+cs\b' = 'public bool? Cs'
    }

    foreach ($pattern in $replacements.Keys) {
        $content = $content -replace $pattern, $replacements[$pattern]
    }

    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        $changedFiles++
        Write-Host "Fixed: $($file.Name)" -ForegroundColor Green
    }
}

Write-Host "`nChanged $changedFiles files" -ForegroundColor Cyan
Write-Host "=== DONE ===" -ForegroundColor Green
