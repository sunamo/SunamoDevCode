# Fix SolutionFolder property access throughout the codebase
# Only fix property access via dot notation, preserve field declarations

Get-ChildItem -Path "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode" -Filter "*.cs" -Recurse |
    Where-Object { $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\' } |
    ForEach-Object {
        $content = Get-Content $_.FullName -Raw
        $originalContent = $content

        # Fix interface properties from ISolutionFolderSerialize
        $content = $content -replace '([a-zA-Z_]\w*)\.fullPathFolder\b', '$1.FullPathFolder'
        $content = $content -replace '([a-zA-Z_]\w*)\.displayedText\b', '$1.DisplayedText'
        $content = $content -replace '([a-zA-Z_]\w*)\.nameSolution\b', '$1.NameSolution'

        # Fix other common patterns (protected fields that should be properties)
        $content = $content -replace '([a-zA-Z_]\w*)\.projectFolder\b', '$1.ProjectFolder'
        $content = $content -replace '([a-zA-Z_]\w*)\.slnFullPath\b', '$1.SlnFullPath'
        $content = $content -replace '([a-zA-Z_]\w*)\.slnNameWoExt\b', '$1.SlnNameWoExt'
        $content = $content -replace '([a-zA-Z_]\w*)\.nameSolutionWithoutDiacritic\b', '$1.NameSolutionWithoutDiacritic'
        $content = $content -replace '([a-zA-Z_]\w*)\.repository\b', '$1.Repository'
        $content = $content -replace '([a-zA-Z_]\w*)\.documentsFolder\b', '$1.DocumentsFolder'

        if ($content -ne $originalContent) {
            Set-Content $_.FullName $content -NoNewline
            Write-Host "Fixed: $($_.FullName)"
        }
    }
