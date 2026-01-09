$rootPath = "E:\vs\Projects\PlatformIndependentNuGetPackages\SunamoDevCode\SunamoDevCode"

# Fix .fullPathFolder -> .FullPathFolder
$files = Get-ChildItem -Path $rootPath -Recurse -Filter "*.cs" | Where-Object {
    (Get-Content $_.FullName -Raw) -match '\.fullPathFolder\b'
}

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $newContent = $content -replace '\.fullPathFolder\b', '.FullPathFolder'
    if ($content -ne $newContent) {
        Set-Content $file.FullName -Value $newContent -NoNewline
        Write-Host "Fixed .fullPathFolder in: $($file.FullName)"
    }
}

# Fix .nameSolution -> .NameSolution
$files = Get-ChildItem -Path $rootPath -Recurse -Filter "*.cs" | Where-Object {
    (Get-Content $_.FullName -Raw) -match '\.nameSolution\b'
}

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $newContent = $content -replace '\.nameSolution\b', '.NameSolution'
    if ($content -ne $newContent) {
        Set-Content $file.FullName -Value $newContent -NoNewline
        Write-Host "Fixed .nameSolution in: $($file.FullName)"
    }
}

# Fix .projectFolder -> .ProjectFolder
$files = Get-ChildItem -Path $rootPath -Recurse -Filter "*.cs" | Where-Object {
    (Get-Content $_.FullName -Raw) -match '\.projectFolder\b'
}

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $newContent = $content -replace '\.projectFolder\b', '.ProjectFolder'
    if ($content -ne $newContent) {
        Set-Content $file.FullName -Value $newContent -NoNewline
        Write-Host "Fixed .projectFolder in: $($file.FullName)"
    }
}

# Fix .slnFullPath -> .SlnFullPath
$files = Get-ChildItem -Path $rootPath -Recurse -Filter "*.cs" | Where-Object {
    (Get-Content $_.FullName -Raw) -match '\.slnFullPath\b'
}

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $newContent = $content -replace '\.slnFullPath\b', '.SlnFullPath'
    if ($content -ne $newContent) {
        Set-Content $file.FullName -Value $newContent -NoNewline
        Write-Host "Fixed .slnFullPath in: $($file.FullName)"
    }
}

# Fix .displayedText -> .DisplayedText
$files = Get-ChildItem -Path $rootPath -Recurse -Filter "*.cs" | Where-Object {
    (Get-Content $_.FullName -Raw) -match '\.displayedText\b'
}

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    $newContent = $content -replace '\.displayedText\b', '.DisplayedText'
    if ($content -ne $newContent) {
        Set-Content $file.FullName -Value $newContent -NoNewline
        Write-Host "Fixed .displayedText in: $($file.FullName)"
    }
}

Write-Host "Done fixing all property names!"
