# Analyze all camelCase public/internal members across the project
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
                    Line = ($content.Substring(0, $match.Index) -split "`n").Count
                }
            }
        }
    }

    return $members | Sort-Object -Property Name -Unique
}

Write-Host "========================================" -ForegroundColor Magenta
Write-Host "Analyzing camelCase public/internal members" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta

$allFiles = Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | Where-Object { -not $_.PSIsContainer }
$filesToProcess = @()

foreach ($file in $allFiles) {
    if (-not (Has-VariablesNamesOk $file.FullName)) {
        $filesToProcess += $file
    }
}

Write-Host "`nTotal C# files: $($allFiles.Count)"
Write-Host "Files without '// variables names: ok': $($filesToProcess.Count)"

# Collect all members across all files
$allMembers = @{}
$fileMembers = @{}

foreach ($file in $filesToProcess) {
    $members = Find-CamelCaseMembers $file.FullName

    if ($members.Count -gt 0) {
        $relativePath = $file.FullName.Replace($projectRoot + "\", "")
        $fileMembers[$relativePath] = $members

        foreach ($member in $members) {
            if (-not $allMembers.ContainsKey($member.Name)) {
                $allMembers[$member.Name] = @{
                    PascalCase = $member.PascalCase
                    Files = @()
                    Count = 0
                }
            }
            $allMembers[$member.Name].Files += $relativePath
            $allMembers[$member.Name].Count++
        }
    }
}

Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "Summary by Member Name" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta

# Sort members by risk (common short names are riskier)
$sortedMembers = $allMembers.GetEnumerator() | Sort-Object {
    $name = $_.Key
    # Risk scoring: shorter names and common words are riskier
    $risk = 0
    if ($name.Length -le 3) { $risk += 100 }
    elseif ($name.Length -le 5) { $risk += 50 }

    # Common words are riskier
    $commonWords = @("id", "file", "source", "target", "data", "value", "item", "type", "name")
    if ($commonWords -contains $name) { $risk += 200 }

    # More files = higher risk of false positives
    $risk += $_.Value.Count * 10

    return -$risk # Negative for descending sort (highest risk first)
}

foreach ($entry in $sortedMembers) {
    $memberName = $entry.Key
    $info = $entry.Value

    $riskLevel = "LOW"
    $riskColor = "Green"

    if ($memberName.Length -le 3 -or @("id", "file", "source", "target", "data", "value", "item", "type", "name") -contains $memberName) {
        $riskLevel = "HIGH"
        $riskColor = "Red"
    } elseif ($info.Count -gt 5) {
        $riskLevel = "MEDIUM"
        $riskColor = "Yellow"
    }

    Write-Host "`n[$riskLevel] " -ForegroundColor $riskColor -NoNewline
    Write-Host "$memberName -> $($info.PascalCase)" -ForegroundColor Cyan
    Write-Host "  Used in $($info.Count) file(s):" -ForegroundColor Gray

    foreach ($file in $info.Files) {
        Write-Host "    - $file" -ForegroundColor DarkGray
    }
}

Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "Files with Most Members" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta

$sortedFiles = $fileMembers.GetEnumerator() | Sort-Object { $_.Value.Count } -Descending | Select-Object -First 10

foreach ($entry in $sortedFiles) {
    $fileName = $entry.Key
    $members = $entry.Value

    Write-Host "`n$fileName ($($members.Count) members):" -ForegroundColor Yellow
    foreach ($member in $members) {
        Write-Host "  - $($member.Name) -> $($member.PascalCase) (line $($member.Line))" -ForegroundColor Gray
    }
}

Write-Host "`n========================================" -ForegroundColor Magenta
Write-Host "Recommendations" -ForegroundColor Magenta
Write-Host "========================================" -ForegroundColor Magenta
Write-Host "1. Start with LOW risk members (specific, long names)" -ForegroundColor Green
Write-Host "2. Manually review MEDIUM risk members before processing" -ForegroundColor Yellow
Write-Host "3. Be very careful with HIGH risk members (short/common names)" -ForegroundColor Red
Write-Host "4. Consider processing files individually for HIGH risk members" -ForegroundColor Red
