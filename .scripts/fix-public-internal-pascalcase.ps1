# Fix all public/internal members to use PascalCase naming convention
# This script finds camelCase members and converts them to PascalCase

param(
    [switch]$TestMode = $false,
    [int]$TestFileCount = 5
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
    # Match: public/internal [static] [readonly] type camelCaseName
    $patterns = @(
        # Fields: public/internal [static] [readonly] Type name;
        '(?m)^\s*(public|internal)\s+(?:static\s+)?(?:readonly\s+)?(?:const\s+)?[\w<>\[\]?]+\s+([a-z]\w*)\s*[;=]',
        # Properties: public/internal Type name { get; set; }
        '(?m)^\s*(public|internal)\s+(?:static\s+)?(?:virtual\s+)?(?:override\s+)?[\w<>\[\]?]+\s+([a-z]\w*)\s*\{',
        # Methods: public/internal Type name(
        '(?m)^\s*(public|internal)\s+(?:static\s+)?(?:virtual\s+)?(?:override\s+)?(?:async\s+)?[\w<>\[\]?]+\s+([a-z]\w*)\s*\('
    )

    foreach ($pattern in $patterns) {
        $matches = [regex]::Matches($content, $pattern)
        foreach ($match in $matches) {
            $memberName = $match.Groups[2].Value
            # Skip single letter names (likely loop variables if somehow captured)
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

# Function to find all usages of a member in the project
function Find-AllUsages {
    param(
        [string]$memberName,
        [string]$projectRoot
    )

    Write-Host "  Searching for usages of '$memberName'..." -ForegroundColor Cyan

    # Try to find ripgrep
    $rgPath = $null
    $rgLocations = @("rg", "rg.exe", "C:\Program Files\ripgrep\rg.exe", "$env:USERPROFILE\.cargo\bin\rg.exe")

    foreach ($location in $rgLocations) {
        if (Get-Command $location -ErrorAction SilentlyContinue) {
            $rgPath = $location
            break
        }
    }

    if ($rgPath) {
        # Use ripgrep if available
        try {
            $results = & $rgPath --type cs --line-number --no-heading --with-filename "\b$memberName\b" $projectRoot 2>$null
            if ($LASTEXITCODE -eq 0 -and $results) {
                $usageCount = ($results | Measure-Object).Count
                Write-Host "  Found $usageCount usages" -ForegroundColor Yellow
                return $results
            }
        } catch {
            Write-Host "  Warning: ripgrep failed, falling back to Select-String" -ForegroundColor Yellow
        }
    }

    # Fallback to Select-String (slower but always available)
    Write-Host "  Using Select-String (slower)..." -ForegroundColor Gray
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

    # Skip if has "variables names: ok" comment
    if (Has-VariablesNamesOk $filePath) {
        Write-Host "  Skipped (has '// variables names: ok' comment)" -ForegroundColor Gray
        return @{
            Processed = $false
            Reason = "HasOkComment"
        }
    }

    # Find camelCase members
    $members = Find-CamelCaseMembers $filePath

    if ($members.Count -eq 0) {
        Write-Host "  No camelCase public/internal members found" -ForegroundColor Gray
        return @{
            Processed = $false
            Reason = "NoMembers"
        }
    }

    Write-Host "  Found $($members.Count) camelCase members:" -ForegroundColor Yellow

    $replacements = @()

    foreach ($member in $members) {
        Write-Host "    - $($member.Name) -> $($member.PascalCase) (line $($member.Line))" -ForegroundColor Cyan

        # Find all usages in project
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
            }
        }

        $replacements += @{
            OldName = $member.Name
            NewName = $member.PascalCase
            FilesChanged = $filesChanged
        }

        Write-Host "    Updated in $filesChanged files" -ForegroundColor Green
    }

    return @{
        Processed = $true
        Members = $members.Count
        Replacements = $replacements
    }
}

# Main script execution
Write-Host "========================================" -ForegroundColor Magenta
Write-Host "Fix Public/Internal PascalCase Script" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta

if ($TestMode) {
    Write-Host "`nTEST MODE: Processing first $TestFileCount files only" -ForegroundColor Yellow
}

# Get all .cs files
$allFiles = Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | Where-Object { -not $_.PSIsContainer }

Write-Host "`nFound $($allFiles.Count) C# files in project"

# Filter files without "variables names: ok" comment
$filesToProcess = @()
foreach ($file in $allFiles) {
    if (-not (Has-VariablesNamesOk $file.FullName)) {
        $filesToProcess += $file
    }
}

Write-Host "Files to process (without '// variables names: ok'): $($filesToProcess.Count)"

if ($TestMode) {
    $filesToProcess = $filesToProcess | Select-Object -First $TestFileCount
    Write-Host "Test mode: Processing $($filesToProcess.Count) files"
}

# Process files
$stats = @{
    Total = $filesToProcess.Count
    Processed = 0
    Skipped = 0
    MembersFixed = 0
}

foreach ($file in $filesToProcess) {
    $result = Process-File $file.FullName

    if ($result.Processed) {
        $stats.Processed++
        $stats.MembersFixed += $result.Members
    } else {
        $stats.Skipped++
    }
}

# Summary
Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "SUMMARY" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta
Write-Host "Total files: $($stats.Total)"
Write-Host "Processed: $($stats.Processed)" -ForegroundColor Green
Write-Host "Skipped: $($stats.Skipped)" -ForegroundColor Gray
Write-Host "Members fixed: $($stats.MembersFixed)" -ForegroundColor Cyan

if ($TestMode) {
    Write-Host "`nTest mode completed. Review changes and run without -TestMode to process all files." -ForegroundColor Yellow
}
