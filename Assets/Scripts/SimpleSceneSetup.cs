using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TapCat
{
    /// <summary>
    /// 简单场景设置脚本
    /// 直接在场景中运行，不需要编辑器菜单
    /// </summary>
    public class SimpleSceneSetup : MonoBehaviour
    {
        [Header("设置选项")]
        [SerializeField] private bool autoSetupOnStart = true;
        [SerializeField] private bool createPlaceholderSprite = true;
        
        [Header("猫咪设置")]
        [SerializeField] private Vector3 catPosition = Vector3.zero;
        [SerializeField] private Color catColor = new Color(1, 0.5f, 0, 1); // 橙色
        
        [Header("UI设置")]
        [SerializeField] private Vector2 uiPosition = new Vector2(-20, -20);
        [SerializeField] private Vector2 uiSize = new Vector2(200, 80);
        
        private void Start()
        {
            if (autoSetupOnStart)
            {
                SetupScene();
            }
        }
        
        /// <summary>
        /// 设置场景
        /// </summary>
        public void SetupScene()
        {
            Debug.Log("开始设置TapCat场景...");
            
            // 1. 创建TapCat对象
            GameObject tapCat = CreateTapCatObject();
            
            // 2. 创建UI
            CreateUI();
            
            // 3. 创建占位精灵（如果需要）
            if (createPlaceholderSprite)
            {
                CreatePlaceholderSprite();
            }
            
            Debug.Log("场景设置完成！");
            Debug.Log("按空格键或点击猫咪进行测试");
        }
        
        private GameObject CreateTapCatObject()
        {
            // 查找或创建TapCat对象
            GameObject tapCat = GameObject.Find("TapCat");
            if (tapCat == null)
            {
                tapCat = new GameObject("TapCat");
                tapCat.transform.position = catPosition;
                
                // 添加SpriteRenderer
                SpriteRenderer spriteRenderer = tapCat.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 1;
                spriteRenderer.color = catColor;
                
                // 添加Animator
                Animator animator = tapCat.AddComponent<Animator>();
                
                // 添加脚本组件
                tapCat.AddComponent<TapCatController>();
                tapCat.AddComponent<AnimationManager>();
                tapCat.AddComponent<InputHandler>();
                
                Debug.Log("创建TapCat游戏对象");
            }
            else
            {
                Debug.Log("找到已存在的TapCat对象");
            }
            
            return tapCat;
        }
        
        private void CreateUI()
        {
            // 创建Canvas
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // 创建UI容器
            GameObject uiContainer = new GameObject("CounterUI");
            uiContainer.transform.SetParent(canvas.transform);
            
            RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            rectTransform.anchoredPosition = uiPosition;
            rectTransform.sizeDelta = uiSize;
            
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
            
            // 添加状态文本
            GameObject statusTextObj = new GameObject("StatusText");
            statusTextObj.transform.SetParent(uiContainer.transform);
            RectTransform statusRect = statusTextObj.AddComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0, 0.2f);
            statusRect.anchorMax = new Vector2(1, 0.5f);
            statusRect.anchoredPosition = Vector2.zero;
            statusRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI statusText = statusTextObj.AddComponent<TextMeshProUGUI>();
            statusText.text = "状态: 等待输入...";
            statusText.fontSize = 18;
            statusText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            statusText.alignment = TextAlignmentOptions.Center;
            
            // 添加CounterUI组件
            CounterUI counterUI = uiContainer.AddComponent<CounterUI>();
            
            Debug.Log("UI创建完成");
        }
        
        private void CreatePlaceholderSprite()
        {
            // 创建一个简单的2D圆形作为占位精灵
            GameObject placeholder = new GameObject("PlaceholderCat");
            placeholder.transform.position = catPosition;
            
            SpriteRenderer spriteRenderer = placeholder.AddComponent<SpriteRenderer>();
            spriteRenderer.color = catColor;
            spriteRenderer.sortingOrder = 0; // 在猫咪后面
            
            // 创建一个简单的圆形精灵
            Texture2D texture = new Texture2D(64, 64);
            Color[] pixels = new Color[64 * 64];
            
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    float centerX = 32;
                    float centerY = 32;
                    float radius = 30;
                    
                    float distance = Mathf.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    
                    if (distance < radius)
                    {
                        pixels[y * 64 + x] = catColor;
                    }
                    else
                    {
                        pixels[y * 64 + x] = Color.clear;
                    }
                }
            }
            
            texture.SetPixels(pixels);
            texture.Apply();
            
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
            spriteRenderer.sprite = sprite;
            
            Debug.Log("占位精灵创建完成");
        }
        
        /// <summary>
        /// 手动调用设置场景
        /// </summary>
        [ContextMenu("手动设置场景")]
        private void ManualSetup()
        {
            SetupScene();
        }
        
        /// <summary>
        /// 重置场景
        /// </summary>
        [ContextMenu("重置场景")]
        private void ResetScene()
        {
            Debug.Log("重置场景...");
            
            // 删除TapCat对象
            GameObject tapCat = GameObject.Find("TapCat");
            if (tapCat != null)
            {
                DestroyImmediate(tapCat);
            }
            
            // 删除Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                DestroyImmediate(canvas);
            }
            
            // 删除占位精灵
            GameObject placeholder = GameObject.Find("PlaceholderCat");
            if (placeholder != null)
            {
                DestroyImmediate(placeholder);
            }
            
            Debug.Log("场景重置完成");
        }
    }
}