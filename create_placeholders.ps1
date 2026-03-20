# 创建占位精灵文件的简单方法
$outputDir = "Assets/Sprites/CatAnimation"
New-Item -ItemType Directory -Force -Path $outputDir

# 创建10个简单的文本文件作为占位符
for ($i = 0; $i -lt 10; $i++) {
    $filename = "cat_anim_$($i.ToString('00')).png"
    $filepath = Join-Path $outputDir $filename
    
    # 创建一个简单的文本文件说明
    $content = "Placeholder for cat animation frame $i`r`nThis file should be replaced with actual cat animation sprite.`r`nFrame order: $i/10"
    
    # 创建文件
    Set-Content -Path $filepath -Value $content -Encoding UTF8
    Write-Host "Created: $filename"
}

Write-Host ""
Write-Host "✅ Created 10 placeholder files in $outputDir"
Write-Host "These files will be replaced with actual sprites when available."