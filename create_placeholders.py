#!/usr/bin/env python3
"""
创建10个占位精灵文件用于TapCat 2D动画
每个文件都是128x128的PNG，带有不同的颜色和帧号
"""

import os
from PIL import Image, ImageDraw, ImageFont
import colorsys

def create_placeholder_frame(frame_num, total_frames=10):
    """创建单个占位帧"""
    size = 128
    img = Image.new('RGBA', (size, size), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)
    
    # 计算颜色（彩虹色）
    hue = frame_num / total_frames
    r, g, b = colorsys.hsv_to_rgb(hue, 0.8, 0.9)
    color = (int(r * 255), int(g * 255), int(b * 255), 255)
    
    # 绘制圆形猫咪头像
    center = size // 2
    radius = 50
    draw.ellipse([center - radius, center - radius, 
                  center + radius, center + radius], 
                 fill=color, outline=(255, 255, 255, 255), width=3)
    
    # 绘制耳朵
    ear_size = 20
    # 左耳
    draw.polygon([
        (center - 30, center - 40),
        (center - 10, center - 60),
        (center + 10, center - 40)
    ], fill=color)
    # 右耳
    draw.polygon([
        (center + 30, center - 40),
        (center + 10, center - 60),
        (center - 10, center - 40)
    ], fill=color)
    
    # 绘制眼睛
    eye_size = 10
    draw.ellipse([center - 20 - eye_size//2, center - 10 - eye_size//2,
                  center - 20 + eye_size//2, center - 10 + eye_size//2],
                 fill=(255, 255, 255, 255))
    draw.ellipse([center + 20 - eye_size//2, center - 10 - eye_size//2,
                  center + 20 + eye_size//2, center - 10 + eye_size//2],
                 fill=(255, 255, 255, 255))
    
    # 绘制鼻子
    draw.polygon([
        (center - 5, center + 5),
        (center + 5, center + 5),
        (center, center + 15)
    ], fill=(255, 150, 150, 255))
    
    # 添加帧号
    try:
        font = ImageFont.truetype("arial.ttf", 20)
    except:
        font = ImageFont.load_default()
    
    text = f"{frame_num:02d}"
    text_bbox = draw.textbbox((0, 0), text, font=font)
    text_width = text_bbox[2] - text_bbox[0]
    text_height = text_bbox[3] - text_bbox[1]
    text_position = (center - text_width // 2, center + 30 - text_height // 2)
    
    draw.text(text_position, text, font=font, fill=(255, 255, 255, 255))
    
    return img

def main():
    """创建所有占位帧"""
    output_dir = "Assets/Sprites/CatAnimation"
    os.makedirs(output_dir, exist_ok=True)
    
    print(f"创建占位精灵文件到: {output_dir}")
    
    for i in range(10):
        img = create_placeholder_frame(i)
        filename = f"cat_anim_{i:02d}.png"
        filepath = os.path.join(output_dir, filename)
        img.save(filepath, "PNG")
        print(f"创建: {filename}")
    
    print("完成！已创建10个占位精灵文件。")

if __name__ == "__main__":
    main()