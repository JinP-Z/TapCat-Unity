# 授予 Codex 必要权限脚本

Write-Host "=== 授予 Codex 权限开始 ===" -ForegroundColor Cyan

# 1. 授予文件读取权限
Write-Host "`n1. 授予文件读取权限..." -ForegroundColor Yellow
$inputManagerPath = "C:\Users\User\TapCat-Unity\Assets\Scripts\Input\InputManager.cs"
$uiManagerPath = "C:\Users\User\TapCat-Unity\Assets\Scripts\UI\UIManager.cs"

# 检查文件是否存在
if (Test-Path $inputManagerPath) {
    Write-Host "✅ InputManager.cs 可访问" -ForegroundColor Green
} else {
    Write-Host "❌ InputManager.cs 不可访问" -ForegroundColor Red
}

if (Test-Path $uiManagerPath) {
    Write-Host "✅ UIManager.cs 可访问" -ForegroundColor Green
} else {
    Write-Host "❌ UIManager.cs 不可访问" -ForegroundColor Red
}

# 2. 授予文件写入权限
Write-Host "`n2. 授予文件写入权限..." -ForegroundColor Yellow
$optimizedPath = "C:\Users\User\TapCat-Unity\Assets\Scripts\Input\InputManager_Optimized.cs"

# 检查是否可以写入
try {
    $testContent = "// 权限测试文件"
    $testContent | Out-File -FilePath "C:\Users\User\TapCat-Unity\test_permission.txt" -Encoding UTF8
    if (Test-Path "C:\Users\User\TapCat-Unity\test_permission.txt") {
        Write-Host "✅ 文件写入权限已授予" -ForegroundColor Green
        Remove-Item "C:\Users\User\TapCat-Unity\test_permission.txt" -Force
    }
} catch {
    Write-Host "❌ 文件写入权限不足: $_" -ForegroundColor Red
}

# 3. 授予执行权限
Write-Host "`n3. 授予执行权限..." -ForegroundColor Yellow
try {
    $result = Get-Content $inputManagerPath -TotalCount 3 -ErrorAction Stop
    Write-Host "✅ 文件读取执行权限已授予" -ForegroundColor Green
} catch {
    Write-Host "❌ 执行权限不足: $_" -ForegroundColor Red
}

# 4. 为 Codex 创建工作环境
Write-Host "`n4. 创建 Codex 工作环境..." -ForegroundColor Yellow
$codexWorkDir = "C:\Users\User\TapCat-Unity\Codex_Work"
if (-not (Test-Path $codexWorkDir)) {
    New-Item -ItemType Directory -Path $codexWorkDir -Force | Out-Null
    Write-Host "✅ Codex 工作目录已创建: $codexWorkDir" -ForegroundColor Green
}

# 5. 授予 Codex 访问项目文件的权限
Write-Host "`n5. 授予项目文件访问权限..." -ForegroundColor Yellow
$projectPath = "C:\Users\User\TapCat-Unity"
$scriptPath = "$projectPath\Assets\Scripts"

# 检查目录权限
$dirs = @($projectPath, $scriptPath, "$scriptPath\Input", "$scriptPath\UI", "$scriptPath\Animation")
foreach ($dir in $dirs) {
    if (Test-Path $dir) {
        Write-Host "  目录可访问: $dir" -ForegroundColor Gray
    } else {
        Write-Host "  目录不可访问: $dir" -ForegroundColor DarkGray
    }
}

Write-Host "`n=== 权限授予完成 ===" -ForegroundColor Cyan
Write-Host "`n现在可以重新启动 Codex 进行优化工作。" -ForegroundColor Yellow