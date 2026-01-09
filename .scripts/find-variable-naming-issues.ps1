# Find C# files with potential variable naming issues
# Excludes files that already have "// variables names: ok" comment

$projectPath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"
$issues = @()

Get-ChildItem -Path $projectPath -Filter "*.cs" -Recurse |
    Where-Object { $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\' } |
    ForEach-Object {
        $content = Get-Content $_.FullName -Raw

        # Skip if file has "variables names: ok" comment
        if ($content -match '^// variables names: ok') {
            return
        }

        $relativePath = $_.FullName.Replace("$projectPath\", "")

        # Check for single-letter parameters (excluding i, j, k in for loops)
        if ($content -match '\(\s*\w+\s+[a-hm-z]\s*[,\)]') {
            $issues += "$relativePath - Single-letter parameter found"
        }

        # Check for camelCase public/internal members
        if ($content -match '(public|internal)\s+static\s+\w+\s+[a-z]\w+\s*[=;{]') {
            $issues += "$relativePath - camelCase public/internal static member found"
        }

        # Check for missing XML docs on public class
        if ($content -match 'public\s+class\s+\w+' -and $content -notmatch '/// <summary>') {
            $issues += "$relativePath - Missing XML documentation"
        }
    }

Write-Host "Found $($issues.Count) potential issues:"
$issues | ForEach-Object { Write-Host $_ }
