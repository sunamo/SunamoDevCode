# Find .cs files without "// variables names: ok" comment
$files = Get-ChildItem -Recurse -Filter '*.cs' | Where-Object {
    $_.FullName -notmatch '\\obj\\' -and $_.FullName -notmatch '\\bin\\'
}

$filesWithoutComment = $files | Where-Object {
    $content = Get-Content $_.FullName -Raw -ErrorAction SilentlyContinue
    $content -and $content -notmatch '//\s*variables\s+names:\s*ok'
}

Write-Output "Total source files: $($files.Count)"
Write-Output "Files WITHOUT 'variables names: ok' comment: $($filesWithoutComment.Count)"
Write-Output ""

# Output all files without the comment
$filesWithoutComment | ForEach-Object {
    $_.FullName.Replace((Get-Location).Path + '\', '')
} | Sort-Object
