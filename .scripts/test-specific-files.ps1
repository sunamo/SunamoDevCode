# Test script on specific files
param(
    [string[]]$TestFiles = @(
        "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\FileFormats\TransUnit.cs",
        "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\ConstsManager.cs",
        "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\Aps\Projs\VsProjectFile.cs"
    )
)

$projectRoot = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

# Function to check if file has "variables names: ok" comment
function Has-VariablesNamesOk {
    param([string]$filePath)

    if (-not (Test-Path $filePath)) {
        return $false
    }

    $firstLine = Get-Content $filePath -First 1 -ErrorAction SilentlyContinue
    return $firstLine -eq "// variables names: ok"
}

# Function to convert camelCase to PascalCase
function ConvertTo-PascalCase {
    param([string]$name)

    if ([string]::IsNullOrEmpty($name)) {
        return $name
    }

    return $name.Substring(0, 1).ToUpper() + $name.Substring(1)
}

# Function to find camelCase public/internal members in a file
function Find-CamelCaseMembers {
    param([string]$filePath)

    $content = Get-Content $filePath -Raw
    $members = @()

    # Regex patterns for public/internal camelCase members
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
                    Line = ($content.Substring(0, $match.Index) -split "`n").Count
                }
            }
        }
    }

    return $members | Sort-Object -Property Name -Unique
}

# Function to find all usages using Select-String
function Find-AllUsages {
    param(
        [string]$memberName,
        [string]$projectRoot
    )

    Write-Host "  Searching for usages of '$memberName'..." -ForegroundColor Cyan

    $files = Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse
    $results = @()

    foreach ($file in $files) {
        $matches = Select-String -Path $file.FullName -Pattern "\b$memberName\b" -AllMatches
        foreach ($match in $matches) {
            $results += "$($file.FullName):$($match.LineNumber):$($match.Line)"
        }
    }

    if ($results.Count -gt 0) {
        Write-Host "  Found $($results.Count) usages" -ForegroundColor Yellow
    }

    return $results
}

# Function to replace all occurrences in a file
function Replace-InFile {
    param(
        [string]$filePath,
        [string]$oldName,
        [string]$newName
    )

    $content = Get-Content $filePath -Raw

    # Skip if file is empty or null
    if ($null -eq $content) {
        return $false
    }

    # Use word boundaries to avoid partial matches
    $pattern = "\b$oldName\b"
    $newContent = $content -replace $pattern, $newName

    # Use .Equals() for case-sensitive comparison (PowerShell -eq is case-insensitive)
    if (-not $content.Equals($newContent)) {
        Set-Content -Path $filePath -Value $newContent -NoNewline
        return $true
    }

    return $false
}

# Main processing function
function Process-File {
    param([string]$filePath)

    Write-Host "`nProcessing: $filePath" -ForegroundColor Green

    if (Has-VariablesNamesOk $filePath) {
        Write-Host "  Skipped (has '// variables names: ok' comment)" -ForegroundColor Gray
        return
    }

    $members = Find-CamelCaseMembers $filePath

    if ($members.Count -eq 0) {
        Write-Host "  No camelCase public/internal members found" -ForegroundColor Gray
        return
    }

    Write-Host "  Found $($members.Count) camelCase members:" -ForegroundColor Yellow

    foreach ($member in $members) {
        Write-Host "    - $($member.Name) -> $($member.PascalCase) (line $($member.Line))" -ForegroundColor Cyan

        $usages = Find-AllUsages $member.Name $projectRoot

        # Group usages by file
        $fileUsages = @{}
        foreach ($usage in $usages) {
            if ($usage -match '^([^:]+):(\d+):(.*)$') {
                $file = $matches[1]
                if (-not $fileUsages.ContainsKey($file)) {
                    $fileUsages[$file] = @()
                }
                $fileUsages[$file] += $usage
            }
        }

        # Replace in all files
        $filesChanged = 0
        foreach ($file in $fileUsages.Keys) {
            $changed = Replace-InFile $file $member.Name $member.PascalCase
            if ($changed) {
                $filesChanged++
                Write-Host "      Updated: $file" -ForegroundColor DarkGreen
            }
        }

        Write-Host "    Total files updated: $filesChanged" -ForegroundColor Green
    }
}

# Main execution
Write-Host "========================================" -ForegroundColor Magenta
Write-Host "Test Public/Internal PascalCase Fix" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta

foreach ($file in $TestFiles) {
    if (Test-Path $file) {
        Process-File $file
    } else {
        Write-Host "`nFile not found: $file" -ForegroundColor Red
    }
}

Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "Test completed" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta
