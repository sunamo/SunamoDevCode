# Find all .cs files with public/internal camelCase members WITHOUT "variables names: ok" comment

$targetDir = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"
$pattern = '^\s*(public|internal)\s+(static\s+)?(readonly\s+)?(const\s+)?[\w<>]+\s+[a-z_]'

# Get all .cs files
$allFiles = Get-ChildItem -Path $targetDir -Filter "*.cs" -Recurse | Select-Object -ExpandProperty FullName

$filesNeedingFix = @()

foreach ($file in $allFiles) {
    # Check if file has the pattern
    $content = Get-Content $file -Raw -ErrorAction SilentlyContinue
    if ($content -match $pattern) {
        # Check if file has the comment
        $firstLines = Get-Content $file -TotalCount 3 -ErrorAction SilentlyContinue
        $hasComment = $firstLines | Where-Object { $_ -match '// variables names: ok' }

        if (-not $hasComment) {
            $filesNeedingFix += $file
        }
    }
}

# Output count and list
Write-Host "Files needing PascalCase fixes: $($filesNeedingFix.Count)"
$filesNeedingFix | ForEach-Object { Write-Host $_ }
