$files = @(
    'SunamoDevCode\FileFormats\XmlLocalisationInterchangeFileFormat21.cs',
    'SunamoDevCode\FileFormats\XmlLocalisationInterchangeFileFormat22.cs',
    'SunamoDevCode\FileFormats\XmlLocalisationInterchangeFileFormat23.cs',
    'SunamoDevCode\FileFormats\XmlLocalisationInterchangeFileFormat24.cs'
)

foreach($file in $files) {
    $fullPath = Join-Path 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode' $file
    if (Test-Path $fullPath) {
        $content = Get-Content $fullPath -Raw
        $content = $content -replace '\.id\b', '.Id'
        $content = $content -replace '\.translate\b', '.Translate'
        $content = $content -replace '\.xml_space\b', '.XmlSpace'
        $content = $content -replace '\.source\b', '.Source'
        $content = $content -replace '\.target\b', '.Target'
        $content = $content -replace '\.path\b', '.Path'
        $content = $content -replace '\.group\b', '.Group'
        $content = $content -replace '\.xmlDocument\b', '.XmlDocument'
        $content = $content -replace '\.transUnits\b', '.TransUnits'
        $content = $content -replace '\.allIds\b', '.AllIds'
        Set-Content $fullPath -Value $content -NoNewline
        Write-Host "Fixed: $file" -ForegroundColor Green
    } else {
        Write-Host "Not found: $file" -ForegroundColor Red
    }
}

Write-Host "Done!" -ForegroundColor Cyan
