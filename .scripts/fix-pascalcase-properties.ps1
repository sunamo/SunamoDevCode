$files = @(
    "SunamoDevCode\SunamoSolutionsIndexer\FoldersWithSolutions.cs",
    "SunamoDevCode\Aps\Projs\ApsProjsHelper.cs",
    "SunamoDevCode\Aps\Helpers\SunamoWebHelper.cs",
    "SunamoDevCode\Aps\ApsHelper.cs",
    "SunamoDevCode\GetSlns.cs",
    "SunamoDevCode\Aps\Projs\SunamoCsprojHelper1.cs",
    "SunamoDevCode\SunamoSolutionsIndexer\FoldersWithSolutionsInstance.cs"
)

$basePath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode"

foreach ($file in $files) {
    $fullPath = Join-Path $basePath $file
    if (Test-Path $fullPath) {
        $content = Get-Content $fullPath -Raw
        $content = $content -replace '\.projectsGetCsprojs', '.ProjectsGetCsprojs'
        $content = $content -replace '\.projectsInSolution', '.ProjectsInSolution'
        $content = $content -replace '\.typeProjectFolder', '.TypeProjectFolder'
        Set-Content $fullPath $content -NoNewline
        Write-Host "Fixed: $file"
    }
}
