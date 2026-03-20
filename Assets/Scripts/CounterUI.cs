using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TapCat
{
    /// <summary>
    /// 璁℃暟鍣║I
    /// 璐熻矗鏄剧ず鐐瑰嚮娆℃暟鍜岀姸鎬佷俊鎭?
    /// </summary>
    public class CounterUI : MonoBehaviour
    {
        // 鍗曚緥瀹炰緥
        public static CounterUI Instance { get; private set; }
        
        [Header("UI寮曠敤")]
        [SerializeField] private TextMeshProUGUI tapCountText;
        [SerializeField] private TextMeshProUGUI statusText;
        [SerializeField] private Image backgroundPanel;
        [SerializeField] private GameObject uiContainer;
        
        [Header("UI璁剧疆")]
        [SerializeField] private Color normalColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
        [SerializeField] private Color highlightColor = new Color(0.2f, 0.4f, 0.8f, 0.9f);
        [SerializeField] private float highlightDuration = 0.3f;
        
        [Header("鏂囨湰鏍煎紡")]
        [SerializeField] private string tapCountFormat = "鐐瑰嚮娆℃暟: {0}";
        [SerializeField] private string statusFormat = "鐘舵€? {0}";
        
        private TapCatController tapCatController;
        private float highlightTimer = 0f;
        private bool isHighlighted = false;
        
        private void Awake()
        {
            // 璁剧疆鍗曚緥
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            
            // 鑾峰彇TapCatController寮曠敤
            tapCatController = FindObjectOfType<TapCatController>();
            
            // 鍒濆鍖朥I
            InitializeUI();
        }
        
        private void Start()
        {
            // 鍒濆鏄剧ず
            UpdateTapCount(0);
            UpdateStatus("绛夊緟杈撳叆...");
            
            Debug.Log("CounterUI: UI initialized.");
        }
        
        private void Update()
        {
            // 澶勭悊楂樹寒鏁堟灉
            if (isHighlighted)
            {
                highlightTimer -= Time.deltaTime;
                if (highlightTimer <= 0)
                {
                    ResetBackgroundColor();
                }
            }
            
            // 鑷姩鏇存柊鐐瑰嚮娆℃暟锛堝鏋渢apCatController瀛樺湪锛?
            if (tapCatController != null)
            {
                UpdateTapCount(tapCatController.GetTapCount());
            }
        }
        
        /// <summary>
        /// 鍒濆鍖朥I
        /// </summary>
        private void InitializeUI()
        {
            // 纭繚UI鍏冪礌瀛樺湪
            if (tapCountText == null || statusText == null)
            {
                Debug.LogWarning("CounterUI: Some UI references missing; attempting auto-find.");
                FindUIElements();
            }
            
            // 璁剧疆鍒濆棰滆壊
            if (backgroundPanel != null)
            {
                backgroundPanel.color = normalColor;
            }
        }
        
        /// <summary>
        /// 鏌ユ壘UI鍏冪礌
        /// </summary>
        private void FindUIElements()
        {
            // 鍦ㄥ瓙瀵硅薄涓煡鎵?
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
            
            // 鏌ユ壘鑳屾櫙闈㈡澘
            if (backgroundPanel == null)
            {
                backgroundPanel = GetComponentInChildren<Image>();
            }
        }
        
        /// <summary>
        /// 鏇存柊鐐瑰嚮娆℃暟鏄剧ず
        /// </summary>
        public void UpdateTapCount(int count)
        {
            if (tapCountText != null)
            {
                tapCountText.text = string.Format(tapCountFormat, count);
                
                // 褰撶偣鍑绘鏁板鍔犳椂楂樹寒
                if (count > 0)
                {
                    HighlightBackground();
                }
            }
        }
        
        /// <summary>
        /// 鏇存柊鐘舵€佹樉绀?
        /// </summary>
        public void UpdateStatus(string status)
        {
            if (statusText != null)
            {
                statusText.text = string.Format(statusFormat, status);
            }
        }
        
        /// <summary>
        /// 楂樹寒鑳屾櫙
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
        /// 閲嶇疆鑳屾櫙棰滆壊
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
        /// 鏄剧ず/闅愯棌UI
        /// </summary>
        public void SetUIVisible(bool visible)
        {
            if (uiContainer != null)
            {
                uiContainer.SetActive(visible);
            }
        }
        
        /// <summary>
        /// 鍒囨崲UI鏄剧ず
        /// </summary>
        public void ToggleUI()
        {
            if (uiContainer != null)
            {
                bool newState = !uiContainer.activeSelf;
                uiContainer.SetActive(newState);
                Debug.Log($"UI鏄剧ず鐘舵€? {newState}");
            }
        }
        
        /// <summary>
        /// 璁剧疆UI閫忔槑搴?
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
        /// 鑾峰彇褰撳墠鐐瑰嚮娆℃暟
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
