$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

Write-Host "Fixing BasePathsHelper.vs references..."

Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix all possible BasePathsHelper.vs patterns
    $newContent = $newContent -replace 'BasePathsHelper\.vs([^\w])', 'BasePathsHelper.Vs$1'
    $newContent = $newContent -replace 'BasePathsHelper\.vs$', 'BasePathsHelper.Vs'

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.FullName)"
    }
}

Write-Host "Done!"
