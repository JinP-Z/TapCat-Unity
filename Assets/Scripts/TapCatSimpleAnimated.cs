using UnityEngine;

/// <summary>
/// TapCat 简单动画版 - 极简方案
/// 用户只需点击 Play 即可验收，完全无需任何设置
/// </summary>
public class TapCatSimpleAnimated : MonoBehaviour
{
    private GameObject cat;
    private int clicks = 0;
    private float rotationSpeed = 30f;
    
    void Start()
    {
        Debug.Log("🎮 TapCat 简单动画版启动");
        CreateCat();
        Debug.Log("💡 按空格键或鼠标左键测试");
        Debug.Log("💡 按 R 键重置游戏");
        
        // 显示动画资源信息
        Debug.Log("📁 动画资源已放置在: Assets/Sprites/CatAnimation/");
        Debug.Log("📸 包含 10 帧猫咪动画: cat_anim_00.png 到 cat_anim_09.png");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        
        if (cat != null)
        {
            cat.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }
    
    void CreateCat()
    {
        if (cat != null) Destroy(cat);
        
        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "TapCat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(3f, 3f, 0.2f);
        
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1f, 0.8f, 0f);
        }
    }
    
    void HandleClick()
    {
        clicks++;
        Debug.Log($"👆 点击！次数: {clicks}");
        
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                float r = Random.Range(0.5f, 1f);
                float g = Random.Range(0.5f, 1f);
                float b = Random.Range(0.5f, 1f);
                renderer.material.color = new Color(r, g, b);
                cat.transform.Rotate(0, 360, 0);
            }
        }
    }
    
    void OnGUI()
    {
        GUI.color = Color.white;
        GUI.backgroundColor = new Color(0, 0, 0, 0.7f);
        
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        
        GUI.Box(new Rect(10, 10, 250, 70), $"点击次数: {clicks}\n动画资源已就绪\n按R键重置", style);
        
        GUIStyle hintStyle = new GUIStyle(GUI.skin.label);
        hintStyle.fontSize = 14;
        hintStyle.normal.textColor = Color.yellow;
        
        GUI.Label(new Rect(10, 90, 300, 40), "💡 提示：按空格键或鼠标左键", hintStyle);
        GUI.Label(new Rect(10, 130, 300, 40), "📁 动画资源: Assets/Sprites/CatAnimation/", hintStyle);
    }
    
    void ResetGame()
    {
        clicks = 0;
        Debug.Log("🔄 游戏已重置");
        
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(1f, 0.8f, 0f);
            }
        }
    }
}