using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 绠€鍗曠尗鍜樉绀轰慨澶嶅伐鍏?
/// 涓嶄緷璧栧叾浠栬剼鏈紝纭繚鐚挭涓€瀹氭樉绀?
/// </summary>
public class SimpleCatFixer : MonoBehaviour
{
    [Header("鐚挭璁剧疆")]
    [SerializeField] private Color catColor = new Color(1f, 0.5f, 0f, 1f); // 姗欒壊
    [SerializeField] private Vector3 catPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 catScale = new Vector3(2f, 2f, 1f); // 鏀惧ぇ涓€鐐?
    
    [Header("璋冭瘯")]
    [SerializeField] private bool showDebugInfo = true;
    
    private GameObject tapCatObject;
    
    void Start()
    {
        FixCatNow();
    }
    
    /// <summary>
    /// 绔嬪嵆淇鐚挭鏄剧ず
    /// </summary>
    public void FixCatNow()
    {
        Debug.Log("寮€濮嬬畝鍗曚慨澶嶇尗鍜樉绀?..");
        
        // 1. 纭繚TapCat瀵硅薄瀛樺湪
        tapCatObject = GameObject.Find("TapCat");
        
        if (tapCatObject == null)
        {
            Debug.Log("鍒涘缓TapCat瀵硅薄");
            tapCatObject = new GameObject("TapCat");
            tapCatObject.transform.position = catPosition;
            tapCatObject.transform.localScale = catScale;
        }
        else
        {
            Debug.Log("鎵惧埌宸插瓨鍦ㄧ殑TapCat瀵硅薄");
        }
        
        // 2. 纭繚SpriteRenderer瀛樺湪
        SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.Log("娣诲姞SpriteRenderer缁勪欢");
            spriteRenderer = tapCatObject.AddComponent<SpriteRenderer>();
        }
        
        // 3. 璁剧疆鐚挭鏄剧ず
        SetCatAppearance(spriteRenderer);
        
        // 4. 纭繚浣嶇疆姝ｇ‘
        tapCatObject.transform.position = catPosition;
        tapCatObject.transform.localScale = catScale;
        
        Debug.Log("绠€鍗曚慨澶嶅畬鎴愶紒");
        
        if (showDebugInfo)
        {
            ShowDebugInfo();
        }
    }
    
    /// <summary>
    /// 璁剧疆鐚挭澶栬
    /// </summary>
    private void SetCatAppearance(SpriteRenderer spriteRenderer)
    {
        // 鍏堝皾璇曞姞杞藉浘鐗?
        bool spriteLoaded = false;
        
        // 鏂规硶1锛氫粠Resources鍔犺浇锛堟爣鍑嗚矾寰勶級
        Sprite catSprite = Resources.Load<Sprite>("Sprites/PlaceholderCat");
        if (catSprite != null)
        {
            spriteRenderer.sprite = catSprite;
            spriteLoaded = true;
            Debug.Log("鎴愬姛鍔犺浇鐚挭鍥剧墖: " + catSprite.name);
        }
        else
        {
            Debug.LogWarning("Resources/Sprites/PlaceholderCat not found.");
        }
        
        // 鏂规硶2锛氬皾璇曞叾浠栬矾寰?
        if (!spriteLoaded)
        {
            catSprite = Resources.Load<Sprite>("PlaceholderCat");
            if (catSprite != null)
            {
                spriteRenderer.sprite = catSprite;
                spriteLoaded = true;
                Debug.Log("鎴愬姛鍔犺浇鐚挭鍥剧墖锛堟牴鐩綍锛? " + catSprite.name);
            }
        }
        
        // 鏂规硶3锛氬垱寤轰复鏃剁簿鐏?
        if (!spriteLoaded)
        {
            Debug.Log("鍒涘缓涓存椂褰╄壊鏂瑰潡浣滀负鐚挭");
            
            // 鍒涘缓绠€鍗曠殑2D鏂瑰潡
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "TempCatCube";
            cube.transform.position = catPosition;
            cube.transform.localScale = new Vector3(1f, 1f, 0.1f); // 鎵佸钩鐨勬柟鍧?
            
            // 璁剧疆棰滆壊
            Renderer cubeRenderer = cube.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = catColor;
            }
            
            // 閿€姣佹棫鐨凾apCat瀵硅薄锛屼娇鐢ㄦ柊鐨勬柟鍧?
            if (tapCatObject != null && tapCatObject != cube)
            {
                Destroy(tapCatObject);
            }
            
            tapCatObject = cube;
            tapCatObject.name = "TapCat";
            
            Debug.Log("鍒涘缓涓存椂鐚挭鏂瑰潡瀹屾垚");
            return;
        }
        
        // 濡傛灉鍔犺浇浜嗗浘鐗囷紝璁剧疆棰滆壊涓虹櫧鑹诧紙涓嶆敼鍙樺浘鐗囬鑹诧級
        spriteRenderer.color = Color.white;
    }
    
    /// <summary>
    /// 鏄剧ず璋冭瘯淇℃伅
    /// </summary>
    private void ShowDebugInfo()
    {
        Debug.Log("=== 绠€鍗曠尗鍜慨澶嶈皟璇曚俊鎭?===");
        Debug.Log($"瀵硅薄: {tapCatObject.name}");
        Debug.Log($"浣嶇疆: {tapCatObject.transform.position}");
        Debug.Log($"缂╂斁: {tapCatObject.transform.localScale}");
        
        SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Debug.Log($"SpriteRenderer: 瀛樺湪");
            Debug.Log($"Sprite: {(spriteRenderer.sprite != null ? spriteRenderer.sprite.name : "None")}");
            Debug.Log($"棰滆壊: {spriteRenderer.color}");
        }
        
        Debug.Log("==========================");
    }
    
    /// <summary>
    /// 缂栬緫鍣ㄥ伐鍏凤細蹇€熶慨澶?
    /// </summary>
    [ContextMenu("Quick Fix Cat")]
    private void QuickFixInEditor()
    {
        FixCatNow();
    }
    
    /// <summary>
    /// 鏇存敼鐚挭棰滆壊
    /// </summary>
    public void ChangeCatColor(Color newColor)
    {
        catColor = newColor;
        SpriteRenderer spriteRenderer = tapCatObject?.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
        }
    }
    
    /// <summary>
    /// 鏇存敼鐚挭澶у皬
    /// </summary>
    public void ChangeCatSize(float size)
    {
        catScale = new Vector3(size, size, 1f);
        if (tapCatObject != null)
        {
            tapCatObject.transform.localScale = catScale;
        }
    }
}
