using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 终极简单版本 - 一个文件搞定一切
public class TapCatFinal : MonoBehaviour
{
    // 猫咪颜色
    public Color catColor = Color.yellow;
    
    // 猫咪大小
    public float catSize = 3f;
    
    // 私有变量
    private GameObject cat;
    private TextMeshProUGUI counterText;
    private int clicks = 0;
    
    // 游戏开始
    void Start()
    {
        Debug.Log("TapCat游戏启动！");
        CreateCat();
        CreateUI();
        Debug.Log("设置完成！按空格键或点击鼠标测试！");
    }
    
    // 每帧更新
    void Update()
    {
        // 检测点击
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            ClickCat();
        }
        
        // 猫咪旋转
        if (cat != null)
        {
            cat.transform.Rotate(0, 30f * Time.deltaTime, 0);
        }
    }
    
    // 创建猫咪
    void CreateCat()
    {
        // 删除旧的猫咪
        if (cat != null) Destroy(cat);
        
        // 创建新猫咪（使用Cube）
        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "Cat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(catSize, catSize, 0.1f);
        
        // 设置颜色
        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = catColor;
        }
    }
    
    // 创建UI
    void CreateUI()
    {
        // 创建Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // 创建计数器
        GameObject counterObj = new GameObject("Counter");
        counterObj.transform.SetParent(canvas.transform);
        
        // 设置位置
        RectTransform rect = counterObj.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(1, 1);
        rect.anchorMax = new Vector2(1, 1);
        rect.pivot = new Vector2(1, 1);
        rect.anchoredPosition = new Vector2(-20, -20);
        rect.sizeDelta = new Vector2(200, 80);
        
        // 添加背景
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(counterObj.transform);
        RectTransform bgRect = bg.AddComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.sizeDelta = Vector2.zero;
        
        Image bgImage = bg.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.7f);
        
        // 添加文本
        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(counterObj.transform);
        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 0.5f);
        textRect.anchorMax = new Vector2(1, 0.8f);
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = Vector2.zero;
        
        counterText = textObj.AddComponent<TextMeshProUGUI>();
        counterText.text = "点击: 0";
        counterText.fontSize = 24;
        counterText.color = Color.white;
        counterText.alignment = TextAlignmentOptions.Center;
    }
    
    // 点击猫咪
    void ClickCat()
    {
        clicks++;
        Debug.Log("点击！次数: " + clicks);
        
        // 更新UI
        if (counterText != null)
        {
            counterText.text = "点击: " + clicks;
        }
        
        // 改变颜色
        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color newColor = new Color(
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    Random.Range(0.5f, 1f),
                    1f
                );
                renderer.material.color = newColor;
                
                // 快速旋转
                cat.transform.Rotate(0, 180, 0);
            }
        }
    }
    
    // 编辑器工具
    [ContextMenu("创建猫咪")]
    void CreateCatInEditor()
    {
        CreateCat();
    }
    
    [ContextMenu("创建UI")]
    void CreateUIInEditor()
    {
        CreateUI();
    }
    
    [ContextMenu("测试点击")]
    void TestClick()
    {
        ClickCat();
    }
}