using UnityEngine;

/// <summary>
/// 娴嬭瘯缂栬瘧鑴氭湰
/// </summary>
public class TestCompile : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Test compile script started.");
        
        // 娴嬭瘯SimpleCatController鏄惁瀛樺湪
        System.Type type = System.Type.GetType("SimpleCatController");
        if (type != null)
        {
            Debug.Log("SimpleCatController class found.");
        }
        else
        {
            Debug.LogError("SimpleCatController绫讳笉瀛樺湪鎴栧懡鍚嶇┖闂存湁闂");
        }
        
        // 娴嬭瘯鍏朵粬鍏抽敭绫?
        TestClass("TapCatController");
        TestClass("AnimationManager");
        TestClass("InputHandler");
        TestClass("CounterUI");
    }
    
    void TestClass(string className)
    {
        System.Type type = System.Type.GetType(className);
        if (type != null)
        {
            Debug.Log($"{className} class found.");
        }
        else
        {
            Debug.LogError($"{className}绫讳笉瀛樺湪");
        }
    }
}
