$files = @(
    "SunamoDevCode\SunamoSolutionsIndexer\FoldersWithSolutions1.cs",
    "SunamoDevCode\SunamoSolutionsIndexer\FoldersWithSolutionsInstance.cs",
    "SunamoDevCode\SunamoSolutionsIndexer\Data\SolutionFolderNs\SolutionFolder.cs",
    "SunamoDevCode\Aps\Data\SolutionFolderDetailedInfo.cs",
    "SunamoDevCode\SunamoSolutionsIndexer\Data\SolutionFoldersNs\SolutionFoldersSerialize.cs",
    "SunamoDevCode\SunamoSolutionsIndexer\Data\SolutionFolderNs\SolutionFolderWithFiles.cs"
)

$basePath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode"

foreach ($file in $files) {
    $fullPath = Join-Path $basePath $file
    if (Test-Path $fullPath) {
        $content = Get-Content $fullPath -Raw
        # Only replace when accessing through dot notation (interface members)
        $content = $content -replace '\.displayedText\b', '.DisplayedText'
        $content = $content -replace '\.fullPathFolder\b', '.FullPathFolder'
        $content = $content -replace '\.nameSolution\b', '.NameSolution'
        Set-Content $fullPath $content -NoNewline
        Write-Host "Fixed: $file"
    }
}
