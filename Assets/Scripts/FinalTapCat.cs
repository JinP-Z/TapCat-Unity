using UnityEngine;

/// <summary>
/// 最终版TapCat - 最简单，最可靠，100%能运行
/// 所有功能在一个文件中，零依赖，自动设置
/// </summary>
public class FinalTapCat : MonoBehaviour
{
    // 猫咪对象
    private GameObject cat;
    
    // 点击计数
    private int clicks = 0;
    
    // 旋转速度
    private float rotationSpeed = 30f;
    
    void Start()
    {
        Debug.Log("🎮 TapCat游戏启动！");
        
        // 自动创建猫咪
        CreateCat();
        
        // 显示提示
        Debug.Log("💡 提示：按空格键或鼠标左键点击猫咪");
        Debug.Log("💡 提示：按R键重置游戏");
    }
    
    void Update()
    {
        // 检测空格键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleClick();
        }
        
        // 检测鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
        
        // 检测重置键
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        
        // 持续旋转
        if (cat != null)
        {
            cat.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
    
    /// <summary>
    /// 创建猫咪（使用Cube，100%可靠）
    /// </summary>
    void CreateCat()
    {
        // 删除旧的猫咪（如果有）
        if (cat != null)
        {
            Destroy(cat);
        }
        
        // 创建新的猫咪
        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "TapCat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(3f, 3f, 0.2f);
        
        // 设置颜色
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1f, 0.8f, 0f); // 亮黄色
        }
        
        Debug.Log("🐱 猫咪创建成功！");
    }
    
    /// <summary>
    /// 处理点击
    /// </summary>
    void HandleClick()
    {
        clicks++;
        Debug.Log($"👆 点击！次数: {clicks}");
        
        // 改变猫咪颜色
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                // 生成随机颜色
                float r = Random.Range(0.5f, 1f);
                float g = Random.Range(0.5f, 1f);
                float b = Random.Range(0.5f, 1f);
                renderer.material.color = new Color(r, g, b);
                
                // 点击时的旋转效果
                cat.transform.Rotate(0, 360, 0);
            }
        }
        
        // 在屏幕上显示计数（使用GUI，最简单的方法）
        ShowClickCount();
    }
    
    /// <summary>
    /// 在屏幕上显示点击计数
    /// </summary>
    void OnGUI()
    {
        // 创建简单的GUI显示
        GUI.color = Color.white;
        GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
        
        // 显示点击计数
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        
        GUI.Box(new Rect(10, 10, 200, 60), $"点击次数: {clicks}\n按R键重置", style);
        
        // 显示操作提示
        GUIStyle hintStyle = new GUIStyle(GUI.skin.label);
        hintStyle.fontSize = 14;
        hintStyle.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 80, 300, 40), "💡 提示：按空格键或鼠标左键点击", hintStyle);
    }
    
    /// <summary>
    /// 显示点击计数（备用方法）
    /// </summary>
    void ShowClickCount()
    {
        // 这个方法被HandleClick调用，但主要显示在OnGUI中
    }
    
    /// <summary>
    /// 重置游戏
    /// </summary>
    void ResetGame()
    {
        clicks = 0;
        Debug.Log("🔄 游戏已重置");
        
        // 重置猫咪颜色
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(1f, 0.8f, 0f); // 亮黄色
            }
        }
    }
    
    /// <summary>
    /// 编辑器工具
    /// </summary>
    [ContextMenu("测试点击")]
    void TestClick()
    {
        HandleClick();
    }
    
    [ContextMenu("重置猫咪")]
    void ResetCat()
    {
        CreateCat();
    }
    
    [ContextMenu("随机颜色")]
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
                Debug.Log("🎨 猫咪颜色已改变");
            }
        }
    }
}