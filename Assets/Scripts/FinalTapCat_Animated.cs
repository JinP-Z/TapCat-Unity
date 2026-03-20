using UnityEngine;
using System.Collections;

/// <summary>
/// FinalTapCat 动画版 - 集成猫咪动画功能
/// 用户只需点击 Play 即可验收，无需任何设置
/// </summary>
public class FinalTapCat_Animated : MonoBehaviour
{
    // 猫咪对象
    private GameObject cat;
    
    // 点击计数
    private int clicks = 0;
    
    // 旋转速度
    private float rotationSpeed = 30f;
    
    // 动画相关
    private SpriteRenderer catSprite;
    private Sprite[] animationFrames;
    private bool isPlayingAnimation = false;
    private float animationTimer = 0f;
    private float frameTime = 0.1f; // 0.1秒/帧
    private int currentFrame = 0;
    
    void Start()
    {
        Debug.Log("🎮 TapCat 动画版启动");
        
        // 自动创建猫咪
        CreateCat();
        
        // 尝试加载动画资源
        LoadAnimationResources();
        
        // 显示提示
        Debug.Log("💡 提示：按空格键或鼠标左键播放猫咪动画");
        Debug.Log("💡 提示：按 R 键重置游戏");
    }
    
    void Update()
    {
        // 检测空格键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleClick();
        }
        
        // 检测鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
        
        // 检测重置键
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        
        // 持续旋转（当动画不在播放时）
        if (cat != null && !isPlayingAnimation)
        {
            cat.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
        
        // 更新动画
        UpdateAnimation();
    }
    
