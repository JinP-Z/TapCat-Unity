using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 瓒呯骇绠€鍗曠殑鑷姩璁剧疆宸ュ叿
/// 鍙湁涓€涓枃浠讹紝娌℃湁渚濊禆闂
/// </summary>
public class AutoSetupSimple : MonoBehaviour
{
    [Header("鑷姩璁剧疆")]
    public bool runOnStart = true;
    
    [Header("鐚挭璁剧疆")]
    public Color catColor = new Color(1f, 0.5f, 0f, 1f); // 姗欒壊
    public Vector3 catPosition = Vector3.zero;
    public Vector3 catScale = new Vector3(2f, 2f, 1f);
    
    [Header("UI璁剧疆")]
    public Vector2 uiPosition = new Vector2(-20f, -20f);
    
    private GameObject tapCatObject;
    private int tapCount = 0;
    
    void Start()
    {
        if (runOnStart)
        {
            Debug.Log("=== 铏惧疂瓒呯骇绠€鍗曡嚜鍔ㄨ缃紑濮?===");
            SetupEverything();
            Debug.Log("=== 铏惧疂鑷姩璁剧疆瀹屾垚 ===");
        }
    }
    
    /// <summary>
    /// 璁剧疆涓€鍒?
    /// </summary>
    public void SetupEverything()
    {
        Debug.Log("铏惧疂寮€濮嬭缃?..");
        
        // 1. 璁剧疆鐚挭
        SetupCat();
        
        // 2. 璁剧疆UI
        SetupUI();
        
        Debug.Log("Setup complete.");
        Debug.Log("鎸夌┖鏍奸敭鎴栫偣鍑婚紶鏍囨祴璇曪紒");
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
    
    void Update()
    {
        // 妫€娴嬬偣鍑?
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            OnTap();
        }
        
        // 绠€鍗曟棆杞姩鐢?
        if (tapCatObject != null)
        {
            tapCatObject.transform.Rotate(0, 30f * Time.deltaTime, 0);
        }
    }
    
    void OnTap()
    {
        tapCount++;
        Debug.Log($"鐐瑰嚮锛佹鏁? {tapCount}");
        
        // 鏀瑰彉鐚挭棰滆壊
        if (tapCatObject != null)
        {
            SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color randomColor = new Color(
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    1f
                );
                spriteRenderer.color = randomColor;
                
                // 蹇€熸棆杞竴涓?
                tapCatObject.transform.Rotate(0, 180f, 0);
            }
        }
        
        // 鏇存柊UI
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
                textComponent.text = $"鐐瑰嚮娆℃暟: {tapCount}";
            }
        }
    }
    
    /// <summary>
    /// 缂栬緫鍣ㄥ伐鍏凤細涓€閿缃?
    /// </summary>
    [ContextMenu("One Click Setup")]
    private void OneClickSetup()
    {
        SetupEverything();
    }
    
    /// <summary>
    /// 閲嶇疆鐐瑰嚮娆℃暟
    /// </summary>
    [ContextMenu("Reset Counter")]
    private void ResetCounter()
    {
        tapCount = 0;
        UpdateUI();
        Debug.Log("Counter reset.");
    }
    
    /// <summary>
    /// 娴嬭瘯棰滆壊鍙樺寲
    /// </summary>
    [ContextMenu("娴嬭瘯棰滆壊鍙樺寲")]
    private void TestColorChange()
    {
        if (tapCatObject != null)
        {
            SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = new Color(
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    1f
                );
            }
        }
    }
}
