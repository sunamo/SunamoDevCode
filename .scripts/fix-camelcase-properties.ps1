# Fix camelCase property accesses to PascalCase
$rootPath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

# Mapping of old camelCase names to new PascalCase names
$replacements = @{
    # SolutionFolder properties
    "\.nameSolution" = ".NameSolution"
    "\.fullPathFolder" = ".FullPathFolder"
    "\.slnNameWoExt" = ".SlnNameWithoutExtension"
    "\.projectsGetCsprojs" = ".ProjectsGetCsprojs"
    "\.nameSolutionWithoutDiacritic" = ".NameSolutionWithoutDiacritic"
    "sln\.projectsGetCsprojs" = "sln.ProjectsGetCsprojs"

    # XlfData properties
    "\.trans_units" = ".TransUnits"

    # Static class properties
    "ApsHelper\.ci" = "ApsHelper.Instance"
    "AllExtensions\.sln" = "AllExtensions.Sln"
    "AllExtensions\.json" = "AllExtensions.Json"
    "BasePathsHelper\.vs" = "BasePathsHelper.Vs"
    "SNumConsts\.mTwo" = "SNumConsts.MTwo"

    # CollectionWithoutDuplicatesDC properties
    "\.c\.Add" = ".InnerCollection.Add"
    "\.c\.Count" = ".InnerCollection.Count"
    "\.c\.Clear" = ".InnerCollection.Clear"

    # VisualStudioTempFse properties
    "VisualStudioTempFse\.foldersInSolutionToDelete" = "VisualStudioTempFse.FoldersInSolutionToDelete"
    "VisualStudioTempFse\.foldersInSolutionDownloaded" = "VisualStudioTempFse.FoldersInSolutionDownloaded"
    "VisualStudioTempFse\.filesInSolutionToDelete" = "VisualStudioTempFse.FilesInSolutionToDelete"
    "VisualStudioTempFse\.filesInSolutionDownloaded" = "VisualStudioTempFse.FilesInSolutionDownloaded"
    "VisualStudioTempFse\.foldersInProjectToDelete" = "VisualStudioTempFse.FoldersInProjectToDelete"
    "VisualStudioTempFse\.filesInProjectToDelete" = "VisualStudioTempFse.FilesInProjectToDelete"

    # AsyncPushSolutions properties
    "AsyncPushSolutions\.foldersWithSolutions" = "AsyncPushSolutions.FoldersWithSolutions"
    "AsyncPushSolutions\.release" = "AsyncPushSolutions.Release"
    "AsyncPushSolutions\.gitBashBuilder" = "AsyncPushSolutions.GitBashBuilder"
    "AsyncPushSolutions\.pushArgs" = "AsyncPushSolutions.PushArgs"
    "AsyncPushSolutions\.commitMessage" = "AsyncPushSolutions.CommitMessage"
    "AsyncPushSolutions\.pushSolutionsData" = "AsyncPushSolutions.PushSolutionsData"
    "AsyncPushSolutions\.gitStatus" = "AsyncPushSolutions.GitStatus"

    # ProjectReferences properties
    "ProjectReferences\.projs" = "ProjectReferences.Projects"
    "ProjectReferences\.nodes" = "ProjectReferences.Nodes"

    # VpsHelperDevCode properties
    "VpsHelperDevCode\.listVpsNew" = "VpsHelperDevCode.ListVpsNew"

    # IsProjectCsprojSdkStyleResult properties
    "IsProjectCsprojSdkStyleResult\.content" = "IsProjectCsprojSdkStyleResult.Content"
    "IsProjectCsprojSdkStyleResult\.isProjectCsprojSdkStyleIsCore" = "IsProjectCsprojSdkStyleResult.IsProjectCsprojSdkStyleIsCore"

    # Local variables in SolutionFolderDetailedInfo (field names)
    "displayedText = " = "DisplayedText = "
    "fullPathFolder = " = "FullPathFolder = "
    "nameSolutionWithoutDiacritic = " = "NameSolutionWithoutDiacritic = "
}

# Get all .cs files
$csFiles = Get-ChildItem -Path $rootPath -Filter "*.cs" -Recurse | Where-Object { $_.FullName -notmatch "\\obj\\" -and $_.FullName -notmatch "\\bin\\" }

$filesModified = 0

foreach ($file in $csFiles) {
    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    $originalContent = $content

    foreach ($pattern in $replacements.Keys) {
        $replacement = $replacements[$pattern]
        $content = $content -replace $pattern, $replacement
    }

    if ($content -ne $originalContent) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8 -NoNewline
        $filesModified++
        Write-Host "Modified: $($file.FullName)"
    }
}

Write-Host "`nTotal files modified: $filesModified"
