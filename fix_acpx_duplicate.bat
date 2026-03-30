@echo off
echo ============================================
echo 修复 ACPX 重复插件注册
echo ============================================
echo.

echo 正在检查 ACPX 插件配置...
echo.

REM 1. 检查当前的插件配置
echo 1. 检查插件配置...
openclaw config show plugins.entries.acpx

echo.
echo 2. 清理重复的 ACPX 插件...
echo.

REM 2. 创建一个修复配置的脚本
powershell -Command "
# 读取当前配置文件
`$configPath = 'C:\Users\User\.openclaw\openclaw.json'
`$config = Get-Content `$configPath | ConvertFrom-Json

# 检查是否有重复的 ACPX 插件
`$acpxEntries = `$config.plugins.entries.PSObject.Properties | Where-Object { `$_.Name -eq 'acpx' }

if (`$acpxEntries.Count -gt 1) {
    echo '发现重复的 ACPX 插件注册'
    
    # 只保留第一个 ACPX 插件
    `$uniqueEntries = @{}
    foreach (`$entry in `$config.plugins.entries.PSObject.Properties) {
        if (`$entry.Name -eq 'acpx') {
            if (-not `$uniqueEntries.ContainsKey('acpx')) {
                `$uniqueEntries['acpx'] = `$entry.Value
            }
        } else {
            `$uniqueEntries[`$entry.Name] = `$entry.Value
        }
    }
    
    # 更新配置
    `$config.plugins.entries = `$uniqueEntries
    
    # 保存配置文件
    `$config | ConvertTo-Json -Depth 10 | Set-Content `$configPath
    echo '✅ 已修复重复插件注册'
} else {
    echo '✅ ACPX 插件配置正常'
}
"

echo.
echo 3. 重新启动 OpenClaw 网关...
echo.

REM 3. 重新启动网关
openclaw gateway restart

echo.
echo ============================================
echo 修复完成！
echo ============================================
echo.
pause