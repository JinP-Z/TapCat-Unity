// 自动场景设置脚本
// 确保点击 Play 时自动创建所有必要组件

using UnityEngine;

public class AutoSceneSetup : MonoBehaviour
{
    [Header("自动设置选项")]
    public bool createTapCatObject = true;
    public bool setupAnimation = true;
    public bool loadResources = true;
    
    [Header("资源路径")]
    public string spritesPath = "Sprites/CatAnimation/";
    
    void Start()
    {
        Debug.Log("🔧 开始自动场景设置...");
        
        if (createTapCatObject)
        {
            SetupTapCatObject();
        }
        
        if (setupAnimation)
        {
            SetupAnimationSystem();
        }
        
        if (loadResources)
        {
            LoadAnimationResources();
        }
        
        Debug.Log("✅ 自动场景设置完成");
        Debug.Log("🎮 现在可以点击 Play 测试游戏了！");
    }
    
    void SetupTapCatObject()
    {
        // 检查是否已存在 TapCat 对象
        GameObject tapCat = GameObject.Find("TapCat");
        
        if (tapCat == null)
        {
            Debug.Log("创建 TapCat 对象...");
            
            // 创建新的猫咪对象
            tapCat = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tapCat.name = "TapCat";
            tapCat.transform.position = Vector3.zero;
            tapCat.transform.localScale = new Vector3(3f, 3f, 0.2f);
            
            // 设置颜色
            Renderer renderer = tapCat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(1f, 0.8f, 0f);
            }
            
            Debug.Log("✅ TapCat 对象创建完成");
        }
        else
        {
            Debug.Log("✅ TapCat 对象已存在");
        }
    }
    
    void SetupAnimationSystem()
    {
        // 查找主控制器
        TapCatAnimated controller = FindObjectOfType<TapCatAnimated>();
        
        if (controller == null)
        {
            Debug.Log("创建 TapCat 控制器...");
            
            // 创建空 GameObject 并添加控制器
            GameObject controllerObj = new GameObject("TapCatController");
            controller = controllerObj.AddComponent<TapCatAnimated>();
            
            Debug.Log("✅ TapCat 控制器创建完成");
        }
        else
        {
            Debug.Log("✅ TapCat 控制器已存在");
        }
        
        // 确保动画控制器存在
        if (controller.catAnimation == null)
        {
            Debug.Log("设置动画控制器...");
            
            SimpleCatAnimation anim = controller.gameObject.GetComponent<SimpleCatAnimation>();
            if (anim == null)
            {
                anim = controller.gameObject.AddComponent<SimpleCatAnimation>();
            }
            
            controller.catAnimation = anim;
            Debug.Log("✅ 动画控制器设置完成");
        }
    }
    
    void LoadAnimationResources()
    {
        // 尝试加载动画帧
        SimpleCatAnimation anim = FindObjectOfType<SimpleCatAnimation>();
        if (anim == null) return;
        
        if (anim.animationFrames == null || anim.animationFrames.Length == 0)
        {
            Debug.Log("尝试加载动画资源...");
            
            // 这里可以添加自动加载资源的逻辑
            // 由于资源加载需要编辑器环境，这里只记录信息
            Debug.Log("📁 动画资源路径: " + spritesPath);
            Debug.Log("💡 请在编辑器中手动设置动画帧数组");
            Debug.Log("💡 将 cat_anim_00.png 到 cat_anim_09.png 拖到 animationFrames 数组中");
        }
        else
        {
            Debug.Log($"✅ 已加载 {anim.animationFrames.Length} 帧动画资源");
        }
    }
    
    // 编辑器工具
    [ContextMenu("运行自动设置")]
    void RunAutoSetup()
    {
        Start();
    }
    
    [ContextMenu("检查场景状态")]
    void CheckSceneStatus()
    {
        Debug.Log("=== 场景状态检查 ===");
        
        // 检查 TapCat 对象
        GameObject tapCat = GameObject.Find("TapCat");
        Debug.Log(tapCat != null ? "✅ TapCat 对象存在" : "❌ TapCat 对象不存在");
        
        // 检查控制器
        TapCatAnimated controller = FindObjectOfType<TapCatAnimated>();
        Debug.Log(controller != null ? "✅ TapCat 控制器存在" : "❌ TapCat 控制器不存在");
        
        // 检查动画系统
        SimpleCatAnimation anim = FindObjectOfType<SimpleCatAnimation>();
        if (anim != null)
        {
            Debug.Log(anim.animationFrames != null && anim.animationFrames.Length > 0 
                ? $"✅ 动画系统就绪 ({anim.animationFrames.Length} 帧)" 
                : "⚠️ 动画系统未设置资源");
        }
        else
        {
            Debug.Log("❌ 动画控制器不存在");
        }
        
        Debug.Log("=== 检查完成 ===");
    }
}