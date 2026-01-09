# Fix XML documentation warnings by removing blank lines within XML comment blocks

$projectRoot = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

# Get all .cs files
$files = Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse

$totalFixed = 0

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content

    # Remove blank XML comment lines (lines that are just "///" with optional whitespace)
    # Pattern matches: start of line, optional whitespace, ///, optional whitespace, end of line + newline
    while ($content -match '(?m)^(\s*)///\s*\r?\n') {
        $content = $content -replace '(?m)^(\s*)///\s*\r?\n', ''
    }

    # Remove completely empty lines within XML documentation blocks
    # Pattern: XML comment line, followed by empty line(s), followed by another XML comment line
    # This removes empty lines between /// comment lines
    while ($content -match '(?m)(///[^\r\n]*\r?\n)\r?\n+(\s*///)') {
        $content = $content -replace '(?m)(///[^\r\n]*\r?\n)\r?\n+(\s*///)', '$1$2'
    }

    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -NoNewline
        $totalFixed++
        Write-Host "Fixed: $($file.FullName)"
    }
}

Write-Host "`nTotal files fixed: $totalFixed"
