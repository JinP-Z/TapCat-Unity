// TapCat 动画版本 - 完全替换原有系统
// 用户只需点击 Play 即可验收

using UnityEngine;

public class TapCatAnimated : MonoBehaviour
{
    [Header("动画设置")]
    public SimpleCatAnimation catAnimation;
    
    [Header("游戏设置")]
    public int clickCount = 0;
    public float rotationSpeed = 30f;
    
    [Header("输入设置")]
    public KeyCode triggerKey = KeyCode.Space;
    public KeyCode resetKey = KeyCode.R;
    public bool useMouseClick = true;
    
    // 私有变量
    private GameObject catObject;
    private Renderer catRenderer;
    
    void Start()
    {
        SetupCat();
        SetupAnimation();
        
        Debug.Log("🎮 TapCat 动画版启动");
        Debug.Log("💡 按空格键或鼠标左键播放猫咪动画");
        Debug.Log("💡 按 R 键重置游戏");
    }
    
    void Update()
    {
        // 处理输入
        if (Input.GetKeyDown(triggerKey))
        {
            HandleClick();
        }
        
        if (useMouseClick && Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
        
        if (Input.GetKeyDown(resetKey))
        {
            ResetGame();
        }
        
        // 持续旋转（当动画不在播放时）
        if (catObject != null && (catAnimation == null || !catAnimation.IsPlaying()))
        {
            catObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
    
    void SetupCat()
    {
        // 查找或创建猫咪对象
        catObject = GameObject.Find("TapCat");
        if (catObject == null)
        {
            catObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            catObject.name = "TapCat";
            catObject.transform.position = Vector3.zero;
            catObject.transform.localScale = new Vector3(3f, 3f, 0.2f);
            
            // 设置颜色
            catRenderer = catObject.GetComponent<Renderer>();
            if (catRenderer != null)
            {
                catRenderer.material.color = new Color(1f, 0.8f, 0f); // 亮黄色
            }
        }
        else
        {
            catRenderer = catObject.GetComponent<Renderer>();
        }
    }
    
    void SetupAnimation()
    {
        // 如果动画控制器未设置，尝试自动获取
        if (catAnimation == null)
        {
            catAnimation = GetComponent<SimpleCatAnimation>();
        }
        
        // 如果还是没有，自动添加
        if (catAnimation == null)
        {
            catAnimation = gameObject.AddComponent<SimpleCatAnimation>();
        }
    }
    
    void HandleClick()
    {
        clickCount++;
        Debug.Log($"👆 点击！次数: {clickCount}");
        
        // 播放动画
        if (catAnimation != null)
        {
            catAnimation.Play();
        }
        
        // 颜色变化效果
        if (catRenderer != null)
        {
            float r = Random.Range(0.5f, 1f);
            float g = Random.Range(0.5f, 1f);
            float b = Random.Range(0.5f, 1f);
            catRenderer.material.color = new Color(r, g, b);
            
            // 点击旋转效果
            catObject.transform.Rotate(0, 360, 0);
        }
        
        // 更新UI显示
        UpdateUI();
    }
    
    void UpdateUI()
    {
        // 在 OnGUI 中显示
    }
    
    void OnGUI()
    {
        // 显示点击计数
        GUI.color = Color.white;
        GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
        
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        
        string status = catAnimation != null && catAnimation.IsPlaying() 
            ? $"点击次数: {clickCount}\n🎬 动画播放中..." 
            : $"点击次数: {clickCount}\n⏸️ 动画待机";
        
        GUI.Box(new Rect(10, 10, 250, 70), $"{status}\n按R键重置", style);
        
        // 操作提示
        GUIStyle hintStyle = new GUIStyle(GUI.skin.label);
        hintStyle.fontSize = 14;
        hintStyle.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 90, 300, 40), "💡 提示：按空格键或鼠标左键", hintStyle);
    }
    
    void ResetGame()
    {
        clickCount = 0;
        Debug.Log("🔄 游戏已重置");
        
        // 重置颜色
        if (catRenderer != null)
        {
            catRenderer.material.color = new Color(1f, 0.8f, 0f);
        }
    }
    
    // 编辑器工具
    [ContextMenu("测试点击")]
    void TestClick()
    {
        HandleClick();
    }
    
    [ContextMenu("重置游戏")]
    void TestReset()
    {
        ResetGame();
    }
}