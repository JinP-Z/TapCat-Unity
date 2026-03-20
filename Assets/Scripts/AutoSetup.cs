using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 鑷姩璁剧疆宸ュ叿 - 鍦║nity鍚姩鏃惰嚜鍔ㄨ缃満鏅?
/// </summary>
public class AutoSetup : MonoBehaviour
{
    [Header("鑷姩璁剧疆")]
    public bool runOnStart = true;
    public bool createCat = true;
    public bool createUI = true;
    
    [Header("鐚挭璁剧疆")]
    public Color catColor = new Color(1f, 0.5f, 0f, 1f); // 姗欒壊
    public Vector3 catPosition = Vector3.zero;
    public Vector3 catScale = new Vector3(2f, 2f, 1f);
    
    [Header("UI璁剧疆")]
    public Vector2 uiPosition = new Vector2(-20f, -20f);
    
    private GameObject tapCatObject;
    
    void Start()
    {
        if (runOnStart)
        {
            Debug.Log("=== 铏惧疂鑷姩璁剧疆寮€濮?===");
            AutoSetupScene();
            Debug.Log("=== 铏惧疂鑷姩璁剧疆瀹屾垚 ===");
        }
    }
    
    /// <summary>
    /// 鑷姩璁剧疆鍦烘櫙
    /// </summary>
    public void AutoSetupScene()
    {
        Debug.Log("铏惧疂寮€濮嬭嚜鍔ㄨ缃甌apCat鍦烘櫙...");
        
        // 1. 璁剧疆鐚挭
        if (createCat)
        {
            SetupCat();
        }
        
        // 2. 璁剧疆UI
        if (createUI)
        {
            SetupUI();
        }
        
        // 3. 娣诲姞绠€鍗曞姩鐢?
        AddSimpleAnimation();
        
        Debug.Log("Auto setup complete.");
        Debug.Log("TapCat should be visible at screen center.");
        Debug.Log("鐐瑰嚮绌烘牸閿垨榧犳爣璁╃尗鍜烦鑸烇紒");
    }
    
    /// <summary>
    /// 璁剧疆鐚挭
    /// </summary>
    private void SetupCat()
    {
        Debug.Log("璁剧疆鐚挭...");
        
        // 鏌ユ壘鎴栧垱寤虹尗鍜璞?
        tapCatObject = GameObject.Find("TapCat");
        if (tapCatObject == null)
        {
            tapCatObject = new GameObject("TapCat");
            Debug.Log("鍒涘缓TapCat瀵硅薄");
        }
        
        // 璁剧疆浣嶇疆鍜岀缉鏀?
        tapCatObject.transform.position = catPosition;
        tapCatObject.transform.localScale = catScale;
        
        // 纭繚鏈塖priteRenderer
        SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = tapCatObject.AddComponent<SpriteRenderer>();
            Debug.Log("娣诲姞SpriteRenderer缁勪欢");
        }
        
        // 璁剧疆棰滆壊
        spriteRenderer.color = catColor;
        Debug.Log($"璁剧疆鐚挭棰滆壊: {catColor}");
        
        // 娣诲姞绠€鍗曟帶鍒跺櫒
        if (tapCatObject.GetComponent<SimpleCatController>() == null)
        {
            tapCatObject.AddComponent<SimpleCatController>();
            Debug.Log("娣诲姞SimpleCatController");
        }
        
        Debug.Log("鐚挭璁剧疆瀹屾垚");
    }
    
    /// <summary>
    /// 璁剧疆UI
    /// </summary>
    private void SetupUI()
    {
        Debug.Log("璁剧疆UI...");
        
        // 鏌ユ壘鎴栧垱寤篊anvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            Debug.Log("鍒涘缓Canvas");
        }
        
        // 鍒涘缓UI瀹瑰櫒
        GameObject uiContainer = new GameObject("TapCounterUI");
        uiContainer.transform.SetParent(canvas.transform);
        
        // 璁剧疆浣嶇疆鍜屽ぇ灏?
        RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(1, 1);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(1, 1);
        rectTransform.anchoredPosition = uiPosition;
        rectTransform.sizeDelta = new Vector2(200, 80);
        
        // 娣诲姞鑳屾櫙
        GameObject background = new GameObject("Background");
        background.transform.SetParent(uiContainer.transform);
        RectTransform bgRect = background.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        Image bgImage = background.AddComponent<Image>();
        bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        
        // 娣诲姞鐐瑰嚮娆℃暟鏂囨湰
        GameObject countTextObj = new GameObject("TapCountText");
        countTextObj.transform.SetParent(uiContainer.transform);
        RectTransform countRect = countTextObj.AddComponent<RectTransform>();
        countRect.anchorMin = new Vector2(0, 0.5f);
        countRect.anchorMax = new Vector2(1, 0.8f);
        countRect.anchoredPosition = Vector2.zero;
        countRect.sizeDelta = Vector2.zero;
        
        TextMeshProUGUI countText = countTextObj.AddComponent<TextMeshProUGUI>();
        countText.text = "鐐瑰嚮娆℃暟: 0";
        countText.fontSize = 24;
        countText.color = Color.white;
        countText.alignment = TextAlignmentOptions.Center;
        
        Debug.Log("UI璁剧疆瀹屾垚");
    }
    
    /// <summary>
    /// 娣诲姞绠€鍗曞姩鐢?
    /// </summary>
    private void AddSimpleAnimation()
    {
        Debug.Log("娣诲姞绠€鍗曞姩鐢?..");
        
        // 娣诲姞鏃嬭浆缁勪欢
        if (tapCatObject != null && tapCatObject.GetComponent<SimpleRotator>() == null)
        {
            tapCatObject.AddComponent<SimpleRotator>();
            Debug.Log("娣诲姞SimpleRotator鍔ㄧ敾");
        }
        
        Debug.Log("鍔ㄧ敾璁剧疆瀹屾垚");
    }
    
    /// <summary>
    /// 缂栬緫鍣ㄥ伐鍏凤細涓€閿缃?
    /// </summary>
    [ContextMenu("One Click Setup")]
    private void OneClickSetup()
    {
        AutoSetupScene();
    }
}
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
