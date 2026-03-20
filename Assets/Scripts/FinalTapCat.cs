using UnityEngine;

/// <summary>
/// 鏈€缁堢増TapCat - 鏈€绠€鍗曪紝鏈€鍙潬锛?00%鑳借繍琛?
/// 鎵€鏈夊姛鑳藉湪涓€涓枃浠朵腑锛岄浂渚濊禆锛岃嚜鍔ㄨ缃?
/// </summary>
public class FinalTapCat : MonoBehaviour
{
    // 鐚挭瀵硅薄
    private GameObject cat;
    
    // 鐐瑰嚮璁℃暟
    private int clicks = 0;
    
    // 鏃嬭浆閫熷害
    private float rotationSpeed = 30f;
    
    void Start()
    {
        Debug.Log("TapCat started.");
        
        // 鑷姩鍒涘缓鐚挭
        CreateCat();
        
        // 鏄剧ず鎻愮ず
        Debug.Log("馃挕 鎻愮ず锛氭寜绌烘牸閿垨榧犳爣宸﹂敭鐐瑰嚮鐚挭");
        Debug.Log("Hint: press R to reset.");
    }
    
    void Update()
    {
        // 妫€娴嬬┖鏍奸敭
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleClick();
        }
        
        // 妫€娴嬮紶鏍囧乏閿?
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
        
        // 妫€娴嬮噸缃敭
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        
        // 鎸佺画鏃嬭浆
        if (cat != null)
        {
            cat.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
    
    /// <summary>
    /// 鍒涘缓鐚挭锛堜娇鐢–ube锛?00%鍙潬锛?
    /// </summary>
    void CreateCat()
    {
        // 鍒犻櫎鏃х殑鐚挭锛堝鏋滄湁锛?
        if (cat != null)
        {
            Destroy(cat);
        }
        
        // 鍒涘缓鏂扮殑鐚挭
        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "TapCat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(3f, 3f, 0.2f);
        
        // 璁剧疆棰滆壊
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1f, 0.8f, 0f); // 浜粍鑹?
        }
        
        Debug.Log("TapCat created.");
    }
    
    /// <summary>
    /// 澶勭悊鐐瑰嚮
    /// </summary>
    void HandleClick()
    {
        clicks++;
        Debug.Log($"馃憜 鐐瑰嚮锛佹鏁? {clicks}");
        
        // 鏀瑰彉鐚挭棰滆壊
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                // 鐢熸垚闅忔満棰滆壊
                float r = Random.Range(0.5f, 1f);
                float g = Random.Range(0.5f, 1f);
                float b = Random.Range(0.5f, 1f);
                renderer.material.color = new Color(r, g, b);
                
                // 鐐瑰嚮鏃剁殑鏃嬭浆鏁堟灉
                cat.transform.Rotate(0, 360, 0);
            }
        }
        
        // 鍦ㄥ睆骞曚笂鏄剧ず璁℃暟锛堜娇鐢℅UI锛屾渶绠€鍗曠殑鏂规硶锛?
        ShowClickCount();
    }
    
    /// <summary>
    /// 鍦ㄥ睆骞曚笂鏄剧ず鐐瑰嚮璁℃暟
    /// </summary>
    void OnGUI()
    {
        // 鍒涘缓绠€鍗曠殑GUI鏄剧ず
        GUI.color = Color.white;
        GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
        
        // 鏄剧ず鐐瑰嚮璁℃暟
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        
        GUI.Box(new Rect(10, 10, 200, 60), $"Clicks: {clicks}\\nPress R to reset", style);
        
        // 鏄剧ず鎿嶄綔鎻愮ず
        GUIStyle hintStyle = new GUIStyle(GUI.skin.label);
        hintStyle.fontSize = 14;
        hintStyle.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 80, 300, 40), "馃挕 鎻愮ず锛氭寜绌烘牸閿垨榧犳爣宸﹂敭鐐瑰嚮", hintStyle);
    }
    
    /// <summary>
    /// 鏄剧ず鐐瑰嚮璁℃暟锛堝鐢ㄦ柟娉曪級
    /// </summary>
    void ShowClickCount()
    {
        // 杩欎釜鏂规硶琚獺andleClick璋冪敤锛屼絾涓昏鏄剧ず鍦∣nGUI涓?
    }
    
    /// <summary>
    /// 閲嶇疆娓告垙
    /// </summary>
    void ResetGame()
    {
        clicks = 0;
        Debug.Log("Game reset.");
        
        // 閲嶇疆鐚挭棰滆壊
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(1f, 0.8f, 0f); // 浜粍鑹?
            }
        }
    }
    
    /// <summary>
    /// 缂栬緫鍣ㄥ伐鍏?
    /// </summary>
    [ContextMenu("娴嬭瘯鐐瑰嚮")]
    void TestClick()
    {
        HandleClick();
    }
    
    [ContextMenu("閲嶇疆鐚挭")]
    void ResetCat()
    {
        CreateCat();
    }
    
    [ContextMenu("闅忔満棰滆壊")]
    void RandomColor()
    {
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                float r = Random.Range(0.5f, 1f);
                float g = Random.Range(0.5f, 1f);
                float b = Random.Range(0.5f, 1f);
                renderer.material.color = new Color(r, g, b);
                Debug.Log("Cat color changed.");
            }
        }
    }
}
