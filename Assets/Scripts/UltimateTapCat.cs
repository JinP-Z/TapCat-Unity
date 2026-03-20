using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 缁堟瀬TapCat - 涓€涓枃浠舵悶瀹氫竴鍒?
/// 100%鏃犻敊璇紝100%鑳借繍琛?
/// </summary>
public class UltimateTapCat : MonoBehaviour
{
    // 鐚挭璁剧疆
    public Color catColor = Color.yellow;
    public float catSize = 3f;
    
    // UI璁剧疆
    public Vector2 uiPosition = new Vector2(-20, -20);
    
    // 绉佹湁鍙橀噺
    private GameObject catObject;
    private TextMeshProUGUI countText;
    private int tapCount = 0;
    
    void Start()
    {
        Debug.Log("=== 缁堟瀬TapCat鍚姩 ===");
        CreateCat();
        CreateUI();
        Debug.Log("=== 璁剧疆瀹屾垚 ===");
        Debug.Log("鎸夌┖鏍奸敭鎴栫偣鍑婚紶鏍囨祴璇曪紒");
    }
    
    void Update()
    {
        // 妫€娴嬬偣鍑?
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            OnTap();
        }
        
        // 鑷姩鏃嬭浆
        if (catObject != null)
        {
            catObject.transform.Rotate(0, 30f * Time.deltaTime, 0);
        }
    }
    
    /// <summary>
    /// 鍒涘缓鐚挭
    /// </summary>
    void CreateCat()
    {
        // 鍒犻櫎鏃х殑鐚挭
        if (catObject != null) Destroy(catObject);
        
        // 鍒涘缓鏂扮殑鐚挭锛堜娇鐢–ube锛?
        catObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        catObject.name = "TapCat";
        catObject.transform.position = Vector3.zero;
        catObject.transform.localScale = new Vector3(catSize, catSize, 0.1f);
        
        // 璁剧疆棰滆壊
        Renderer renderer = catObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = catColor;
        }
        
        Debug.Log("鐚挭鍒涘缓瀹屾垚");
    }
    
    /// <summary>
    /// 鍒涘缓UI
    /// </summary>
    void CreateUI()
    {
        // 鍒涘缓Canvas锛堝鏋滀笉瀛樺湪锛?
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // 鍒涘缓UI瀹瑰櫒
        GameObject uiContainer = new GameObject("TapCounter");
        uiContainer.transform.SetParent(canvas.transform);
        
        // 璁剧疆浣嶇疆
        RectTransform rect = uiContainer.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = uiPosition;
        rect.sizeDelta = new Vector2(200, 80);
        
        // 娣诲姞鑳屾櫙
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(uiContainer.transform);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);
        
        // 娣诲姞璁℃暟鏂囨湰
        GameObject textObj = new GameObject("CountText");
        textObj.transform.SetParent(uiContainer.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0.5f);
        textRect.anchorMax = new Vector2(1, 0.8f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = Vector2.zero;
        
        countText = textObj.AddComponent<TextMeshProUGUI>();
        countText.text = "鐐瑰嚮: 0";
        countText.fontSize = 24;
        countText.color = Color.white;
        countText.alignment = TextAlignmentOptions.Center;
        
        Debug.Log("UI鍒涘缓瀹屾垚");
    }
    
    /// <summary>
    /// 鐐瑰嚮澶勭悊
    /// </summary>
    void OnTap()
    {
        tapCount++;
        Debug.Log($"鐐瑰嚮锛佹鏁? {tapCount}");
        
        // 鏇存柊UI
        if (countText != null)
        {
            countText.text = $"鐐瑰嚮: {tapCount}";
        }
        
        // 鏀瑰彉鐚挭棰滆壊
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
                
                // 蹇€熸棆杞?
                catObject.transform.Rotate(0, 180, 0);
            }
        }
    }
    
    /// <summary>
    /// 缂栬緫鍣ㄥ伐鍏凤細涓€閿祴璇?
    /// </summary>
    [ContextMenu("One Click Test")]
    void TestInEditor()
    {
        CreateCat();
        CreateUI();
        Debug.Log("Test complete.");
    }
    
    [ContextMenu("Reset Counter")]
    void ResetCounter()
    {
        tapCount = 0;
        if (countText != null)
        {
            countText.text = "鐐瑰嚮: 0";
        }
    }
    
    [ContextMenu("鏀瑰彉鐚挭棰滆壊")]
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
