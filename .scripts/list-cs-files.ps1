Get-ChildItem -Path "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode" -Recurse -Filter "*.cs" |
    Where-Object { $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\' } |
    Select-Object -ExpandProperty FullName
