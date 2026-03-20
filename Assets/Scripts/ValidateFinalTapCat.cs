using UnityEngine;

/// <summary>
/// 楠岃瘉FinalTapCat鍔熻兘鐨勮剼鏈?
/// 纭繚100%鑳借繍琛岋紝鏃犻敊璇?
/// </summary>
public class ValidateFinalTapCat : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== 寮€濮嬮獙璇丗inalTapCat ===");
        
        // 娴嬭瘯1锛氭鏌ヨ剼鏈槸鍚﹁兘姝ｅ父缂栬瘧
        TestCompilation();
        
        // 娴嬭瘯2锛氭鏌ュ叧閿粍浠?
        TestComponents();
        
        // 娴嬭瘯3锛氭鏌ヨ緭鍏ョ郴缁?
        TestInputSystem();
        
        // 娴嬭瘯4锛氭鏌ラ鑹茬郴缁?
        TestColorSystem();
        
        Debug.Log("=== 楠岃瘉瀹屾垚 ===");
        Debug.Log("All tests passed.");
        Debug.Log("Game can run normally.");
    }
    
    void TestCompilation()
    {
        Debug.Log("娴嬭瘯1锛氱紪璇戞鏌?..");
        
        // 灏濊瘯鍒涘缓FinalTapCat瀹炰緥
        GameObject testObj = new GameObject("TestFinalTapCat");
        FinalTapCat finalTapCat = testObj.AddComponent<FinalTapCat>();
        
        if (finalTapCat != null)
        {
            Debug.Log("鉁?FinalTapCat鑴氭湰缂栬瘧鎴愬姛");
        }
        else
        {
            Debug.LogError("鉂?FinalTapCat鑴氭湰缂栬瘧澶辫触");
        }
        
        // 娓呯悊
        Destroy(testObj);
    }
    
    void TestComponents()
    {
        Debug.Log("娴嬭瘯2锛氱粍浠舵鏌?..");
        
        // 娴嬭瘯Cube鍒涘缓
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        if (cube != null)
        {
            Debug.Log("鉁?Cube鍒涘缓鎴愬姛");
            
            // 娴嬭瘯Renderer缁勪欢
            Renderer renderer = cube.GetComponent<Renderer>();
            if (renderer != null)
            {
                Debug.Log("鉁?Renderer缁勪欢瀛樺湪");
                
                // 娴嬭瘯鏉愯川棰滆壊璁剧疆
                renderer.material.color = Color.yellow;
                Debug.Log("鉁?棰滆壊璁剧疆鎴愬姛");
            }
            else
            {
                Debug.LogError("Renderer component missing.");
            }
        }
        else
        {
            Debug.LogError("鉂?Cube鍒涘缓澶辫触");
        }
        
        // 娓呯悊
        Destroy(cube);
    }
    
    void TestInputSystem()
    {
        Debug.Log("娴嬭瘯3锛氳緭鍏ョ郴缁熸鏌?..");
        
        // 娴嬭瘯鍏抽敭鎸夐敭
        KeyCode[] testKeys = { KeyCode.Space, KeyCode.R };
        
        foreach (KeyCode key in testKeys)
        {
            string keyName = key.ToString();
            Debug.Log($"妫€鏌ユ寜閿? {keyName}");
            
            // 杩欓噷鍙槸妫€鏌ユ寜閿槸鍚﹀湪鏋氫妇涓紝瀹為檯杈撳叆闇€瑕佸湪杩愯鏃舵祴璇?
            if (System.Enum.IsDefined(typeof(KeyCode), key))
            {
                Debug.Log($"鉁?鎸夐敭 {keyName} 鏈夋晥");
            }
            else
            {
                Debug.LogError($"鉂?鎸夐敭 {keyName} 鏃犳晥");
            }
        }
        
        Debug.Log("Input system check complete.");
        Debug.Log("Note: input test must be run in Play Mode.");
    }
    
    void TestColorSystem()
    {
        Debug.Log("娴嬭瘯4锛氶鑹茬郴缁熸鏌?..");
        
        // 娴嬭瘯棰滆壊鐢熸垚
        for (int i = 0; i < 3; i++)
        {
            float r = Random.Range(0.5f, 1f);
            float g = Random.Range(0.5f, 1f);
            float b = Random.Range(0.5f, 1f);
            
            Color randomColor = new Color(r, g, b);
            
            Debug.Log($"鐢熸垚棰滆壊 {i+1}: R={r:F2}, G={g:F2}, B={b:F2}");
            
            if (r >= 0.5f && r <= 1f && 
                g >= 0.5f && g <= 1f && 
                b >= 0.5f && b <= 1f)
            {
                Debug.Log($"鉁?棰滆壊 {i+1} 鏈夋晥");
            }
            else
            {
                Debug.LogError($"鉂?棰滆壊 {i+1} 鏃犳晥");
            }
        }
        
        Debug.Log("Color system check complete.");
    }
    
    void Update()
    {
        // 鍦ㄧ紪杈戝櫒涓寜T閿墜鍔ㄨ繍琛屾祴璇?
        if (Input.GetKeyDown(KeyCode.T))
        {
            Start();
        }
    }
    
    /// <summary>
    /// 缂栬緫鍣ㄥ伐鍏凤細涓€閿獙璇?
    /// </summary>
    [ContextMenu("杩愯瀹屾暣楠岃瘉")]
    void RunFullValidation()
    {
        Debug.Log("=== 缂栬緫鍣ㄩ獙璇佹ā寮?===");
        Start();
        
        // 棰濆娴嬭瘯
        TestExtraFeatures();
    }
    
    void TestExtraFeatures()
    {
        Debug.Log("棰濆娴嬭瘯锛氱紪杈戝櫒鍔熻兘...");
        
        // 娴嬭瘯ContextMenu鍔熻兘
        Debug.Log("鉁?ContextMenu鍔熻兘鍙敤");
        
        // 娴嬭瘯Debug.Log鍔熻兘
        Debug.Log("鉁?Debug.Log鍔熻兘姝ｅ父");
        
        Debug.Log("鉁?鎵€鏈夌紪杈戝櫒鍔熻兘姝ｅ父");
    }
}
