$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

Write-Host "Fixing additional member references..."

Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix CSharpGeneratorArgs members
    $newContent = $newContent -replace '\.splitKeyWith\b', '.SplitKeyWith'
    $newContent = $newContent -replace '\.addHyphens\b', '.AddHyphens'
    $newContent = $newContent -replace '\.addingValue\b', '.AddingValue'

    # Fix FindProjectsWhichIsSdkStyleResult members
    $newContent = $newContent -replace '\.csprojSdkStyleList\b', '.CsprojSdkStyleList'
    $newContent = $newContent -replace '\.netstandardList\b', '.NetstandardList'
    $newContent = $newContent -replace '\.nonCsprojSdkStyleList\b', '.NonCsprojSdkStyleList'

    # Fix AllExtensions members
    $newContent = $newContent -replace 'AllExtensions\.old\b', 'AllExtensions.Old'

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.FullName)"
    }
}

Write-Host "Done!"
