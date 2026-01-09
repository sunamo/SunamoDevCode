# Simple and direct fix for public/internal PascalCase
# This version processes ALL files in one pass for each member

$projectRoot = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

function Has-VariablesNamesOk {
    param([string]$filePath)
    $firstLine = Get-Content $filePath -First 1 -ErrorAction SilentlyContinue
    return $firstLine -eq "// variables names: ok"
}

function ConvertTo-PascalCase {
    param([string]$name)
    return $name.Substring(0, 1).ToUpper() + $name.Substring(1)
}

function Find-CamelCaseMembers {
    param([string]$filePath)

    $content = Get-Content $filePath -Raw
    if ($null -eq $content) {
        return @()
    }

    $members = @()
    $patterns = @(
        '(?m)^\s*(public|internal)\s+(?:static\s+)?(?:readonly\s+)?(?:const\s+)?[\w<>\[\]?]+\s+([a-z]\w*)\s*[;=]',
        '(?m)^\s*(public|internal)\s+(?:static\s+)?(?:virtual\s+)?(?:override\s+)?[\w<>\[\]?]+\s+([a-z]\w*)\s*\{'
    )

    foreach ($pattern in $patterns) {
        $matches = [regex]::Matches($content, $pattern)
        foreach ($match in $matches) {
            $memberName = $match.Groups[2].Value
            if ($memberName.Length -gt 1) {
                $members += @{
                    Name = $memberName
                    PascalCase = ConvertTo-PascalCase $memberName
                }
            }
        }
    }

    return $members | Sort-Object -Property Name -Unique
}

Write-Host "========================================" -ForegroundColor Magenta
Write-Host "Fix Public/Internal PascalCase (Simple)" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta

# Step 1: Collect ALL camelCase members from ALL files
Write-Host "`nStep 1: Scanning for camelCase members..." -ForegroundColor Cyan

$allFiles = Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse
$allMembers = @{}

foreach ($file in $allFiles) {
    if (Has-VariablesNamesOk $file.FullName) {
        continue
    }

    $members = Find-CamelCaseMembers $file.FullName
    foreach ($member in $members) {
        if (-not $allMembers.ContainsKey($member.Name)) {
            $allMembers[$member.Name] = $member.PascalCase
        }
    }
}

Write-Host "Found $($allMembers.Count) unique camelCase members" -ForegroundColor Yellow

# Step 2: Replace each member across ALL files
Write-Host "`nStep 2: Replacing members..." -ForegroundColor Cyan

foreach ($entry in $allMembers.GetEnumerator()) {
    $oldName = $entry.Key
    $newName = $entry.Value

    Write-Host "`nReplacing: $oldName -> $newName" -ForegroundColor Green

    $filesChanged = 0
    $totalReplacements = 0

    foreach ($file in $allFiles) {
        $content = Get-Content $file.FullName -Raw
        if ($null -eq $content) {
            continue
        }

        # Use word boundaries to avoid partial matches
        $pattern = "\b$oldName\b"
        $newContent = $content -replace $pattern, $newName

        # Check if anything changed using case-sensitive comparison
        if (-not $content.Equals($newContent)) {
            try {
                Set-Content -Path $file.FullName -Value $newContent -NoNewline -Force
                $filesChanged++

                # Count replacements
                $replacementCount = ([regex]::Matches($content, $pattern)).Count
                $totalReplacements += $replacementCount

                if ($replacementCount -gt 0) {
                    Write-Host "  $($file.Name): $replacementCount replacements" -ForegroundColor DarkGreen
                }
            } catch {
                Write-Host "  ERROR in $($file.Name): $($_.Exception.Message)" -ForegroundColor Red
            }
        }
    }

    if ($filesChanged -gt 0) {
        Write-Host "  Total: $totalReplacements replacements in $filesChanged files" -ForegroundColor Green
    } else {
        Write-Host "  No changes made" -ForegroundColor Gray
    }
}

Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "COMPLETED" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta
Write-Host "All public/internal camelCase members have been converted to PascalCase" -ForegroundColor Green
