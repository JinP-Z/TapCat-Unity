using UnityEngine;

/// <summary>
/// Tests if the project can compile successfully.
/// </summary>
public class CompilationTest : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== Compilation Test Started ===");
        
        // Test basic Unity functionality
        TestBasicUnity();
        
        // Test UI functionality
        TestUI();
        
        // Test TextMeshPro functionality
        TestTextMeshPro();
        
        Debug.Log("=== Compilation Test Completed ===");
    }
    
    private void TestBasicUnity()
    {
        try
        {
            GameObject testObj = new GameObject("TestObject");
            Debug.Log("✓ GameObject creation: PASSED");
            
            testObj.AddComponent<Rigidbody>();
            Debug.Log("✓ Component addition: PASSED");
            
            GameObject.Destroy(testObj);
            Debug.Log("✓ Object destruction: PASSED");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Basic Unity test failed: {e.Message}");
        }
    }
    
    private void TestUI()
    {
        try
        {
            // Test if UI namespace is available
            System.Type uiTextType = System.Type.GetType("UnityEngine.UI.Text, UnityEngine.UI");
            if (uiTextType != null)
            {
                Debug.Log("✓ UI namespace: AVAILABLE");
            }
            else
            {
                Debug.LogWarning("UI namespace: NOT AVAILABLE (TextMeshPro might be used instead)");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"UI test warning: {e.Message}");
        }
    }
    
    private void TestTextMeshPro()
    {
        try
        {
            // Test if TextMeshPro namespace is available
            System.Type tmpType = System.Type.GetType("TMPro.TextMeshProUGUI, Unity.TextMeshPro");
            if (tmpType != null)
            {
                Debug.Log("✓ TextMeshPro namespace: AVAILABLE");
            }
            else
            {
                Debug.LogWarning("TextMeshPro namespace: NOT AVAILABLE (check Package Manager)");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"TextMeshPro test warning: {e.Message}");
        }
    }
    
    [ContextMenu("Run Compilation Test")]
    private void RunTest()
    {
        Start();
    }
}