using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TapCat
{
    /// <summary>
    /// 猫咪显示修复工具
    /// 确保猫咪一定显示在屏幕上
    /// </summary>
    public class CatFixer : MonoBehaviour
    {
        [Header("猫咪设置")]
        [SerializeField] private Color catColor = new Color(1f, 0.5f, 0f, 1f); // 橙色
        [SerializeField] private Vector3 catPosition = new Vector3(0f, 0f, 0f);
        [SerializeField] private Vector3 catScale = new Vector3(1f, 1f, 1f);
        
        [Header("UI设置")]
        [SerializeField] private bool createUI = true;
        [SerializeField] private Vector2 uiPosition = new Vector2(-20f, -20f);
        
        [Header("调试")]
        [SerializeField] private bool debugMode = false;
        
        private GameObject tapCatObject;
        
        void Start()
        {
            FixCatDisplay();
        }
        
        /// <summary>
        /// 修复猫咪显示
        /// </summary>
        public void FixCatDisplay()
        {
            Debug.Log("开始修复猫咪显示...");
            
            // 1. 确保TapCat对象存在
            EnsureTapCatObject();
            
            // 2. 确保SpriteRenderer存在并设置
            EnsureSpriteRenderer();
            
            // 3. 确保位置正确
            EnsurePosition();
            
            // 4. 创建UI（如果需要）
            if (createUI)
            {
                EnsureUI();
            }
            
            Debug.Log("猫咪显示修复完成！");
            
            if (debugMode)
            {
                LogDebugInfo();
            }
        }
        
        /// <summary>
        /// 确保TapCat对象存在
        /// </summary>
        private void EnsureTapCatObject()
        {
            tapCatObject = GameObject.Find("TapCat");
            
            if (tapCatObject == null)
            {
                Debug.Log("创建TapCat对象");
                tapCatObject = new GameObject("TapCat");
                tapCatObject.transform.position = catPosition;
                tapCatObject.transform.localScale = catScale;
                
                // 添加必要组件
                tapCatObject.AddComponent<TapCatController>();
                tapCatObject.AddComponent<AnimationManager>();
                tapCatObject.AddComponent<InputHandler>();
                tapCatObject.AddComponent<Animator>();
            }
            else
            {
                Debug.Log("找到已存在的TapCat对象");
            }
        }
        
        /// <summary>
        /// 确保SpriteRenderer存在并设置
        /// </summary>
        private void EnsureSpriteRenderer()
        {
            SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
            
            if (spriteRenderer == null)
            {
                Debug.Log("添加SpriteRenderer组件");
                spriteRenderer = tapCatObject.AddComponent<SpriteRenderer>();
            }
            
            // 尝试加载猫咪图片
            bool spriteLoaded = false;
            
            // 方法1：从Resources加载
            Sprite catSprite = Resources.Load<Sprite>("Sprites/PlaceholderCat");
            if (catSprite != null)
            {
                spriteRenderer.sprite = catSprite;
                spriteLoaded = true;
                Debug.Log("成功加载猫咪图片");
            }
            else
            {
                Debug.LogWarning("猫咪图片未找到，使用彩色方块");
            }
            
            // 如果图片未加载，设置颜色
            if (!spriteLoaded)
            {
                spriteRenderer.color = catColor;
            }
            
            // 确保排序正确
            spriteRenderer.sortingOrder = 0;
        }
        
        /// <summary>
        /// 确保位置正确
        /// </summary>
        private void EnsurePosition()
        {
            tapCatObject.transform.position = catPosition;
            tapCatObject.transform.localScale = catScale;
            
            Debug.Log($"设置猫咪位置: {catPosition}, 缩放: {catScale}");
        }
        
        /// <summary>
        /// 确保UI存在
        /// </summary>
        private void EnsureUI()
        {
            // 查找或创建Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                Debug.Log("创建Canvas");
            }
            
            // 检查CounterUI是否存在
            GameObject counterUI = GameObject.Find("CounterUI");
            if (counterUI == null)
            {
                CreateSimpleUI(canvas);
            }
        }
        
        /// <summary>
        /// 创建简单UI
        /// </summary>
        private void CreateSimpleUI(Canvas canvas)
        {
            // 创建UI容器
            GameObject uiContainer = new GameObject("CounterUI");
            uiContainer.transform.SetParent(canvas.transform);
            
            // 添加RectTransform
            RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            rectTransform.anchoredPosition = uiPosition;
            rectTransform.sizeDelta = new Vector2(200, 80);
            
            // 添加背景
            GameObject background = new GameObject("Background");
            background.transform.SetParent(uiContainer.transform);
            RectTransform bgRect = background.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            
            // 添加点击次数文本
            GameObject countTextObj = new GameObject("TapCountText");
            countTextObj.transform.SetParent(uiContainer.transform);
            RectTransform countRect = countTextObj.AddComponent<RectTransform>();
            countRect.anchorMin = new Vector2(0, 0.5f);
            countRect.anchorMax = new Vector2(1, 0.8f);
            countRect.anchoredPosition = Vector2.zero;
            countRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI countText = countTextObj.AddComponent<TextMeshProUGUI>();
            countText.text = "点击次数: 0";
            countText.fontSize = 24;
            countText.color = Color.white;
            countText.alignment = TextAlignmentOptions.Center;
            
            Debug.Log("创建简单UI");
        }
        
        /// <summary>
        /// 记录调试信息
        /// </summary>
        private void LogDebugInfo()
        {
            Debug.Log("=== 猫咪显示调试信息 ===");
            Debug.Log($"对象名称: {tapCatObject.name}");
            Debug.Log($"位置: {tapCatObject.transform.position}");
            Debug.Log($"缩放: {tapCatObject.transform.localScale}");
            
            SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Debug.Log($"SpriteRenderer存在: 是");
                Debug.Log($"Sprite: {spriteRenderer.sprite?.name ?? "无"}");
                Debug.Log($"颜色: {spriteRenderer.color}");
            }
            else
            {
                Debug.LogError("SpriteRenderer不存在！");
            }
            
            Debug.Log("======================");
        }
        
        /// <summary>
        /// 编辑器工具：快速修复
        /// </summary>
        [ContextMenu("快速修复猫咪显示")]
        private void QuickFix()
        {
            FixCatDisplay();
        }
    }
}