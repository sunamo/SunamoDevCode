# Script to fix case sensitivity errors in bulk

$replacements = @(
    # SolutionFolder properties
    @{ File = "**\*.cs"; Old = "\.typeProjectFolder\b"; New = ".TypeProjectFolder"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.projectsGetCsprojs\b"; New = ".ProjectsGetCsprojs"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.repository\b"; New = ".Repository"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.nameSolutionWithoutDiacritic\b"; New = ".NameSolutionWithoutDiacritic"; IsRegex = $true },

    # VisualStudioTempFse properties
    @{ File = "**\*.cs"; Old = "\.gitFolderName\b"; New = ".GitFolderName"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.foldersInSolutionDownloaded\b"; New = ".FoldersInSolutionDownloaded"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.foldersInSolutionToDelete\b"; New = ".FoldersInSolutionToDelete"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.foldersInProjectToDelete\b"; New = ".FoldersInProjectToDelete"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.foldersAnywhereToDelete\b"; New = ".FoldersAnywhereToDelete"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.foldersInProjectDownloaded\b"; New = ".FoldersInProjectDownloaded"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.foldersAnywhereDownloaded\b"; New = ".FoldersAnywhereDownloaded"; IsRegex = $true },

    # IsNetCore5UpMonikerResult properties
    @{ File = "**\*.cs"; Old = "\.targetFramework\b"; New = ".TargetFramework"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.platformTfm\b"; New = ".PlatformTfm"; IsRegex = $true },

    # RemoveFromXlfWhichHaveEmptyTargetOrSourceArgs properties
    @{ File = "**\*.cs"; Old = "\.removeWholeTransUnit\b"; New = ".RemoveWholeTransUnit"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.save\b"; New = ".Save"; IsRegex = $true },

    # XlfData properties
    @{ File = "**\*.cs"; Old = "\.xd\b"; New = ".XmlDocument"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.path\b"; New = ".Path"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.group\b"; New = ".Group"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.allids\b"; New = ".AllIds"; IsRegex = $true },

    # TransUnit properties
    @{ File = "**\*.cs"; Old = "\.tTransUnit\b"; New = ".TransUnitTagName"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.id\b"; New = ".Id"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.source\b"; New = ".Source"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.target\b"; New = ".Target"; IsRegex = $true },

    # FindProjectsWhichIsSdkStyleResult properties
    @{ File = "**\*.cs"; Old = "\.csprojSdkStyleList\b"; New = ".CsprojSdkStyleList"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.netstandardList\b"; New = ".NetstandardList"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.nonCsprojSdkStyleList\b"; New = ".NonCsprojSdkStyleList"; IsRegex = $true },

    # IsProjectCsprojSdkStyleResult properties
    @{ File = "**\*.cs"; Old = "\.isProjectCsprojSdkStyleIsCore\b"; New = ".IsProjectCsprojSdkStyleIsCore"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.isNetstandard\b"; New = ".IsNetstandard"; IsRegex = $true },

    # CSharpGeneratorArgs properties
    @{ File = "**\*.cs"; Old = "\.splitKeyWith\b"; New = ".SplitKeyWith"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.addHyphens\b"; New = ".AddHyphens"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "\.addingValue\b"; New = ".AddingValue"; IsRegex = $true },

    # AllExtensions properties
    @{ File = "**\*.cs"; Old = "AllExtensions\.zip\b"; New = "AllExtensions.Zip"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "AllExtensions\.cs\b"; New = "AllExtensions.Cs"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "AllExtensions\.ts\b"; New = "AllExtensions.Ts"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "AllExtensions\.exe\b"; New = "AllExtensions.Exe"; IsRegex = $true },
    @{ File = "**\*.cs"; Old = "AllExtensions\.old\b"; New = "AllExtensions.Old"; IsRegex = $true }
)

Write-Host "Starting case sensitivity fixes..." -ForegroundColor Cyan

foreach ($rep in $replacements) {
    Write-Host "Fixing: $($rep.Old) -> $($rep.New)" -ForegroundColor Yellow

    if ($rep.IsRegex) {
        # Use ripgrep to find files and PowerShell to replace
        $files = rg -l $rep.Old "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode" 2>$null
        foreach ($file in $files) {
            if (Test-Path $file) {
                $content = Get-Content $file -Raw
                $newContent = $content -replace $rep.Old, $rep.New
                if ($content -ne $newContent) {
                    Set-Content -Path $file -Value $newContent -NoNewline
                    Write-Host "  Updated: $file" -ForegroundColor Green
                }
            }
        }
    }
}

Write-Host "Done!" -ForegroundColor Cyan
