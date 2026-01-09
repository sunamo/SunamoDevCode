$replacements = @{
    '\.displayedText\b' = '.DisplayedText'
    '\.projectFolder\b' = '.ProjectFolder'
    '\.slnFullPath\b' = '.SlnFullPath'
    '\.nameSolution\b' = '.NameSolution'
    '\.nameSolutionWithoutDiacritic\b' = '.NameSolutionWithoutDiacritic'
    '\.repository\b' = '.Repository'
    '\.fullPathFolder\b' = '.FullPathFolder'
}

$files = Get-ChildItem -Path 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode' -Filter '*.cs' -Recurse
$fixedCount = 0

foreach($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content

    foreach($pattern in $replacements.Keys) {
        $replacement = $replacements[$pattern]
        $content = $content -replace $pattern, $replacement
    }

    if ($content -ne $originalContent) {
        Set-Content $file.FullName -Value $content -NoNewline
        Write-Host "Fixed: $($file.FullName.Replace('E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\', ''))" -ForegroundColor Green
        $fixedCount++
    }
}

Write-Host "`nTotal files fixed: $fixedCount" -ForegroundColor Cyan
