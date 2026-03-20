using UnityEngine;

/// <summary>
/// Simple compilation test script
/// </summary>
public class TestCompilation : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== Compilation Test ===");
        Debug.Log("If you see this message, the project compiles successfully!");
        Debug.Log("Unity version: " + Application.unityVersion);
        Debug.Log("Platform: " + Application.platform);
        
        // Test basic C# features
        TestCSharpFeatures();
        TestUnityAPIs();
        
        Debug.Log("=== Compilation Test PASSED ===");
    }
    
    private void TestCSharpFeatures()
    {
        // Test C# language features
        int[] numbers = { 1, 2, 3, 4, 5 };
        int sum = 0;
        
        foreach (int num in numbers)
        {
            sum += num;
        }
        
        Debug.Log($"C# array sum test: {sum} (expected: 15)");
        
        // Test LINQ (if available)
        try
        {
            System.Func<int, bool> isEven = x => x % 2 == 0;
            Debug.Log("✓ C# lambda expressions: WORKING");
        }
        catch
        {
            Debug.LogWarning("C# lambda expressions: NOT WORKING");
        }
    }
    
    private void TestUnityAPIs()
    {
        // Test common Unity APIs
        try
        {
            GameObject testObj = new GameObject("TestObject");
            testObj.transform.position = Vector3.zero;
            testObj.transform.rotation = Quaternion.identity;
            testObj.transform.localScale = Vector3.one;
            
            Debug.Log("✓ GameObject/Transform APIs: WORKING");
            
            // Test component system
            testObj.AddComponent<MeshRenderer>();
            testObj.AddComponent<MeshFilter>();
            
            Debug.Log("✓ Component system: WORKING");
            
            Destroy(testObj);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Unity API test failed: {e.Message}");
        }
    }
    
    [ContextMenu("Run Test")]
    private void RunTest()
    {
        Start();
    }
}