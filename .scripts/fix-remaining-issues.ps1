Get-ChildItem -Path "SunamoDevCode" -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw

    # Fix remaining camelCase properties that should be PascalCase
    $content = $content -replace '\.documentsFolder\b', '.DocumentsFolder'
    $content = $content -replace '\bdisplayedText\b', 'DisplayedText'
    $content = $content -replace '\.empty\b', '.Empty'
    $content = $content -replace '\bthis\.empty\b', 'this.Empty'

    # Fix fwss to Fwss (but not in foreach where item might be called fwss)
    $content = $content -replace '\bfwss\.', 'Fwss.'
    $content = $content -replace '\bfwss\)', 'Fwss)'
    $content = $content -replace '\(fwss\b', '(Fwss'
    $content = $content -replace ' fwss ', ' Fwss '
    $content = $content -replace ' fwss;', ' Fwss;'

    # Fix SolutionsIndexerSettings property
    $content = $content -replace 'SolutionsIndexerSettings\.ignorePartAfterUnderscore', 'SolutionsIndexerSettings.IgnorePartAfterUnderscore'

    # Fix VisualStudioTempFse.gitFolderName (if it exists)
    $content = $content -replace 'VisualStudioTempFse\.gitFolderName', 'VisualStudioTempFse.GitFolderName'

    Set-Content $_.FullName $content
}
