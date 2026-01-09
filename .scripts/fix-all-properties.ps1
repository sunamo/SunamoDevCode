Get-ChildItem -Path "SunamoDevCode" -Filter "*.cs" -Recurse | ForEach-Object {
    $content = Get-Content $_.FullName -Raw

    # FromToDC properties (from base class FromToTSHDC)
    $content = $content -replace '\bthis\.from\b', 'this.From'
    $content = $content -replace '\bthis\.to\b', 'this.To'
    $content = $content -replace '\bthis\.ftUse\b', 'this.FtUse'
    $content = $content -replace '\.ftUse\b', '.FtUse'
    $content = $content -replace '\.from\b', '.From'
    $content = $content -replace '\.to\b', '.To'

    # SolutionFolder properties
    $content = $content -replace '\.nameSolution\b', '.NameSolution'
    $content = $content -replace '\.slnNameWoExt\b', '.SlnNameWoExt'
    $content = $content -replace '\bnameSolution\b', 'NameSolution'
    $content = $content -replace '\.fullPathFolder\b', '.FullPathFolder'
    $content = $content -replace '\bfullPathFolder\b', 'FullPathFolder'
    $content = $content -replace '\.repository\b', '.Repository'
    $content = $content -replace '\.displayedText\b', '.DisplayedText'
    $content = $content -replace '\.projectFolder\b', '.ProjectFolder'
    $content = $content -replace '\.slnFullPath\b', '.SlnFullPath'

    # FindProjectsWhichIsSdkStyleResult properties (already fixed in previous script)
    $content = $content -replace '\bnonCsprojSdkStyleList\b', 'NonCsprojSdkStyleList'
    $content = $content -replace '\bcsprojSdkStyleList\b', 'CsprojSdkStyleList'
    $content = $content -replace '\bnetstandardList\b', 'NetstandardList'

    # IsProjectCsprojSdkStyleResult properties (already fixed in previous script)
    $content = $content -replace '\bisProjectCsprojSdkStyleIsCore\b', 'IsProjectCsprojSdkStyleIsCore'
    $content = $content -replace '\bisNetstandard\b', 'IsNetstandard'

    # FoldersWithSolutions properties
    $content = $content -replace '\.onlyRealLoadedSolutionsFolders\b', '.OnlyRealLoadedSolutionsFolders'

    # CollectionOnDriveArgs properties
    $content = $content -replace '\.LoadChangesFromDrive\b', '.IsLoadChangesFromDrive'

    # VisualStudioTempFse properties
    $content = $content -replace '\.gitFolderName\b', '.GitFolderName'

    # Fix variable name that doesn't exist
    $content = $content -replace '\bfwss\b(?=\s*=)', 'Fwss'

    Set-Content $_.FullName $content
}
