using UnityEngine;

/// <summary>
/// 测试编译脚本
/// </summary>
public class TestCompile : MonoBehaviour
{
    void Start()
    {
        Debug.Log("测试编译脚本启动成功！");
        
        // 测试SimpleCatController是否存在
        System.Type type = System.Type.GetType("SimpleCatController");
        if (type != null)
        {
            Debug.Log("SimpleCatController类存在");
        }
        else
        {
            Debug.LogError("SimpleCatController类不存在或命名空间有问题");
        }
        
        // 测试其他关键类
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
            Debug.Log($"{className}类存在");
        }
        else
        {
            Debug.LogError($"{className}类不存在");
        }
    }
}