using UnityEngine;

/// <summary>
/// 自动 TapCat 设置脚本
/// 场景启动时自动创建所有必要组件
/// 用户只需点击 Play 即可验收
/// </summary>
public class AutoTapCatSetup : MonoBehaviour
{
    void Start()
    {
        Debug.Log("🔧 开始自动 TapCat 设置...");
        
        // 1. 确保主相机位置正确
        SetupCamera();
        
        // 2. 创建 TapCat 控制器
        CreateTapCatController();
        
        // 3. 检查动画资源
        CheckAnimationResources();
        
        Debug.Log("✅ 自动设置完成！");
        Debug.Log("🎮 现在可以点击 Play 测试游戏了！");
        
        // 自动销毁自己，清理场景
        Destroy(gameObject, 2f);
    }
    
    void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.transform.position = new Vector3(0, 0, -10);
            Debug.Log("✅ 创建主相机");
        }
        else
        {
            mainCamera.transform.position = new Vector3(0, 0, -10);
            Debug.Log("✅ 调整主相机位置");
        }
    }
    
    void CreateTapCatController()
    {
        // 检查是否已存在 FinalTapCat_Animated 组件
        FinalTapCat_Animated existingController = FindObjectOfType<FinalTapCat_Animated>();
        
        if (existingController != null)
        {
            Debug.Log("✅ TapCat 控制器已存在");
            return;
        }
        
        // 创建新的控制器 GameObject
        GameObject controllerObj = new GameObject("TapCatController");
        controllerObj.AddComponent<FinalTapCat_Animated>();
        
        Debug.Log("✅ 创建 TapCat 控制器");
    }
    
    void CheckAnimationResources()
    {
        Debug.Log("📁 检查动画资源...");
        
        string spritePath = Application.dataPath + "/Sprites/CatAnimation/";
        if (System.IO.Directory.Exists(spritePath))
        {
            int pngCount = System.IO.Directory.GetFiles(spritePath, "*.png").Length;
            
            if (pngCount >= 10)
            {
                Debug.Log($"✅ 动画资源完整 ({pngCount} 个 PNG 文件)");
                Debug.Log("💡 资源位置: Assets/Sprites/CatAnimation/");
                Debug.Log("💡 需要在 Unity 编辑器中设置 Sprite 导入属性");
            }
            else if (pngCount > 0)
            {
                Debug.LogWarning($"⚠️ 动画资源不完整: {pngCount}/10 帧");
            }
            else
            {
                Debug.LogWarning("⚠️ 动画资源目录为空");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ 动画资源目录不存在: Assets/Sprites/CatAnimation/");
        }
        
        // 提供设置指南
        Debug.Log("📝 设置指南：");
        Debug.Log("   1. 在 Project 面板中，打开 Assets/Sprites/CatAnimation/");
        Debug.Log("   2. 全选所有 PNG 文件");
        Debug.Log("   3. 在 Inspector 中设置：");
        Debug.Log("      - Texture Type: Sprite (2D and UI)");
        Debug.Log("      - Sprite Mode: Single");
        Debug.Log("      - Pixels Per Unit: 100");
        Debug.Log("      - Filter Mode: Point (no filter)");
        Debug.Log("      - Compression: None");
        Debug.Log("   4. 点击 Apply");
        Debug.Log("   5. 选择 TapCatController GameObject");
        Debug.Log("   6. 将图片拖到 FinalTapCat_Animated 组件的 animationFrames 数组中");
    }
    
    // 编辑器工具
    [ContextMenu("运行自动设置")]
    void RunAutoSetup()
    {
        Start();
    }
    
    [ContextMenu("检查当前场景")]
    void CheckCurrentScene()
    {
        Debug.Log("=== 场景检查 ===");
        
        // 检查相机
        Camera mainCamera = Camera.main;
        Debug.Log(mainCamera != null ? "✅ 主相机存在" : "❌ 主相机不存在");
        
        // 检查控制器
        FinalTapCat_Animated controller = FindObjectOfType<FinalTapCat_Animated>();
        if (controller != null)
        {
            Debug.Log("✅ TapCat 控制器存在");
            
            // 检查猫咪对象
            GameObject cat = GameObject.Find("TapCat");
            Debug.Log(cat != null ? "✅ 猫咪对象存在" : "❌ 猫咪对象不存在");
        }
        else
        {
            Debug.Log("❌ TapCat 控制器不存在");
        }
        
        // 检查资源
        string spritePath = Application.dataPath + "/Sprites/CatAnimation/";
        if (System.IO.Directory.Exists(spritePath))
        {
            int pngCount = System.IO.Directory.GetFiles(spritePath, "*.png").Length;
            Debug.Log($"📸 动画资源: {pngCount}/10 帧");
        }
        else
        {
            Debug.Log("❌ 动画资源目录不存在");
        }
        
        Debug.Log("=== 检查完成 ===");
    }
}