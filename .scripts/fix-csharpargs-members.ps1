$projectRoot = 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode'

Write-Host "Fixing CSharpGeneratorArgs and related members..."

Get-ChildItem -Path $projectRoot -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content -Path $_.FullName -Raw
    $newContent = $content

    # Fix CSharpGeneratorArgs members - use spaces around for object initializers
    $newContent = $newContent -replace 'splitKeyWith\s*=', 'SplitKeyWith ='
    $newContent = $newContent -replace 'addHyphens\s*=', 'AddHyphens ='
    $newContent = $newContent -replace 'addingValue\s*=', 'AddingValue ='

    # Fix FindProjectsWhichIsSdkStyleResult members with dot prefix
    $newContent = $newContent -replace '\.csprojSdkStyleList', '.CsprojSdkStyleList'
    $newContent = $newContent -replace '\.netstandardList', '.NetstandardList'
    $newContent = $newContent -replace '\.nonCsprojSdkStyleList', '.NonCsprojSdkStyleList'

    # Fix AllExtensions members
    $newContent = $newContent -replace 'AllExtensions\.old', 'AllExtensions.Old'

    if ($content -ne $newContent) {
        Set-Content -Path $_.FullName -Value $newContent -NoNewline
        Write-Host "Fixed: $($_.Name)"
    }
}

Write-Host "Done!"
