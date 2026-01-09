$files = Get-ChildItem -Path 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\FileFormats' -Filter 'XmlLocalisationInterchangeFileFormat*.cs'
foreach($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($content -match 'TransUnit\.tTransUnit') {
        $content = $content -replace 'TransUnit\.tTransUnit', 'TransUnit.TransUnitTagName'
        Set-Content $file.FullName -Value $content -NoNewline
        Write-Host "Fixed: $($file.Name)" -ForegroundColor Green
    }
}
Write-Host "Done!" -ForegroundColor Cyan
