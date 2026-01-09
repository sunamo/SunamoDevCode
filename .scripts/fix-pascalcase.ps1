# Script to fix public/internal members to use PascalCase
# EN: Fixes all public/internal fields, properties, and constants to use PascalCase
# CZ: Opraví všechny public/internal fields, properties a konstanty na PascalCase

param(
    [switch]$DryRun = $false
)

$projectRoot = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

function Convert-ToPascalCase {
    param([string]$name)

    if ($name.Length -eq 0) { return $name }

    # First character to uppercase
    $firstChar = $name.Substring(0, 1).ToUpper()
    $rest = $name.Substring(1)

    return $firstChar + $rest
}

function Get-CamelCaseMembers {
    param([string]$filePath)

    $lines = Get-Content $filePath

    # Skip files with "// variables names: ok"
    if ($lines.Count -gt 0 -and $lines[0] -match '//\s*variables\s+names:\s*ok') {
        return @()
    }

    $members = @()

    # Find public/internal members with camelCase names
    # Pattern: (public|internal) [static] [readonly] Type camelCaseName [= or ; or {]
    foreach ($line in $lines) {
        # Match: public/internal [static] [readonly] Type camelCaseName
        if ($line -match '^\s*(public|internal)\s+(static\s+)?(readonly\s+)?([a-zA-Z<>\[\]`]+)\s+([a-z][a-zA-Z0-9]*)\s*[=;\{]') {
            $visibility = $Matches[1]
            $isStatic = $Matches[2] -ne ''
            $isReadonly = $Matches[3] -ne ''
            $type = $Matches[4]
            $oldName = $Matches[5]
            $newName = Convert-ToPascalCase $oldName

            if ($oldName -ne $newName) {
                $members += @{
                    OldName = $oldName
                    NewName = $newName
                    Visibility = $visibility
                    Type = $type
                    IsStatic = $isStatic
                    IsReadonly = $isReadonly
                    Line = $line
                }
            }
        }
    }

    return $members
}

function Rename-MemberInFile {
    param(
        [string]$filePath,
        [string]$oldName,
        [string]$newName
    )

    $content = Get-Content $filePath -Raw

    # Replace member access: .oldName or ->oldName
    $content = $content -replace "\.${oldName}\b", ".$newName"
    $content = $content -replace "->${oldName}\b", "->$newName"

    # Replace in assignments and declarations
    $content = $content -replace "\b${oldName}\s*=", "$newName ="
    $content = $content -replace "\b${oldName}\s*;", "$newName;"

    # Replace as standalone identifier (parameter, variable reference)
    # But be careful not to replace in strings
    $content = $content -replace "([^a-zA-Z0-9_])${oldName}([^a-zA-Z0-9_])", "`$1$newName`$2"

    if (-not $DryRun) {
        Set-Content $filePath $content -NoNewline
    }

    return $true
}

Write-Host "Scanning for public/internal members with camelCase..." -ForegroundColor Cyan

$allFiles = Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse
$filesWithIssues = @{}

foreach ($file in $allFiles) {
    $members = Get-CamelCaseMembers $file.FullName

    if ($members.Count -gt 0) {
        $filesWithIssues[$file.FullName] = $members

        Write-Host "`nFile: $($file.Name)" -ForegroundColor Yellow
        foreach ($member in $members) {
            Write-Host "  $($member.Visibility) $($member.Type) $($member.OldName) -> $($member.NewName)" -ForegroundColor Gray
        }
    }
}

if ($filesWithIssues.Count -eq 0) {
    Write-Host "`nNo issues found!" -ForegroundColor Green
    exit 0
}

Write-Host "`n`nFound $($filesWithIssues.Count) files with issues." -ForegroundColor Cyan

if ($DryRun) {
    Write-Host "DRY RUN - No changes will be made." -ForegroundColor Yellow
    exit 0
}

Write-Host "`nStarting rename process..." -ForegroundColor Cyan

$renamedCount = 0

foreach ($filePath in $filesWithIssues.Keys) {
    $members = $filesWithIssues[$filePath]

    Write-Host "`nProcessing: $(Split-Path $filePath -Leaf)" -ForegroundColor Yellow

    foreach ($member in $members) {
        Write-Host "  Renaming $($member.OldName) -> $($member.NewName)..." -ForegroundColor Gray

        # Rename in all .cs files in the project
        foreach ($file in $allFiles) {
            Rename-MemberInFile $file.FullName $member.OldName $member.NewName | Out-Null
        }

        $renamedCount++
    }
}

Write-Host "`n`nCompleted! Renamed $renamedCount members." -ForegroundColor Green
Write-Host "Please verify the changes and run tests." -ForegroundColor Yellow
