using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TapCat
{
    /// <summary>
    /// 创建猫咪动画占位图
    /// 在没有实际猫咪动画帧时自动生成10帧彩色占位图
    /// </summary>
    public class CreatePlaceholderSprites : MonoBehaviour
    {
        [Header("占位图设置")]
        [SerializeField] private int textureSize = 128;
        [SerializeField] private string outputFolder = "Assets/Sprites/CatAnimation/";
        [SerializeField] private bool createOnStart = true;
        
        [Header("颜色设置")]
        [SerializeField] private Color[] frameColors = new Color[]
        {
            Color.red,
            new Color(1, 0.5f, 0), // 橙色
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            new Color(0.5f, 0, 1), // 紫色
            Color.magenta,
            Color.white,
            Color.gray
        };
        
        private void Start()
        {
            if (createOnStart)
            {
                CheckAndCreatePlaceholders();
            }
        }
        
        /// <summary>
        /// 检查并创建占位图
        /// </summary>
        public void CheckAndCreatePlaceholders()
        {
            Debug.Log("检查猫咪动画帧资源...");
            
            // 检查输出目录是否存在
            if (!Directory.Exists(outputFolder))
            {
                Debug.Log($"创建目录: {outputFolder}");
                Directory.CreateDirectory(outputFolder);
            }
            
            // 检查是否已有猫咪动画帧
            bool hasCatFrames = false;
            string[] pngFiles = Directory.GetFiles(outputFolder, "*.png");
            foreach (string file in pngFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.StartsWith("cat_anim_"))
                {
                    hasCatFrames = true;
                    break;
                }
            }
            
            if (!hasCatFrames)
            {
                Debug.Log("未找到猫咪动画帧，创建占位图...");
                CreatePlaceholderTextures();
            }
            else
            {
                Debug.Log("已找到猫咪动画帧，无需创建占位图");
            }
        }
        
        /// <summary>
        /// 创建占位图纹理
        /// </summary>
        private void CreatePlaceholderTextures()
        {
            for (int i = 0; i < 10; i++)
            {
                // 创建纹理
                Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
                
                // 选择颜色
                Color color = frameColors[i % frameColors.Length];
                
                // 填充纹理
                Color[] pixels = new Color[textureSize * textureSize];
                for (int p = 0; p < pixels.Length; p++)
                {
                    // 创建简单的图案（中心圆形）
                    int x = p % textureSize;
                    int y = p / textureSize;
                    float centerX = textureSize / 2f;
                    float centerY = textureSize / 2f;
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                    float radius = textureSize / 3f;
                    
                    if (distance < radius)
                    {
                        pixels[p] = color;
                    }
                    else if (distance < radius + 2)
                    {
                        pixels[p] = Color.black; // 边框
                    }
                    else
                    {
                        pixels[p] = new Color(0.2f, 0.2f, 0.2f, 0.5f); // 背景
                    }
                }
                
                texture.SetPixels(pixels);
                texture.Apply();
                
                // 保存纹理
                string fileName = $"cat_anim_{i:00}_placeholder.png";
                string filePath = Path.Combine(outputFolder, fileName);
                byte[] pngData = texture.EncodeToPNG();
                File.WriteAllBytes(filePath, pngData);
                
                Debug.Log($"创建占位图: {fileName}");
                
                // 清理
                Destroy(texture);
            }
            
            Debug.Log("占位图创建完成！");
            Debug.Log($"位置: {outputFolder}");
            Debug.Log("请将实际的猫咪动画帧（cat_anim_00.png到cat_anim_09.png）替换这些占位图");
            
            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }
        
        /// <summary>
        /// 手动创建占位图
        /// </summary>
        [ContextMenu("创建占位图")]
        private void CreatePlaceholdersManual()
        {
            CheckAndCreatePlaceholders();
        }
        
        /// <summary>
        /// 清理占位图
        /// </summary>
        [ContextMenu("清理占位图")]
        private void CleanupPlaceholders()
        {
            if (Directory.Exists(outputFolder))
            {
                string[] placeholderFiles = Directory.GetFiles(outputFolder, "*_placeholder.png");
                foreach (string file in placeholderFiles)
                {
                    File.Delete(file);
                    Debug.Log($"删除: {Path.GetFileName(file)}");
                }
                
                #if UNITY_EDITOR
                AssetDatabase.Refresh();
                #endif
                
                Debug.Log("占位图清理完成");
            }
        }
        
        /// <summary>
        /// 检查资源状态
        /// </summary>
        [ContextMenu("检查资源状态")]
        private void CheckResourceStatus()
        {
            if (!Directory.Exists(outputFolder))
            {
                Debug.LogWarning($"目录不存在: {outputFolder}");
                return;
            }
            
            string[] pngFiles = Directory.GetFiles(outputFolder, "*.png");
            int placeholderCount = 0;
            int actualFrameCount = 0;
            
            foreach (string file in pngFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.Contains("placeholder"))
                {
                    placeholderCount++;
                }
                else if (fileName.StartsWith("cat_anim_"))
                {
                    actualFrameCount++;
                }
            }
            
            Debug.Log("资源状态报告:");
            Debug.Log($"- 总PNG文件: {pngFiles.Length}");
            Debug.Log($"- 占位图数量: {placeholderCount}");
            Debug.Log($"- 实际动画帧: {actualFrameCount}");
            
            if (actualFrameCount >= 10)
            {
                Debug.Log("✓ 资源充足：已找到足够的猫咪动画帧");
            }
            else if (actualFrameCount > 0)
            {
                Debug.LogWarning($"⚠ 资源不足：只有{actualFrameCount}帧，需要10帧");
            }
            else
            {
                Debug.LogError("✗ 资源缺失：未找到猫咪动画帧");
                Debug.Log("建议：运行'创建占位图'或添加实际的cat_anim_00.png到cat_anim_09.png文件");
            }
        }
        
        /// <summary>
        /// 生成资源使用指南
        /// </summary>
        [ContextMenu("生成资源指南")]
        private void GenerateResourceGuide()
        {
            string guide = @"
            TapCat2D 资源使用指南
            ====================
            
            1. 必需资源：
               - 10张PNG图片：cat_anim_00.png 到 cat_anim_09.png
               - 建议尺寸：128x128 或 256x256
               - 透明背景（PNG支持Alpha通道）
            
            2. 放置位置：
               - Assets/Sprites/CatAnimation/
               - 或自定义目录（需在Inspector中设置）
            
            3. 资源状态：
               - 如果缺少资源，系统会自动创建彩色占位图
               - 占位图可用于测试，但建议替换为实际猫咪动画
            
            4. 导入设置建议：
               - Texture Type: Sprite (2D and UI)
               - Pixels Per Unit: 100
               - Filter Mode: Point (无插值)
               - Compression: None 或 Low Quality
            
            5. 命名规范：
               - 必须按顺序命名：cat_anim_00.png, cat_anim_01.png, ..., cat_anim_09.png
               - 数字必须是两位数（00, 01, ..., 09）
            
            6. 动画效果：
               - 每帧显示0.1秒（10 FPS）
               - 点击一次播放完整10帧序列
               - 循环播放直到序列结束
            
            7. 故障排除：
               - 如果动画不播放：检查图片是否已导入为Sprite
               - 如果颜色异常：检查图片格式和Alpha通道
               - 如果帧顺序错误：检查文件名数字顺序
            ";
            
            Debug.Log(guide);
        }
    }
}