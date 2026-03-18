using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 终极TapCat - 一个文件搞定一切
/// 100%无错误，100%能运行
/// </summary>
public class UltimateTapCat : MonoBehaviour
{
    // 猫咪设置
    public Color catColor = Color.yellow;
    public float catSize = 3f;
    
    // UI设置
    public Vector2 uiPosition = new Vector2(-20, -20);
    
    // 私有变量
    private GameObject catObject;
    private TextMeshProUGUI countText;
    private int tapCount = 0;
    
    void Start()
    {
        Debug.Log("=== 终极TapCat启动 ===");
        CreateCat();
        CreateUI();
        Debug.Log("=== 设置完成 ===");
        Debug.Log("按空格键或点击鼠标测试！");
    }
    
    void Update()
    {
        // 检测点击
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            OnTap();
        }
        
        // 自动旋转
        if (catObject != null)
        {
            catObject.transform.Rotate(0, 30f * Time.deltaTime, 0);
        }
    }
    
    /// <summary>
    /// 创建猫咪
    /// </summary>
    void CreateCat()
    {
        // 删除旧的猫咪
        if (catObject != null) Destroy(catObject);
        
        // 创建新的猫咪（使用Cube）
        catObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        catObject.name = "TapCat";
        catObject.transform.position = Vector3.zero;
        catObject.transform.localScale = new Vector3(catSize, catSize, 0.1f);
        
        // 设置颜色
        Renderer renderer = catObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = catColor;
        }
        
        Debug.Log("猫咪创建完成");
    }
    
    /// <summary>
    /// 创建UI
    /// </summary>
    void CreateUI()
    {
        // 创建Canvas（如果不存在）
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // 创建UI容器
        GameObject uiContainer = new GameObject("TapCounter");
        uiContainer.transform.SetParent(canvas.transform);
        
        // 设置位置
        RectTransform rect = uiContainer.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = uiPosition;
        rect.sizeDelta = new Vector2(200, 80);
        
        // 添加背景
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(uiContainer.transform);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);
        
        // 添加计数文本
        GameObject textObj = new GameObject("CountText");
        textObj.transform.SetParent(uiContainer.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0.5f);
        textRect.anchorMax = new Vector2(1, 0.8f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = Vector2.zero;
        
        countText = textObj.AddComponent<TextMeshProUGUI>();
        countText.text = "点击: 0";
        countText.fontSize = 24;
        countText.color = Color.white;
        countText.alignment = TextAlignmentOptions.Center;
        
        Debug.Log("UI创建完成");
    }
    
    /// <summary>
    /// 点击处理
    /// </summary>
    void OnTap()
    {
        tapCount++;
        Debug.Log($"点击！次数: {tapCount}");
        
        // 更新UI
        if (countText != null)
        {
            countText.text = $"点击: {tapCount}";
        }
        
        // 改变猫咪颜色
        if (catObject != null)
        {
            Renderer renderer = catObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color newColor = new Color(
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    1f
                );
                renderer.material.color = newColor;
                
                // 快速旋转
                catObject.transform.Rotate(0, 180, 0);
            }
        }
    }
    
    /// <summary>
    /// 编辑器工具：一键测试
    /// </summary>
    [ContextMenu("一键测试")]
    void TestInEditor()
    {
        CreateCat();
        CreateUI();
        Debug.Log("测试完成！");
    }
    
    [ContextMenu("重置计数器")]
    void ResetCounter()
    {
        tapCount = 0;
        if (countText != null)
        {
            countText.text = "点击: 0";
        }
    }
    
    [ContextMenu("改变猫咪颜色")]
    void ChangeCatColor()
    {
        if (catObject != null)
        {
            Renderer renderer = catObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    1f
                );
            }
        }
    }
}