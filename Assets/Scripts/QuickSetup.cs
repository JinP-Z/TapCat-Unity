// 快速设置脚本 - 简化用户操作
// 在编辑器中运行此脚本，自动完成所有设置

using UnityEngine;

public class QuickSetup : MonoBehaviour
{
    [Header("设置选项")]
    public bool setupSprites = true;
    public bool createController = true;
    public bool configureAnimation = true;
    
    [Header("动画帧")]
    public Sprite[] animationFrames;
    
    void Start()
    {
        Debug.Log("🚀 开始快速设置...");
        
        if (setupSprites)
        {
            SetupSpriteResources();
        }
        
        if (createController)
        {
            CreateTapCatController();
        }
        
        if (configureAnimation)
        {
            ConfigureAnimationSystem();
        }
        
        Debug.Log("✅ 快速设置完成！");
        Debug.Log("🎮 现在可以点击 Play 按钮测试游戏了！");
    }
    
    void SetupSpriteResources()
    {
        Debug.Log("检查图片资源...");
        
        // 这里可以添加自动检查资源的逻辑
        Debug.Log("💡 请确保所有猫咪图片已正确导入");
        Debug.Log("💡 导入设置：Sprite (2D and UI), Single, Pixels Per Unit: 100");
    }
    
    void CreateTapCatController()
    {
        Debug.Log("创建控制器...");
        
        // 检查是否已存在控制器
        TapCatAnimated existingController = FindObjectOfType<TapCatAnimated>();
        
        if (existingController != null)
        {
            Debug.Log("✅ 控制器已存在");
            return;
        }
        
        // 创建新的控制器 GameObject
        GameObject controllerObj = new GameObject("TapCatController");
        controllerObj.AddComponent<TapCatAnimated>();
        
        Debug.Log("✅ 控制器创建完成");
    }
    
    void ConfigureAnimationSystem()
    {
        Debug.Log("配置动画系统...");
        
        // 查找控制器
        TapCatAnimated controller = FindObjectOfType<TapCatAnimated>();
        if (controller == null)
        {
            Debug.LogError("❌ 未找到 TapCat 控制器");
            return;
        }
        
        // 确保动画组件存在
        SimpleCatAnimation anim = controller.gameObject.GetComponent<SimpleCatAnimation>();
        if (anim == null)
        {
            anim = controller.gameObject.AddComponent<SimpleCatAnimation>();
        }
        
        // 设置动画帧
        if (animationFrames != null && animationFrames.Length > 0)
        {
            anim.animationFrames = animationFrames;
            Debug.Log($"✅ 已设置 {animationFrames.Length} 帧动画");
        }
        else
        {
            Debug.Log("⚠️ 请手动设置动画帧数组");
            Debug.Log("💡 将 cat_anim_00.png 到 cat_anim_09.png 拖到 animationFrames 数组中");
        }
        
        // 连接控制器
        controller.catAnimation = anim;
        
        Debug.Log("✅ 动画系统配置完成");
    }
    
    // 编辑器工具
    [ContextMenu("运行快速设置")]
    void RunQuickSetup()
    {
        Start();
    }
    
    [ContextMenu("检查当前配置")]
    void CheckCurrentConfiguration()
    {
        Debug.Log("=== 当前配置检查 ===");
        
        // 检查控制器
        TapCatAnimated controller = FindObjectOfType<TapCatAnimated>();
        if (controller != null)
        {
            Debug.Log("✅ TapCat 控制器存在");
            
            // 检查动画系统
            if (controller.catAnimation != null)
            {
                SimpleCatAnimation anim = controller.catAnimation;
                Debug.Log(anim.animationFrames != null && anim.animationFrames.Length > 0 
                    ? $"✅ 动画系统已配置 ({anim.animationFrames.Length} 帧)" 
                    : "⚠️ 动画系统未设置资源");
            }
            else
            {
                Debug.Log("❌ 动画控制器未连接");
            }
        }
        else
        {
            Debug.Log("❌ TapCat 控制器不存在");
        }
        
        Debug.Log("=== 检查完成 ===");
    }
    
    [ContextMenu("创建测试场景")]
    void CreateTestScene()
    {
        Debug.Log("创建测试场景...");
        
        // 创建猫咪对象
        GameObject cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "TapCat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(3f, 3f, 0.2f);
        
        // 设置颜色

        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1f, 0.8f, 0f);
        }
        
        // 创建控制器

        GameObject controllerObj = new GameObject("TapCatController");
        TapCatAnimated controller = controllerObj.AddComponent<TapCatAnimated>();
        
        // 添加动画组件

        SimpleCatAnimation anim = controllerObj.AddComponent<SimpleCatAnimation>();
        controller.catAnimation = anim;
        
        Debug.Log("✅ 测试场景创建完成");
        Debug.Log("💡 现在需要手动设置动画帧数组");
    }
}