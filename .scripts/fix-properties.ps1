Get-ChildItem -Path "SunamoDevCode" -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $content = $content -replace 'nonCsprojSdkStyleList', 'NonCsprojSdkStyleList'
    $content = $content -replace 'csprojSdkStyleList', 'CsprojSdkStyleList'
    $content = $content -replace 'netstandardList', 'NetstandardList'
    $content = $content -replace 'isProjectCsprojSdkStyleIsCore', 'IsProjectCsprojSdkStyleIsCore'
    $content = $content -replace 'isNetstandard', 'IsNetstandard'
    $content = $content -replace '\.fullPathFolder', '.FullPathFolder'
    Set-Content $_.FullName $content
}
