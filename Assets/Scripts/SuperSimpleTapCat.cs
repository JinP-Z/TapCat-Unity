using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 瓒呯骇绠€鍗曠殑TapCat - 涓€涓枃浠舵悶瀹氭墍鏈夛紝100%鏃犻敊璇?
/// 鑷姩璁剧疆锛岄浂閰嶇疆锛岀洿鎺ヨ繍琛?
/// </summary>
public class SuperSimpleTapCat : MonoBehaviour
{
    // 鐚挭瀵硅薄
    private GameObject cat;
    
    // UI缁勪欢
    private Text uiText;
    private int clickCount = 0;
    
    // 棰滆壊鐩稿叧
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
        Debug.Log("=== 瓒呯骇绠€鍗昑apCat鍚姩 ===");
        
        // 1. 鑷姩鍒涘缓鐚挭
        CreateCat();
        
        // 2. 鑷姩鍒涘缓UI
        CreateUI();
        
        // 3. 鏄剧ず鍚姩淇℃伅
        Debug.Log("Game ready.");
        Debug.Log("Press Space or click to test.");
    }
    
    void Update()
    {
        // 妫€娴嬭緭鍏?
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
        
        // 鑷姩鏃嬭浆锛堢畝鍗曞姩鐢伙級
        if (cat != null)
        {
            cat.transform.Rotate(0, 45f * Time.deltaTime, 0);
        }
    }
    
    /// <summary>
    /// 鍒涘缓鐚挭锛堜娇鐢–ube浣滀负鍗犱綅锛?
    /// </summary>
    void CreateCat()
    {
        // 濡傛灉宸茬粡瀛樺湪锛屽厛鍒犻櫎
        if (cat != null)
        {
            Destroy(cat);
        }
        
        // 鍒涘缓Cube浣滀负鐚挭
        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "TapCat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(2f, 2f, 0.1f); // 鎵佸钩鍖?
        
        // 璁剧疆鍒濆棰滆壊
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow;
        }
        
        Debug.Log("TapCat created.");
    }
    
    /// <summary>
    /// 鍒涘缓UI
    /// </summary>
    void CreateUI()
    {
        // 鍒涘缓Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 鍒涘缓UI鏂囨湰
        GameObject textObj = new GameObject("ClickCounter");
        textObj.transform.SetParent(canvasObj.transform);
        
        // 璁剧疆鏂囨湰浣嶇疆
        RectTransform rect = textObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1); // 宸︿笂瑙?
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = new Vector2(10, -10);
        rect.sizeDelta = new Vector2(200, 50);
        
        // 娣诲姞鏂囨湰缁勪欢
        uiText = textObj.AddComponent<Text>();
        uiText.text = "鐐瑰嚮娆℃暟: 0";
        uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        uiText.fontSize = 20;
        uiText.color = Color.white;
        
        // 娣诲姞鑳屾櫙
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(textObj.transform);
        RectTransform bgRect = bgObj.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = new Vector2(20, 10); // 澧炲姞涓€浜涘唴杈硅窛
        
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.5f);
        
        Debug.Log("UI created.");
    }
    
    /// <summary>
    /// 鐐瑰嚮澶勭悊
    /// </summary>
    void OnClick()
    {
        clickCount++;
        Debug.Log($"鐐瑰嚮锛佹€绘鏁? {clickCount}");
        
        // 鏇存柊UI
        if (uiText != null)
        {
            uiText.text = $"鐐瑰嚮娆℃暟: {clickCount}";
        }
        
        // 鏀瑰彉鐚挭棰滆壊
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                // 闅忔満閫夋嫨棰滆壊
                Color newColor = colors[Random.Range(0, colors.Length)];
                renderer.material.color = newColor;
                
                // 鐐瑰嚮鏃剁殑鏃嬭浆鏁堟灉
                cat.transform.Rotate(0, 180, 0);
            }
        }
    }
    
    /// <summary>
    /// 閲嶇疆娓告垙
    /// </summary>
    public void ResetGame()
    {
        clickCount = 0;
        if (uiText != null)
        {
            uiText.text = "鐐瑰嚮娆℃暟: 0";
        }
        
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.yellow;
            }
        }
        
        Debug.Log("Game reset.");
    }
    
    /// <summary>
    /// 缂栬緫鍣ㄥ伐鍏凤細涓€閿祴璇?
    /// </summary>
    [ContextMenu("Test All Functions")]
    void TestAllFunctions()
    {
        Debug.Log("=== 寮€濮嬫祴璇?===");
        
        // 娴嬭瘯鐚挭鍒涘缓
        CreateCat();
        
        // 娴嬭瘯UI鍒涘缓
        CreateUI();
        
        // 妯℃嫙鐐瑰嚮
        OnClick();
        OnClick();
        OnClick();
        
        // 娴嬭瘯閲嶇疆
        ResetGame();
        
        Debug.Log("=== 娴嬭瘯瀹屾垚 ===");
    }
    
    [ContextMenu("闅忔満鏀瑰彉鐚挭棰滆壊")]
    void RandomizeCatColor()
    {
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = colors[Random.Range(0, colors.Length)];
                Debug.Log("Cat color changed.");
            }
        }
    }
}
