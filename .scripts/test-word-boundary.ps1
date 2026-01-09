# Test word boundary approaches
$testString = @"
public string id;
var myid = id;
var identifier = "test";
this.id = id;
"@

Write-Host "Original text:" -ForegroundColor Cyan
Write-Host $testString
Write-Host ""

# Test 1: Simple replace (NO word boundaries) - DANGEROUS
Write-Host "Test 1: Simple -replace 'id', 'Id'" -ForegroundColor Yellow
$result1 = $testString -replace 'id', 'Id'
Write-Host $result1
Write-Host "PROBLEM: 'identifier' became 'Identifier', 'myid' became 'myId'!" -ForegroundColor Red
Write-Host ""

# Test 2: Word boundary with \b - doesn't work well in PowerShell
Write-Host "Test 2: -replace '\bid\b', 'Id'" -ForegroundColor Yellow
$result2 = $testString -replace '\bid\b', 'Id'
Write-Host $result2
Write-Host ""

# Test 3: [regex]::Replace with \b
Write-Host "Test 3: [regex]::Replace with '\bid\b'" -ForegroundColor Yellow
$result3 = [regex]::Replace($testString, '\bid\b', 'Id')
Write-Host $result3
Write-Host ""

# Test 4: Manual word boundary pattern
Write-Host "Test 4: Manual word boundary '(?<!\w)id(?!\w)'" -ForegroundColor Yellow
$result4 = $testString -replace '(?<!\w)id(?!\w)', 'Id'
Write-Host $result4
Write-Host ""

# Test 5: More specific - only alphanumeric boundaries
Write-Host "Test 5: '(?<![a-zA-Z0-9_])id(?![a-zA-Z0-9_])'" -ForegroundColor Yellow
$result5 = $testString -replace '(?<![a-zA-Z0-9_])id(?![a-zA-Z0-9_])', 'Id'
Write-Host $result5
