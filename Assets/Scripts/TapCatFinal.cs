using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 缁堟瀬绠€鍗曠増鏈?- 涓€涓枃浠舵悶瀹氫竴鍒?
public class TapCatFinal : MonoBehaviour
{
    // 鐚挭棰滆壊
    public Color catColor = Color.yellow;
    
    // 鐚挭澶у皬
    public float catSize = 3f;
    
    // 绉佹湁鍙橀噺
    private GameObject cat;
    private TextMeshProUGUI counterText;
    private int clicks = 0;
    
    // 娓告垙寮€濮?
    void Start()
    {
        Debug.Log("TapCat started.");
        CreateCat();
        CreateUI();
        Debug.Log("Setup complete. Press Space or click to test.");
    }
    
    // 姣忓抚鏇存柊
    void Update()
    {
        // 妫€娴嬬偣鍑?
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            ClickCat();
        }
        
        // 鐚挭鏃嬭浆
        if (cat != null)
        {
            cat.transform.Rotate(0, 30f * Time.deltaTime, 0);
        }
    }
    
    // 鍒涘缓鐚挭
    void CreateCat()
    {
        // 鍒犻櫎鏃х殑鐚挭
        if (cat != null) Destroy(cat);
        
        // 鍒涘缓鏂扮尗鍜紙浣跨敤Cube锛?
        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "Cat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(catSize, catSize, 0.1f);
        
        // 璁剧疆棰滆壊
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = catColor;
        }
    }
    
    // 鍒涘缓UI
    void CreateUI()
    {
        // 鍒涘缓Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 鍒涘缓璁℃暟鍣?
        GameObject counterObj = new GameObject("Counter");
        counterObj.transform.SetParent(canvas.transform);
        
        // 璁剧疆浣嶇疆
        RectTransform rect = counterObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-20, -20);
        rect.sizeDelta = new Vector2(200, 80);
        
        // 娣诲姞鑳屾櫙
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(counterObj.transform);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);
        
        // 娣诲姞鏂囨湰
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(counterObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0.5f);
        textRect.anchorMax = new Vector2(1, 0.8f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = Vector2.zero;
        
        counterText = textObj.AddComponent<TextMeshProUGUI>();
        counterText.text = "鐐瑰嚮: 0";
        counterText.fontSize = 24;
        counterText.color = Color.white;
        counterText.alignment = TextAlignmentOptions.Center;
    }
    
    // 鐐瑰嚮鐚挭
    void ClickCat()
    {
        clicks++;
        Debug.Log("鐐瑰嚮锛佹鏁? " + clicks);
        
        // 鏇存柊UI
        if (counterText != null)
        {
            counterText.text = "鐐瑰嚮: " + clicks;
        }
        
        // 鏀瑰彉棰滆壊
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
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
                cat.transform.Rotate(0, 180, 0);
            }
        }
    }
    
    // 缂栬緫鍣ㄥ伐鍏?
    [ContextMenu("鍒涘缓鐚挭")]
    void CreateCatInEditor()
    {
        CreateCat();
    }
    
    [ContextMenu("鍒涘缓UI")]
    void CreateUIInEditor()
    {
        CreateUI();
    }
    
    [ContextMenu("娴嬭瘯鐐瑰嚮")]
    void TestClick()
    {
        ClickCat();
    }
}