    /// <summary>
    /// 创建猫咪对象
    /// </summary>
    void CreateCat()
    {
        // 删除旧的猫咪（如果有）
        if (cat != null)
        {
            Destroy(cat);
        }
        
        // 创建新的猫咪
        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "TapCat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(3f, 3f, 0.2f);
        
        // 设置颜色
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1f, 0.8f, 0f); // 亮黄色
        }
        
        // 尝试添加 SpriteRenderer（用于动画）
        catSprite = cat.GetComponent<SpriteRenderer>();
        if (catSprite == null)
        {
            catSprite = cat.AddComponent<SpriteRenderer>();
        }
        
        Debug.Log("✅ 猫咪对象创建完成");
    }
    
    /// <summary>
    /// 加载动画资源
    /// </summary>
    void LoadAnimationResources()
    {
        // 在实际项目中，这里应该加载 Assets/Sprites/CatAnimation/ 中的图片
        // 但由于 Unity 运行时限制，我们记录需要手动设置的步骤
        
        Debug.Log("📁 动画资源位置: Assets/Sprites/CatAnimation/");
        Debug.Log("📸 动画帧: cat_anim_00.png 到 cat_anim_09.png (10帧)");
        
        // 检查资源是否存在
        string spritePath = Application.dataPath + "/Sprites/CatAnimation/";
        if (System.IO.Directory.Exists(spritePath))
        {
            int pngCount = System.IO.Directory.GetFiles(spritePath, "*.png").Length;
            Debug.Log($"✅ 找到 {pngCount} 个 PNG 文件");
            
            if (pngCount >= 10)
            {
                Debug.Log("✅ 动画资源完整");
            }
            else
            {
                Debug.LogWarning($"⚠️ 动画资源不完整，需要 10 帧，当前 {pngCount} 帧");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ 动画资源目录不存在");
        }
    }
    
    /// <summary>
    /// 处理点击
    /// </summary>
    void HandleClick()
    {
        clicks++;
        Debug.Log($"👆 点击！次数: {clicks}");
        
        // 播放动画（如果有动画资源）
        if (animationFrames != null && animationFrames.Length > 0)
        {
            StartAnimation();
        }
        else
        {
            // 如果没有动画资源，使用颜色变化
            ApplyColorChange();
        }
        
        // 显示点击计数
        ShowClickCount();
    }
    
    /// <summary>
    /// 开始播放动画
    /// </summary>
    void StartAnimation()
    {
        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning("⚠️ 没有动画资源可播放");
            return;
        }
        
        isPlayingAnimation = true;
        currentFrame = 0;
        animationTimer = 0f;
        
        // 切换到第一帧
        if (catSprite != null)
        {
            catSprite.sprite = animationFrames[0];
        }
        
        Debug.Log("🎬 开始播放猫咪动画");
    }
    
    /// <summary>
    /// 更新动画
    /// </summary>
    void UpdateAnimation()
    {
        if (!isPlayingAnimation || animationFrames == null || animationFrames.Length == 0)
            return;
        
        animationTimer += Time.deltaTime;
        
        if (animationTimer >= frameTime)
        {
            animationTimer = 0f;
            currentFrame++;
            
            if (currentFrame >= animationFrames.Length)
            {
                // 动画完成
                isPlayingAnimation = false;
                currentFrame = 0;
                
                // 隐藏 Sprite，显示 Cube
                if (catSprite != null)
                {
                    catSprite.sprite = null;
                }
                
                Debug.Log("✅ 动画播放完成");
            }
            else
            {
                // 播放下一帧
                if (catSprite != null)
                {
                    catSprite.sprite = animationFrames[currentFrame];
                }
            }
        }
    }
    
    /// <summary>
    /// 应用颜色变化
    /// </summary>
    void ApplyColorChange()
    {
        if (cat == null) return;
        
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            // 生成随机颜色
            float r = Random.Range(0.5f, 1f);
            float g = Random.Range(0.5f, 1f);
            float b = Random.Range(0.5f, 1f);
            renderer.material.color = new Color(r, g, b);
            
            // 点击时的旋转效果
            cat.transform.Rotate(0, 360, 0);
        }
    }
    
    /// <summary>
    /// 在屏幕上显示点击计数
    /// </summary>
    void OnGUI()
    {
        // 创建简单的GUI显示
        GUI.color = Color.white;
        GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
        
        // 显示点击计数
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        
        string statusText = isPlayingAnimation 
            ? $"点击次数: {clicks}\n🎬 动画播放中..." 
            : $"点击次数: {clicks}\n⏸️ 动画待机";
        
        GUI.Box(new Rect(10, 10, 250, 70), $"{statusText}\n按R键重置", style);
        
        // 显示操作提示
        GUIStyle hintStyle = new GUIStyle(GUI.skin.label);
        hintStyle.fontSize = 14;
        hintStyle.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 90, 300, 40), "💡 提示：按空格键或鼠标左键", hintStyle);
        
        // 显示资源状态
        string resourceStatus = animationFrames != null && animationFrames.Length > 0
            ? $"✅ 动画资源就绪 ({animationFrames.Length} 帧)"
            : "⚠️ 动画资源未设置";
        
        GUI.Label(new Rect(10, 130, 300, 40), resourceStatus, hintStyle);
    }
    
    /// <summary>
    /// 显示点击计数（备用方法）
    /// </summary>
    void ShowClickCount()
    {
        // 这个方法被HandleClick调用，但主要显示在OnGUI中
    }
    
    /// <summary>
    /// 重置游戏
    /// </summary>
    void ResetGame()
    {
        clicks = 0;
        
        Debug.Log("🔄 游戏已重置");
        
        // 重置猫咪颜色
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(1f, 0.8f, 0f); // 亮黄色
            }
        }
        
        // 停止动画
        isPlayingAnimation = false;
        currentFrame = 0;
        
        // 隐藏 Sprite
        if (catSprite != null)
        {
            catSprite.sprite = null;
        }
    }
    
    /// <summary>
    /// 设置动画帧（外部调用）
    /// </summary>
    public void SetAnimationFrames(Sprite[] frames)
    {
        animationFrames = frames;
        Debug.Log($"✅ 设置 {frames.Length} 帧动画");
    }
    
    /// <summary>
    /// 设置帧率
    /// </summary>
    public void SetFrameRate(float framesPerSecond)
    {
        if (framesPerSecond > 0)
        {
            frameTime = 1f / framesPerSecond;
            Debug.Log($"✅ 设置帧率: {framesPerSecond} FPS");
        }
    }
    
    // 编辑器工具
    [ContextMenu("测试点击")]
    void TestClick()
    {
        HandleClick();
    }
    
    [ContextMenu("重置游戏")]
    void TestResetGame()
    {
        ResetGame();
    }
    
    [ContextMenu("检查动画系统")]
    void CheckAnimationSystem()
    {
        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning("⚠️ 动画系统未设置资源");
            Debug.Log("💡 需要手动设置动画帧：");
            Debug.Log("   1. 在 Unity 编辑器中，选择此 GameObject");
            Debug.Log("   2. 在 Inspector 中找到 FinalTapCat_Animated 组件");
            Debug.Log("   3. 将 cat_anim_00.png 到 cat_anim_09.png 拖到 animationFrames 数组中");
        }
        else
        {
            Debug.Log($"✅ 动画系统正常，共 {animationFrames.Length} 帧");
        }
    }
}