# Find .cs files without "// variables names: ok" comment, excluding already-fixed files

$fixedFiles = @(
    "AllLists1.cs", "FS.cs", "SH.cs", "IMainWindowCsFileFilter.cs", "ApsMainWindow.cs",
    "CsFileFilter1.cs", "CSharpGenerator.cs", "CSharpGenerator2.cs", "TypeScriptGenerator.cs",
    "GeneratorCodeAbstract.cs", "TransUnit.cs", "TypeScriptHelper.cs", "PpkOnDriveDevCodeBase.cs",
    "ReplacerXlf.cs", "RepoGetFileMascGeneratorData.cs", "VersionHelper.cs", "ApsProjsHelper.cs",
    "SystemWindowsControls.cs", "TextOutputGeneratorDC.cs", "UnindexableHelper.cs", "ChangeProjects.cs",
    "VsProjectFile.cs", "SolutionsIndexerHelper.cs", "FoldersWithSolutions1.cs", "VsProjectsFileHelper3.cs",
    "CsprojShared.cs", "RemoveEmptyLinesService.cs", "CAG.cs", "FSGetFiles.cs", "SHFormat.cs",
    "SHTrim.cs", "ConvertTypeShortcutFullName.cs", "XmlNodeListExtensions.cs", "AppPaths.cs",
    "CsprojFileParser.cs", "DefaultPaths.cs", "GlobalUsingsHelper.cs", "DotnetOutputService.cs",
    "DotnetBuildOutputLine.cs", "RemoveCommentsHelper.cs", "ExtensionSortedCollection.cs",
    "CollectionWithoutDuplicatesBaseDC.cs", "CSharpGenerator1.cs", "CSharpHelper1.cs", "CSharpHelper2.cs",
    "GeneratorCpp.cs", "XmlLocalisationInterchangeFileFormat23.cs", "XmlLocalisationInterchangeFileFormat24.cs",
    "SqlGenerator.cs", "EnumItem.cs", "GetSlns.cs", "GenerateJson.cs", "FSGetFilesDC.cs",
    "Ignored.cs", "ProjectTypeGuid.cs", "TrimerTags.cs", "TF.cs", "XH.cs",
    "XmlGenerator.cs", "VueConsts.cs", "SunamoFramework.cs", "ProgressBar.cs", "SunamoWebHelper.cs",
    "VsProjectKeys.cs", "IMoveToShared.cs", "MoveToShared.cs", "UnindexableFiles.cs",
    "ExistsNonExistsList.cs", "ParseGlobalUsingsResult.cs", "Unindexable.cs"
)

$projectPath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"
$allFiles = Get-ChildItem -Path $projectPath -Filter "*.cs" -Recurse -File

$unfixedFiles = @()

foreach ($file in $allFiles) {
    $fileName = $file.Name

    # Skip fixed files
    if ($fixedFiles -contains $fileName) {
        continue
    }

    # Read first 5 lines to check for comment
    $firstLines = Get-Content $file.FullName -TotalCount 5
    $hasComment = $firstLines | Where-Object { $_ -match "//\s*variables\s+names:\s*ok" }

    if (-not $hasComment) {
        $unfixedFiles += $file.FullName
    }
}

Write-Host "Found $($unfixedFiles.Count) unfixed files:"
$unfixedFiles | ForEach-Object { Write-Host $_ }
