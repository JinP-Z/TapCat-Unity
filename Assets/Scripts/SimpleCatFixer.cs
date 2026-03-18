using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 简单猫咪显示修复工具
/// 不依赖其他脚本，确保猫咪一定显示
/// </summary>
public class SimpleCatFixer : MonoBehaviour
{
    [Header("猫咪设置")]
    [SerializeField] private Color catColor = new Color(1f, 0.5f, 0f, 1f); // 橙色
    [SerializeField] private Vector3 catPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 catScale = new Vector3(2f, 2f, 1f); // 放大一点
    
    [Header("调试")]
    [SerializeField] private bool showDebugInfo = true;
    
    private GameObject tapCatObject;
    
    void Start()
    {
        FixCatNow();
    }
    
    /// <summary>
    /// 立即修复猫咪显示
    /// </summary>
    public void FixCatNow()
    {
        Debug.Log("开始简单修复猫咪显示...");
        
        // 1. 确保TapCat对象存在
        tapCatObject = GameObject.Find("TapCat");
        
        if (tapCatObject == null)
        {
            Debug.Log("创建TapCat对象");
            tapCatObject = new GameObject("TapCat");
            tapCatObject.transform.position = catPosition;
            tapCatObject.transform.localScale = catScale;
        }
        else
        {
            Debug.Log("找到已存在的TapCat对象");
        }
        
        // 2. 确保SpriteRenderer存在
        SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.Log("添加SpriteRenderer组件");
            spriteRenderer = tapCatObject.AddComponent<SpriteRenderer>();
        }
        
        // 3. 设置猫咪显示
        SetCatAppearance(spriteRenderer);
        
        // 4. 确保位置正确
        tapCatObject.transform.position = catPosition;
        tapCatObject.transform.localScale = catScale;
        
        Debug.Log("简单修复完成！");
        
        if (showDebugInfo)
        {
            ShowDebugInfo();
        }
    }
    
    /// <summary>
    /// 设置猫咪外观
    /// </summary>
    private void SetCatAppearance(SpriteRenderer spriteRenderer)
    {
        // 先尝试加载图片
        bool spriteLoaded = false;
        
        // 方法1：从Resources加载（标准路径）
        Sprite catSprite = Resources.Load<Sprite>("Sprites/PlaceholderCat");
        if (catSprite != null)
        {
            spriteRenderer.sprite = catSprite;
            spriteLoaded = true;
            Debug.Log("成功加载猫咪图片: " + catSprite.name);
        }
        else
        {
            Debug.LogWarning("Resources/Sprites/PlaceholderCat 未找到");
        }
        
        // 方法2：尝试其他路径
        if (!spriteLoaded)
        {
            catSprite = Resources.Load<Sprite>("PlaceholderCat");
            if (catSprite != null)
            {
                spriteRenderer.sprite = catSprite;
                spriteLoaded = true;
                Debug.Log("成功加载猫咪图片（根目录）: " + catSprite.name);
            }
        }
        
        // 方法3：创建临时精灵
        if (!spriteLoaded)
        {
            Debug.Log("创建临时彩色方块作为猫咪");
            
            // 创建简单的2D方块
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = "TempCatCube";
            cube.transform.position = catPosition;
            cube.transform.localScale = new Vector3(1f, 1f, 0.1f); // 扁平的方块
            
            // 设置颜色
            Renderer cubeRenderer = cube.GetComponent<Renderer>();
            if (cubeRenderer != null)
            {
                cubeRenderer.material.color = catColor;
            }
            
            // 销毁旧的TapCat对象，使用新的方块
            if (tapCatObject != null && tapCatObject != cube)
            {
                Destroy(tapCatObject);
            }
            
            tapCatObject = cube;
            tapCatObject.name = "TapCat";
            
            Debug.Log("创建临时猫咪方块完成");
            return;
        }
        
        // 如果加载了图片，设置颜色为白色（不改变图片颜色）
        spriteRenderer.color = Color.white;
    }
    
    /// <summary>
    /// 显示调试信息
    /// </summary>
    private void ShowDebugInfo()
    {
        Debug.Log("=== 简单猫咪修复调试信息 ===");
        Debug.Log($"对象: {tapCatObject.name}");
        Debug.Log($"位置: {tapCatObject.transform.position}");
        Debug.Log($"缩放: {tapCatObject.transform.localScale}");
        
        SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Debug.Log($"SpriteRenderer: 存在");
            Debug.Log($"Sprite: {spriteRenderer.sprite?.name ?? "无"}");
            Debug.Log($"颜色: {spriteRenderer.color}");
        }
        
        Debug.Log("==========================");
    }
    
    /// <summary>
    /// 编辑器工具：快速修复
    /// </summary>
    [ContextMenu("快速修复猫咪")]
    private void QuickFixInEditor()
    {
        FixCatNow();
    }
    
    /// <summary>
    /// 更改猫咪颜色
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
    /// 更改猫咪大小
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