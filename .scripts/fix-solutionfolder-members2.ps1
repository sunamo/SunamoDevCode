$replacements = @(
    @('.displayedText', '.DisplayedText'),
    @('.projectFolder', '.ProjectFolder'),
    @('.slnFullPath', '.SlnFullPath'),
    @('.nameSolution', '.NameSolution'),
    @('.nameSolutionWithoutDiacritic', '.NameSolutionWithoutDiacritic'),
    @('.repository', '.Repository'),
    @('.fullPathFolder', '.FullPathFolder')
)

$files = Get-ChildItem -Path 'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode' -Filter '*.cs' -Recurse
$fixedCount = 0

foreach($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $originalContent = $content
    $changed = $false

    foreach($pair in $replacements) {
        $old = $pair[0]
        $new = $pair[1]
        if ($content.Contains($old)) {
            $content = $content.Replace($old, $new)
            $changed = $true
        }
    }

    if ($changed) {
        Set-Content $file.FullName -Value $content -NoNewline
        Write-Host "Fixed: $($file.FullName.Replace('E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\', ''))" -ForegroundColor Green
        $fixedCount++
    }
}

Write-Host "`nTotal files fixed: $fixedCount" -ForegroundColor Cyan
