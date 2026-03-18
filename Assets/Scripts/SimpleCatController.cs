using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 简单猫咪控制器
/// 处理点击响应和颜色变化
/// </summary>
public class SimpleCatController : MonoBehaviour
{
    private int tapCount = 0;
    private float rotationSpeed = 0f;
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1f, 0.5f, 0f, 1f); // 橙色
        }
        
        Debug.Log("SimpleCatController启动");
    }
    
    void Update()
    {
        // 检测点击
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            OnTap();
        }
        
        // 旋转动画
        if (rotationSpeed > 0)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            rotationSpeed = Mathf.Lerp(rotationSpeed, 30f, Time.deltaTime); // 逐渐减速
        }
    }
    
    void OnTap()
    {
        tapCount++;
        Debug.Log($"点击！次数: {tapCount}");
        
        // 改变颜色
        if (spriteRenderer != null)
        {
            Color randomColor = new Color(
                Random.Range(0.5f, 1f),
                Random.Range(0.5f, 1f),
                Random.Range(0.5f, 1f),
                1f
            );
            spriteRenderer.color = randomColor;
        }
        
        // 增加旋转速度
        rotationSpeed = 180f;
        
        // 更新UI（如果存在）
        UpdateUI();
    }
    
    void UpdateUI()
    {
        GameObject uiText = GameObject.Find("TapCountText");
        if (uiText != null)
        {
            TextMeshProUGUI textComponent = uiText.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = $"点击次数: {tapCount}";
            }
        }
    }
    
    /// <summary>
    /// 获取当前点击次数
    /// </summary>
    public int GetTapCount()
    {
        return tapCount;
    }
    
    /// <summary>
    /// 重置点击次数
    /// </summary>
    public void ResetTapCount()
    {
        tapCount = 0;
        UpdateUI();
    }
    
    /// <summary>
    /// 设置猫咪颜色
    /// </summary>
    public void SetCatColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }
    
    /// <summary>
    /// 编辑器工具：测试点击
    /// </summary>
    [ContextMenu("测试点击")]
    private void TestTap()
    {
        OnTap();
    }
}