$files = Get-ChildItem -Path "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode" -Recurse -Filter "*.cs" |
    Where-Object {
        $_.Directory.Name -ne "obj" -and
        $_.Directory.Name -ne "bin" -and
        $_.Name -notlike "*.g.cs" -and
        $_.Name -ne "AssemblyInfo.cs" -and
        $_.Name -notlike "*AssemblyAttributes.cs" -and
        $_.Name -ne "GlobalUsings.cs"
    } |
    Select-Object -ExpandProperty FullName |
    Sort-Object

$files | ForEach-Object { Write-Output $_ }
Write-Output ""
Write-Output "Total files: $($files.Count)"
