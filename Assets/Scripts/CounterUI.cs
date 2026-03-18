using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TapCat
{
    /// <summary>
    /// 计数器UI
    /// 负责显示点击次数和状态信息
    /// </summary>
    public class CounterUI : MonoBehaviour
    {
        // 单例实例
        public static CounterUI Instance { get; private set; }
        
        [Header("UI引用")]
        [SerializeField] private TextMeshProUGUI tapCountText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Image backgroundPanel;
        [SerializeField] private GameObject uiContainer;
        
        [Header("UI设置")]
        [SerializeField] private Color normalColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        [SerializeField] private Color highlightColor = new Color(0.2f, 0.4f, 0.8f, 0.9f);
        [SerializeField] private float highlightDuration = 0.3f;
        
        [Header("文本格式")]
        [SerializeField] private string tapCountFormat = "点击次数: {0}";
        [SerializeField] private string statusFormat = "状态: {0}";
        
        private TapCatController tapCatController;
        private float highlightTimer = 0f;
        private bool isHighlighted = false;
        
        private void Awake()
        {
            // 设置单例
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // 获取TapCatController引用
            tapCatController = FindObjectOfType<TapCatController>();
            
            // 初始化UI
            InitializeUI();
        }
        
        private void Start()
        {
            // 初始显示
            UpdateTapCount(0);
            UpdateStatus("等待输入...");
            
            Debug.Log("CounterUI: UI系统初始化完成");
        }
        
        private void Update()
        {
            // 处理高亮效果
            if (isHighlighted)
            {
                highlightTimer -= Time.deltaTime;
                if (highlightTimer <= 0)
                {
                    ResetBackgroundColor();
                }
            }
            
            // 自动更新点击次数（如果tapCatController存在）
            if (tapCatController != null)
            {
                UpdateTapCount(tapCatController.GetTapCount());
            }
        }
        
        /// <summary>
        /// 初始化UI
        /// </summary>
        private void InitializeUI()
        {
            // 确保UI元素存在
            if (tapCountText == null || statusText == null)
            {
                Debug.LogWarning("CounterUI: 部分UI元素未设置，将尝试自动查找");
                FindUIElements();
            }
            
            // 设置初始颜色
            if (backgroundPanel != null)
            {
                backgroundPanel.color = normalColor;
            }
        }
        
        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindUIElements()
        {
            // 在子对象中查找
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (var text in texts)
            {
                if (text.gameObject.name.Contains("TapCount") && tapCountText == null)
                {
                    tapCountText = text;
                }
                else if (text.gameObject.name.Contains("Status") && statusText == null)
                {
                    statusText = text;
                }
            }
            
            // 查找背景面板
            if (backgroundPanel == null)
            {
                backgroundPanel = GetComponentInChildren<Image>();
            }
        }
        
        /// <summary>
        /// 更新点击次数显示
        /// </summary>
        public void UpdateTapCount(int count)
        {
            if (tapCountText != null)
            {
                tapCountText.text = string.Format(tapCountFormat, count);
                
                // 当点击次数增加时高亮
                if (count > 0)
                {
                    HighlightBackground();
                }
            }
        }
        
        /// <summary>
        /// 更新状态显示
        /// </summary>
        public void UpdateStatus(string status)
        {
            if (statusText != null)
            {
                statusText.text = string.Format(statusFormat, status);
            }
        }
        
        /// <summary>
        /// 高亮背景
        /// </summary>
        public void HighlightBackground()
        {
            if (backgroundPanel != null)
            {
                backgroundPanel.color = highlightColor;
                isHighlighted = true;
                highlightTimer = highlightDuration;
            }
        }
        
        /// <summary>
        /// 重置背景颜色
        /// </summary>
        public void ResetBackgroundColor()
        {
            if (backgroundPanel != null)
            {
                backgroundPanel.color = normalColor;
                isHighlighted = false;
            }
        }
        
        /// <summary>
        /// 显示/隐藏UI
        /// </summary>
        public void SetUIVisible(bool visible)
        {
            if (uiContainer != null)
            {
                uiContainer.SetActive(visible);
            }
        }
        
        /// <summary>
        /// 切换UI显示
        /// </summary>
        public void ToggleUI()
        {
            if (uiContainer != null)
            {
                bool newState = !uiContainer.activeSelf;
                uiContainer.SetActive(newState);
                Debug.Log($"UI显示状态: {newState}");
            }
        }
        
        /// <summary>
        /// 设置UI透明度
        /// </summary>
        public void SetUIOpacity(float opacity)
        {
            if (backgroundPanel != null)
            {
                Color color = backgroundPanel.color;
                color.a = Mathf.Clamp(opacity, 0.1f, 1f);
                backgroundPanel.color = color;
            }
            
            if (tapCountText != null)
            {
                Color color = tapCountText.color;
                color.a = Mathf.Clamp(opacity, 0.5f, 1f);
                tapCountText.color = color;
            }
            
            if (statusText != null)
            {
                Color color = statusText.color;
                color.a = Mathf.Clamp(opacity, 0.5f, 1f);
                statusText.color = color;
            }
        }
        
        /// <summary>
        /// 获取当前点击次数
        /// </summary>
        public int GetCurrentTapCount()
        {
            if (tapCatController != null)
            {
                return tapCatController.GetTapCount();
            }
            return 0;
        }
    }
}