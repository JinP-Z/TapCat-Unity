# PowerShell script to fix animation file paths in TapCat project
# Copies actual animation frames from Sprites to Resources folder

Write-Host "=== TapCat Animation Path Fix ===" -ForegroundColor Cyan

$projectPath = "C:\Users\User\TapCat-Unity"
$sourceDir = Join-Path $projectPath "Assets\Sprites\CatAnimation"
$targetDir = Join-Path $projectPath "Assets\Resources\CatAnimation"

Write-Host "Source: $sourceDir"
Write-Host "Target: $targetDir"

# Check if source directory exists
if (-not (Test-Path $sourceDir)) {
    Write-Host "ERROR: Source directory not found!" -ForegroundColor Red
    exit 1
}

# Create target directory if it doesn't exist
if (-not (Test-Path $targetDir)) {
    Write-Host "Creating target directory..." -ForegroundColor Yellow
    New-Item -ItemType Directory -Path $targetDir -Force | Out-Null
}

# Get animation files from source
$sourceFiles = Get-ChildItem -Path $sourceDir -Filter "cat_anim_*.png"
Write-Host "Found $($sourceFiles.Count) animation files in source" -ForegroundColor Green

if ($sourceFiles.Count -eq 0) {
    Write-Host "ERROR: No animation files found in source!" -ForegroundColor Red
    exit 1
}

# Copy files
$copiedCount = 0
foreach ($file in $sourceFiles) {
    $targetPath = Join-Path $targetDir $file.Name
    
    # Check if target file exists and compare sizes
    if (Test-Path $targetPath) {
        $sourceSize = (Get-Item $file.FullName).Length
        $targetSize = (Get-Item $targetPath).Length
        
        if ($sourceSize -gt $targetSize) {
            # Source is larger, copy it
            Copy-Item -Path $file.FullName -Destination $targetPath -Force
            Write-Host "Updated: $($file.Name) ($sourceSize bytes)" -ForegroundColor Yellow
            $copiedCount++
        } else {
            Write-Host "Skipped: $($file.Name) (already up to date)" -ForegroundColor Gray
        }
    } else {
        # Target doesn't exist, copy it
        Copy-Item -Path $file.FullName -Destination $targetPath -Force
        Write-Host "Copied: $($file.Name)" -ForegroundColor Green
        $copiedCount++
    }
}

Write-Host "`n=== Summary ===" -ForegroundColor Cyan
Write-Host "Total files processed: $($sourceFiles.Count)"
Write-Host "Files copied/updated: $copiedCount"

# Check if we have all 10 frames
if ($sourceFiles.Count -ge 10) {
    Write-Host "✓ Animation frames complete (10+ frames)" -ForegroundColor Green
} else {
    Write-Host "⚠ Warning: Incomplete animation frames ($($sourceFiles.Count)/10)" -ForegroundColor Yellow
}

Write-Host "`nFix completed successfully!" -ForegroundColor Green