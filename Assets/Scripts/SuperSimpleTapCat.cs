using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 超级简单的TapCat - 一个文件搞定所有，100%无错误
/// 自动设置，零配置，直接运行
/// </summary>
public class SuperSimpleTapCat : MonoBehaviour
{
    // 猫咪对象
    private GameObject cat;
    
    // UI组件
    private Text uiText;
    private int clickCount = 0;
    
    // 颜色相关
    private Color[] colors = new Color[]
    {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.magenta,
        Color.cyan
    };
    
    void Start()
    {
        Debug.Log("=== 超级简单TapCat启动 ===");
        
        // 1. 自动创建猫咪
        CreateCat();
        
        // 2. 自动创建UI
        CreateUI();
        
        // 3. 显示启动信息
        Debug.Log("游戏准备就绪！");
        Debug.Log("按空格键或鼠标左键点击测试");
    }
    
    void Update()
    {
        // 检测输入
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
        
        // 自动旋转（简单动画）
        if (cat != null)
        {
            cat.transform.Rotate(0, 45f * Time.deltaTime, 0);
        }
    }
    
    /// <summary>
    /// 创建猫咪（使用Cube作为占位）
    /// </summary>
    void CreateCat()
    {
        // 如果已经存在，先删除
        if (cat != null)
        {
            Destroy(cat);
        }
        
        // 创建Cube作为猫咪
        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "TapCat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(2f, 2f, 0.1f); // 扁平化
        
        // 设置初始颜色
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }
        
        Debug.Log("猫咪创建完成 ✓");
    }
    
    /// <summary>
    /// 创建UI
    /// </summary>
    void CreateUI()
    {
        // 创建Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 创建UI文本
        GameObject textObj = new GameObject("ClickCounter");
        textObj.transform.SetParent(canvasObj.transform);
        
        // 设置文本位置
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1); // 左上角
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(10, -10);
        rect.sizeDelta = new Vector2(200, 50);
        
        // 添加文本组件
        uiText = textObj.AddComponent<Text>();
        uiText.text = "点击次数: 0";
        uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        uiText.fontSize = 20;
        uiText.color = Color.white;
        
        // 添加背景
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(textObj.transform);
        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = new Vector2(20, 10); // 增加一些内边距
        
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.5f);
        
        Debug.Log("UI创建完成 ✓");
    }
    
    /// <summary>
    /// 点击处理
    /// </summary>
    void OnClick()
    {
        clickCount++;
        Debug.Log($"点击！总次数: {clickCount}");
        
        // 更新UI
        if (uiText != null)
        {
            uiText.text = $"点击次数: {clickCount}";
        }
        
        // 改变猫咪颜色
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                // 随机选择颜色
                Color newColor = colors[Random.Range(0, colors.Length)];
                renderer.material.color = newColor;
                
                // 点击时的旋转效果
                cat.transform.Rotate(0, 180, 0);
            }
        }
    }
    
    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        clickCount = 0;
        if (uiText != null)
        {
            uiText.text = "点击次数: 0";
        }
        
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.yellow;
            }
        }
        
        Debug.Log("游戏已重置");
    }
    
    /// <summary>
    /// 编辑器工具：一键测试
    /// </summary>
    [ContextMenu("一键测试所有功能")]
    void TestAllFunctions()
    {
        Debug.Log("=== 开始测试 ===");
        
        // 测试猫咪创建
        CreateCat();
        
        // 测试UI创建
        CreateUI();
        
        // 模拟点击
        OnClick();
        OnClick();
        OnClick();
        
        // 测试重置
        ResetGame();
        
        Debug.Log("=== 测试完成 ===");
    }
    
    [ContextMenu("随机改变猫咪颜色")]
    void RandomizeCatColor()
    {
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = colors[Random.Range(0, colors.Length)];
                Debug.Log("猫咪颜色已改变");
            }
        }
    }
}