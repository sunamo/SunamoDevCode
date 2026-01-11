$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

Write-Host "Fixing all field references..."

Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix ci → Instance
    $newContent = $newContent -replace '\bci\.', 'Instance.'
    $newContent = $newContent -replace '\bci\s*=', 'Instance ='

    # Fix xlfSolutions → __xlfSolutions
    $newContent = $newContent -replace '\b_xlfSolutions\b', '__xlfSolutions'
    $newContent = $newContent -replace '(?<![_a-zA-Z])xlfSolutions\b', '__xlfSolutions'

    # Fix __types → ___types
    $newContent = $newContent -replace '\b__types\b', '___types'

    # Fix __defaultValueForType → ___defaultValueForType
    $newContent = $newContent -replace '\b__defaultValueForType\b', '___defaultValueForType'

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.FullName)"
    }
}

Write-Host "Done!"
