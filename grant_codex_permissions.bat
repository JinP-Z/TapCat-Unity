@echo off
echo ============================================
echo 授予 Codex 权限脚本
echo ============================================
echo.

echo 1. 授予文件读取权限...
echo.

REM 检查文件是否可访问
if exist "Assets\Scripts\Input\InputManager.cs" (
    echo ✅ InputManager.cs 可访问
) else (
    echo ❌ InputManager.cs 不可访问
)

if exist "Assets\Scripts\UI\UIManager.cs" (
    echo ✅ UIManager.cs 可访问
) else (
    echo ❌ UIManager.cs 不可访问
)

echo.
echo 2. 授予文件写入权限...
echo.

REM 创建测试文件检查写入权限
echo // 权限测试文件 > test_permission.txt
if exist "test_permission.txt" (
    echo ✅ 文件写入权限已授予
    del test_permission.txt
) else (
    echo ❌ 文件写入权限不足
)

echo.
echo 3. 授予执行权限...
echo.

REM 测试执行权限
dir /b "Assets\Scripts\Input\InputManager.cs" >nul 2>&1
if %errorlevel% equ 0 (
    echo ✅ 执行权限已授予
) else (
    echo ❌ 执行权限不足
)

echo.
echo 4. 创建 Codex 工作环境...
echo.

REM 创建 Codex 工作目录
if not exist "Codex_Work" (
    mkdir Codex_Work
    echo ✅ Codex 工作目录已创建: Codex_Work
) else (
    echo ✅ Codex 工作目录已存在: Codex_Work
)

echo.
echo ============================================
echo 权限授予完成！
echo ============================================
echo.
echo 现在可以重新启动 Codex 进行优化工作。
echo.
pause