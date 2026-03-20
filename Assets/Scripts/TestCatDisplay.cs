using UnityEngine;

/// <summary>
/// 鏈€绠€鍗曠殑鐚挭鏄剧ず娴嬭瘯鑴氭湰
/// 涓嶄緷璧栦换浣曞叾浠栬剼鏈紝100%鑳芥樉绀虹尗鍜?
/// </summary>
public class TestCatDisplay : MonoBehaviour
{
    [Header("鐚挭璁剧疆")]
    public Color catColor = Color.yellow; // 榛勮壊鏇存樉鐪?
    public float catSize = 3f;
    
    [Header("浣嶇疆")]
    public Vector3 position = new Vector3(0, 0, 0);
    
    [Header("鑷姩杩愯")]
    public bool runOnStart = true;
    
    private GameObject catObject;
    
    void Start()
    {
        if (runOnStart)
        {
            ShowCat();
        }
    }
    
    /// <summary>
    /// 鏄剧ず鐚挭
    /// </summary>
    public void ShowCat()
    {
        Debug.Log("=== 寮€濮嬫樉绀虹尗鍜?===");
        
        // 鍒犻櫎鏃х殑鐚挭瀵硅薄锛堝鏋滄湁锛?
        if (catObject != null)
        {
            Destroy(catObject);
        }
        
        // 鍒涘缓鏂扮殑鐚挭瀵硅薄
        catObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        catObject.name = "TestCat";
        catObject.transform.position = position;
        catObject.transform.localScale = new Vector3(catSize, catSize, 0.1f);
        
        // 璁剧疆棰滆壊
        Renderer renderer = catObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = catColor;
            Debug.Log($"璁剧疆鐚挭棰滆壊: {catColor}");
        }
        
        // 娣诲姞鏃嬭浆鍔ㄧ敾锛堣鐚挭鍔ㄨ捣鏉ワ級
        catObject.AddComponent<Rotator>();
        
        Debug.Log($"鐚挭鍒涘缓瀹屾垚锛佷綅缃? {position}, 澶у皬: {catSize}");
        Debug.Log("=== 鐚挭鏄剧ず瀹屾垚 ===");
    }
    
    /// <summary>
    /// 鏇存敼鐚挭棰滆壊
    /// </summary>
    public void ChangeColor(Color newColor)
    {
        catColor = newColor;
        if (catObject != null)
        {
            Renderer renderer = catObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = newColor;
            }
        }
    }
    
    /// <summary>
    /// 璁╃尗鍜烦鑸烇紙鏃嬭浆锛?
    /// </summary>
    public void MakeCatDance()
    {
        if (catObject != null)
        {
            Rotator rotator = catObject.GetComponent<Rotator>();
            if (rotator == null)
            {
                rotator = catObject.AddComponent<Rotator>();
            }
            rotator.rotationSpeed = 180f; // 蹇€熸棆杞?
        }
    }
    
    /// <summary>
    /// 鍋滄璺宠垶
    /// </summary>
    public void StopDancing()
    {
        if (catObject != null)
        {
            Rotator rotator = catObject.GetComponent<Rotator>();
            if (rotator != null)
            {
                rotator.rotationSpeed = 0f;
            }
        }
    }
    
    /// <summary>
    /// 鍦ㄧ紪杈戝櫒涓祴璇?
    /// </summary>
    [ContextMenu("娴嬭瘯鏄剧ず鐚挭")]
    private void TestInEditor()
    {
        ShowCat();
    }
    
    [ContextMenu("Make Cat Dance")]
    private void TestDance()
    {
        MakeCatDance();
    }
    
    [ContextMenu("鍙樻垚绾㈣壊")]
    private void TestRed()
    {
        ChangeColor(Color.red);
    }
    
    [ContextMenu("鍙樻垚钃濊壊")]
    private void TestBlue()
    {
        ChangeColor(Color.blue);
    }
    
    [ContextMenu("鍙樻垚缁胯壊")]
    private void TestGreen()
    {
        ChangeColor(Color.green);
    }
}

/// <summary>
/// 绠€鍗曠殑鏃嬭浆缁勪欢
/// </summary>
public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 90f;
    
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
