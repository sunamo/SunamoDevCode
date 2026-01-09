$rootPath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

$replacements = @(
    @{Pattern = '\bdisplayedText\b'; Replacement = 'DisplayedText'; Description = 'displayedText -> DisplayedText'},
    @{Pattern = '\bprojectFolder\b'; Replacement = 'ProjectFolder'; Description = 'projectFolder -> ProjectFolder'},
    @{Pattern = '\bslnFullPath\b'; Replacement = 'SlnFullPath'; Description = 'slnFullPath -> SlnFullPath'},
    @{Pattern = '\bnameSolution\b'; Replacement = 'NameSolution'; Description = 'nameSolution -> NameSolution'},
    @{Pattern = '\brepository\b(?=\s*[;\)])'; Replacement = 'Repository'; Description = 'repository -> Repository'},
    @{Pattern = '\bcheckForGit\b'; Replacement = 'CheckForGit'; Description = 'checkForGit -> CheckForGit'},
    @{Pattern = '\bftUse\b'; Replacement = 'FtUse'; Description = 'ftUse -> FtUse'},
    @{Pattern = '\b\.from\b'; Replacement = '.From'; Description = '.from -> .From'},
    @{Pattern = '\b\.to\b'; Replacement = '.To'; Description = '.to -> .To'}
)

$totalFixed = 0

foreach ($item in $replacements) {
    Write-Host "Processing: $($item.Description)..." -ForegroundColor Cyan

    $files = Get-ChildItem -Path $rootPath -Recurse -Filter "*.cs" | Where-Object {
        (Get-Content $_.FullName -Raw) -match $item.Pattern
    }

    foreach ($file in $files) {
        $content = Get-Content $file.FullName -Raw
        $newContent = $content -replace $item.Pattern, $item.Replacement

        if ($content -ne $newContent) {
            Set-Content $file.FullName -Value $newContent -NoNewline
            Write-Host "  Fixed in: $($file.Name)" -ForegroundColor Green
            $totalFixed++
        }
    }
}

Write-Host "`nTotal files fixed: $totalFixed" -ForegroundColor Yellow
Write-Host "Done!" -ForegroundColor Green
