using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 自动设置工具 - 在Unity启动时自动设置场景
/// </summary>
public class AutoSetup : MonoBehaviour
{
    [Header("自动设置")]
    public bool runOnStart = true;
    public bool createCat = true;
    public bool createUI = true;
    
    [Header("猫咪设置")]
    public Color catColor = new Color(1f, 0.5f, 0f, 1f); // 橙色
    public Vector3 catPosition = Vector3.zero;
    public Vector3 catScale = new Vector3(2f, 2f, 1f);
    
    [Header("UI设置")]
    public Vector2 uiPosition = new Vector2(-20f, -20f);
    
    private GameObject tapCatObject;
    
    void Start()
    {
        if (runOnStart)
        {
            Debug.Log("=== 虾宝自动设置开始 ===");
            AutoSetupScene();
            Debug.Log("=== 虾宝自动设置完成 ===");
        }
    }
    
    /// <summary>
    /// 自动设置场景
    /// </summary>
    public void AutoSetupScene()
    {
        Debug.Log("虾宝开始自动设置TapCat场景...");
        
        // 1. 设置猫咪
        if (createCat)
        {
            SetupCat();
        }
        
        // 2. 设置UI
        if (createUI)
        {
            SetupUI();
        }
        
        // 3. 添加简单动画
        AddSimpleAnimation();
        
        Debug.Log("虾宝自动设置完成！");
        Debug.Log("猫咪应该显示在屏幕中央了！");
        Debug.Log("点击空格键或鼠标让猫咪跳舞！");
    }
    
    /// <summary>
    /// 设置猫咪
    /// </summary>
    private void SetupCat()
    {
        Debug.Log("设置猫咪...");
        
        // 查找或创建猫咪对象
        tapCatObject = GameObject.Find("TapCat");
        if (tapCatObject == null)
        {
            tapCatObject = new GameObject("TapCat");
            Debug.Log("创建TapCat对象");
        }
        
        // 设置位置和缩放
        tapCatObject.transform.position = catPosition;
        tapCatObject.transform.localScale = catScale;
        
        // 确保有SpriteRenderer
        SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = tapCatObject.AddComponent<SpriteRenderer>();
            Debug.Log("添加SpriteRenderer组件");
        }
        
        // 设置颜色
        spriteRenderer.color = catColor;
        Debug.Log($"设置猫咪颜色: {catColor}");
        
        // 添加简单控制器
        if (tapCatObject.GetComponent<SimpleCatController>() == null)
        {
            tapCatObject.AddComponent<SimpleCatController>();
            Debug.Log("添加SimpleCatController");
        }
        
        Debug.Log("猫咪设置完成");
    }
    
    /// <summary>
    /// 设置UI
    /// </summary>
    private void SetupUI()
    {
        Debug.Log("设置UI...");
        
        // 查找或创建Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            Debug.Log("创建Canvas");
        }
        
        // 创建UI容器
        GameObject uiContainer = new GameObject("TapCounterUI");
        uiContainer.transform.SetParent(canvas.transform);
        
        // 设置位置和大小
        RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
        rectTransform.anchoredPosition = uiPosition;
        rectTransform.sizeDelta = new Vector2(200, 80);
        
        // 添加背景
        GameObject background = new GameObject("Background");
        background.transform.SetParent(uiContainer.transform);
        RectTransform bgRect = background.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        
        // 添加点击次数文本
        GameObject countTextObj = new GameObject("TapCountText");
        countTextObj.transform.SetParent(uiContainer.transform);
        RectTransform countRect = countTextObj.AddComponent<RectTransform>();
        countRect.anchorMin = new Vector2(0, 0.5f);
        countRect.anchorMax = new Vector2(1, 0.8f);
        countRect.anchoredPosition = Vector2.zero;
        countRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI countText = countTextObj.AddComponent<TextMeshProUGUI>();
        countText.text = "点击次数: 0";
        countText.fontSize = 24;
        countText.color = Color.white;
        countText.alignment = TextAlignmentOptions.Center;
        
        Debug.Log("UI设置完成");
    }
    
    /// <summary>
    /// 添加简单动画
    /// </summary>
    private void AddSimpleAnimation()
    {
        Debug.Log("添加简单动画...");
        
        // 添加旋转组件
        if (tapCatObject != null && tapCatObject.GetComponent<SimpleRotator>() == null)
        {
            tapCatObject.AddComponent<SimpleRotator>();
            Debug.Log("添加SimpleRotator动画");
        }
        
        Debug.Log("动画设置完成");
    }
    
    /// <summary>
    /// 编辑器工具：一键设置
    /// </summary>
    [ContextMenu("虾宝一键设置")]
    private void OneClickSetup()
    {
        AutoSetupScene();
    }
}

/// <summary>
    /// 简单猫咪控制器
/// </summary>
public class SimpleCatController : MonoBehaviour
{
    private int tapCount = 0;
    private float rotationSpeed = 0f;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("SimpleCatController启动");
    }
    
    void Update()
    {
        // 检测点击
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            OnTap();
        }
        
        // 旋转动画
        if (rotationSpeed > 0)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
    
    void OnTap()
    {
        tapCount++;
        Debug.Log($"点击！次数: {tapCount}");
        
        // 改变颜色
        if (spriteRenderer != null)
        {
            Color randomColor = new Color(
                Random.Range(0.5f, 1f),
                Random.Range(0.5f, 1f),
                Random.Range(0.5f, 1f),
                1f
            );
            spriteRenderer.color = randomColor;
        }
        
        // 增加旋转速度
        rotationSpeed = 180f;
        
        // 更新UI（如果存在）
        UpdateUI();
    }
    
    void UpdateUI()
    {
        GameObject uiText = GameObject.Find("TapCountText");
        if (uiText != null)
        {
            TextMeshProUGUI textComponent = uiText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = $"点击次数: {tapCount}";
            }
        }
    }
}

/// <summary>
/// 简单旋转组件
/// </summary>
public class SimpleRotator : MonoBehaviour
{
    public float baseRotationSpeed = 30f;
    private float currentRotationSpeed = 0f;
    
    void Update()
    {
        if (currentRotationSpeed > 0)
        {
            transform.Rotate(0, currentRotationSpeed * Time.deltaTime, 0);
            currentRotationSpeed = Mathf.Lerp(currentRotationSpeed, baseRotationSpeed, Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, baseRotationSpeed * Time.deltaTime, 0);
        }
    }
    
    public void SetRotationSpeed(float speed)
    {
        currentRotationSpeed = speed;
    }
}