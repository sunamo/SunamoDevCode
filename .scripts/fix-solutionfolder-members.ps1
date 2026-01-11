$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

Write-Host "Fixing SolutionFolder member references..."

Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix SolutionFolder member references
    $newContent = $newContent -replace '\.projectsGetCsprojs\b', '.ProjectsGetCsprojs'
    $newContent = $newContent -replace '\.fullPathFolder\b', '.FullPathFolder'
    $newContent = $newContent -replace '\.nameSolutionWithoutDiacritic\b', '.NameSolutionWithoutDiacritic'
    $newContent = $newContent -replace '\.repository\b', '.Repository'

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.FullName)"
    }
}

Write-Host "Done!"
