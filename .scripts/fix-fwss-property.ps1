$files = @(
    "SunamoDevCode\Aps\ApsHelper.cs",
    "SunamoDevCode\Aps\ApsMainWindow.cs",
    "SunamoDevCode\Aps\ApsPluginStatic.cs",
    "SunamoDevCode\ToNetCore\research\RunnerShared.cs",
    "SunamoDevCode\SunamoSolutionsIndexer\SolutionsIndexerHelper.cs",
    "SunamoDevCode\ToNetCore\research\Runner.cs"
)

$basePath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode"

foreach ($file in $files) {
    $fullPath = Join-Path $basePath $file
    if (Test-Path $fullPath) {
        $content = Get-Content $fullPath -Raw
        $content = $content -replace 'FoldersWithSolutions\.fwss\b', 'FoldersWithSolutions.Fwss'
        Set-Content $fullPath $content -NoNewline
        Write-Host "Fixed: $file"
    }
}
