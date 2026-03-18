using UnityEngine;

/// <summary>
/// 最简单的猫咪显示测试脚本
/// 不依赖任何其他脚本，100%能显示猫咪
/// </summary>
public class TestCatDisplay : MonoBehaviour
{
    [Header("猫咪设置")]
    public Color catColor = Color.yellow; // 黄色更显眼
    public float catSize = 3f;
    
    [Header("位置")]
    public Vector3 position = new Vector3(0, 0, 0);
    
    [Header("自动运行")]
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
    /// 显示猫咪
    /// </summary>
    public void ShowCat()
    {
        Debug.Log("=== 开始显示猫咪 ===");
        
        // 删除旧的猫咪对象（如果有）
        if (catObject != null)
        {
            Destroy(catObject);
        }
        
        // 创建新的猫咪对象
        catObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        catObject.name = "TestCat";
        catObject.transform.position = position;
        catObject.transform.localScale = new Vector3(catSize, catSize, 0.1f);
        
        // 设置颜色
        Renderer renderer = catObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = catColor;
            Debug.Log($"设置猫咪颜色: {catColor}");
        }
        
        // 添加旋转动画（让猫咪动起来）
        catObject.AddComponent<Rotator>();
        
        Debug.Log($"猫咪创建完成！位置: {position}, 大小: {catSize}");
        Debug.Log("=== 猫咪显示完成 ===");
    }
    
    /// <summary>
    /// 更改猫咪颜色
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
    /// 让猫咪跳舞（旋转）
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
            rotator.rotationSpeed = 180f; // 快速旋转
        }
    }
    
    /// <summary>
    /// 停止跳舞
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
    /// 在编辑器中测试
    /// </summary>
    [ContextMenu("测试显示猫咪")]
    private void TestInEditor()
    {
        ShowCat();
    }
    
    [ContextMenu("让猫咪跳舞")]
    private void TestDance()
    {
        MakeCatDance();
    }
    
    [ContextMenu("变成红色")]
    private void TestRed()
    {
        ChangeColor(Color.red);
    }
    
    [ContextMenu("变成蓝色")]
    private void TestBlue()
    {
        ChangeColor(Color.blue);
    }
    
    [ContextMenu("变成绿色")]
    private void TestGreen()
    {
        ChangeColor(Color.green);
    }
}

/// <summary>
/// 简单的旋转组件
/// </summary>
public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 90f;
    
    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}