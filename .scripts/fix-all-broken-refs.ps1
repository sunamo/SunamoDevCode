$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix private static fields with single underscore to double underscore
    $newContent = $newContent -replace '\b_xlfSolutions\b', '__xlfSolutions'
    $newContent = $newContent -replace '(?<!_)xlfSolutions(?![a-zA-Z])', '__xlfSolutions'
    $newContent = $newContent -replace '\b__types(?![a-zA-Z])', '___types'
    $newContent = $newContent -replace '\b__defaultValueForType\b', '___defaultValueForType'

    # Fix ci (this might be a class instance, need to check what it was renamed to)
    # Skip for now until we find what it should be

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.FullName)"
    }
}

Write-Host "Field references fixed."
