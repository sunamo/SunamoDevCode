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
        # Revert only field/local variable access, NOT interface properties
        # This is tricky - we need to revert but keep interface implementations
        $content = $content -replace '([\s\.])(DisplayedText)\b(?!\s*\{)', '$1displayedText'
        $content = $content -replace '([\s\.])(FullPathFolder)\b(?!\s*\{)', '$1fullPathFolder'
        $content = $content -replace '([\s\.])(NameSolution)\b(?!\s*\{)', '$1nameSolution'
        Set-Content $fullPath $content -NoNewline
        Write-Host "Reverted: $file"
    }
}
