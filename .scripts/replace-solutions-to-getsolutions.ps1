$files = @(
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\Aps\ApsHelper2.cs',
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\ToNetCore\research\RunnerShared.cs',
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\SunamoSolutionsIndexer\FoldersWithSolutionsInstance.cs',
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\Aps\Helpers\SunamoWebHelper.cs',
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\Aps\ApsHelper.cs',
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\SunamoSolutionsIndexer\FoldersWithSolutions2.cs',
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\SunamoSolutionsIndexer\SolutionsIndexerHelper.cs',
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\SunamoSolutionsIndexer\FoldersWithSolutions.cs',
    'E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode\GetSlns.cs'
)

foreach ($file in $files) {
    $content = Get-Content -Path $file -Raw
    $newContent = $content -replace '\.Solutions\(', '.GetSolutions('
    Set-Content -Path $file -Value $newContent -NoNewline
    Write-Host "Updated: $file"
}
