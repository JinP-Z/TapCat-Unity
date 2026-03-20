using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TapCat
{
    /// <summary>
    /// 绠€鍗曞満鏅缃剼鏈?
    /// 鐩存帴鍦ㄥ満鏅腑杩愯锛屼笉闇€瑕佺紪杈戝櫒鑿滃崟
    /// </summary>
    public class SimpleSceneSetup : MonoBehaviour
    {
        [Header("璁剧疆閫夐」")]
        [SerializeField] private bool autoSetupOnStart = true;
        [SerializeField] private bool createPlaceholderSprite = true;
        
        [Header("鐚挭璁剧疆")]
        [SerializeField] private Vector3 catPosition = Vector3.zero;
        [SerializeField] private Color catColor = new Color(1, 0.5f, 0, 1); // 姗欒壊
        
        [Header("UI璁剧疆")]
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
        /// 璁剧疆鍦烘櫙
        /// </summary>
        public void SetupScene()
        {
            Debug.Log("寮€濮嬭缃甌apCat鍦烘櫙...");
            
            // 1. 鍒涘缓TapCat瀵硅薄
            GameObject tapCat = CreateTapCatObject();
            
            // 2. 鍒涘缓UI
            CreateUI();
            
            // 3. 鍒涘缓鍗犱綅绮剧伒锛堝鏋滈渶瑕侊級
            if (createPlaceholderSprite)
            {
                CreatePlaceholderSprite();
            }
            
            Debug.Log("Scene setup complete.");
            Debug.Log("Press Space or click the cat to test.");
        }
        
        private GameObject CreateTapCatObject()
        {
            // 鏌ユ壘鎴栧垱寤篢apCat瀵硅薄
            GameObject tapCat = GameObject.Find("TapCat");
            if (tapCat == null)
            {
                tapCat = new GameObject("TapCat");
                tapCat.transform.position = catPosition;
                
                // 娣诲姞SpriteRenderer
                SpriteRenderer spriteRenderer = tapCat.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 1;
                spriteRenderer.color = catColor;
                
                // 娣诲姞Animator
                Animator animator = tapCat.AddComponent<Animator>();
                
                // 娣诲姞鑴氭湰缁勪欢
                tapCat.AddComponent<TapCatController>();
                tapCat.AddComponent<AnimationManager>();
                tapCat.AddComponent<InputHandler>();
                
                Debug.Log("鍒涘缓TapCat娓告垙瀵硅薄");
            }
            else
            {
                Debug.Log("鎵惧埌宸插瓨鍦ㄧ殑TapCat瀵硅薄");
            }
            
            return tapCat;
        }
        
        private void CreateUI()
        {
            // 鍒涘缓Canvas
            GameObject canvasObj = new GameObject("Canvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
            
            // 鍒涘缓UI瀹瑰櫒
            GameObject uiContainer = new GameObject("CounterUI");
            uiContainer.transform.SetParent(canvas.transform);
            
            RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            rectTransform.anchoredPosition = uiPosition;
            rectTransform.sizeDelta = uiSize;
            
            // 娣诲姞鑳屾櫙
            GameObject background = new GameObject("Background");
            background.transform.SetParent(uiContainer.transform);
            RectTransform bgRect = background.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.sizeDelta = Vector2.zero;
            
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            
            // 娣诲姞鐐瑰嚮娆℃暟鏂囨湰
            GameObject countTextObj = new GameObject("TapCountText");
            countTextObj.transform.SetParent(uiContainer.transform);
            RectTransform countRect = countTextObj.AddComponent<RectTransform>();
            countRect.anchorMin = new Vector2(0, 0.5f);
            countRect.anchorMax = new Vector2(1, 0.8f);
            countRect.anchoredPosition = Vector2.zero;
            countRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI countText = countTextObj.AddComponent<TextMeshProUGUI>();
            countText.text = "鐐瑰嚮娆℃暟: 0";
            countText.fontSize = 24;
            countText.color = Color.white;
            countText.alignment = TextAlignmentOptions.Center;
            
            // 娣诲姞鐘舵€佹枃鏈?
            GameObject statusTextObj = new GameObject("StatusText");
            statusTextObj.transform.SetParent(uiContainer.transform);
            RectTransform statusRect = statusTextObj.AddComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0, 0.2f);
            statusRect.anchorMax = new Vector2(1, 0.5f);
            statusRect.anchoredPosition = Vector2.zero;
            statusRect.sizeDelta = Vector2.zero;
            
            TextMeshProUGUI statusText = statusTextObj.AddComponent<TextMeshProUGUI>();
            statusText.text = "鐘舵€? 绛夊緟杈撳叆...";
            statusText.fontSize = 18;
            statusText.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            statusText.alignment = TextAlignmentOptions.Center;
            
            // 娣诲姞CounterUI缁勪欢
            CounterUI counterUI = uiContainer.AddComponent<CounterUI>();
            
            Debug.Log("UI鍒涘缓瀹屾垚");
        }
        
        private void CreatePlaceholderSprite()
        {
            // 鍒涘缓涓€涓畝鍗曠殑2D鍦嗗舰浣滀负鍗犱綅绮剧伒
            GameObject placeholder = new GameObject("PlaceholderCat");
            placeholder.transform.position = catPosition;
            
            SpriteRenderer spriteRenderer = placeholder.AddComponent<SpriteRenderer>();
            spriteRenderer.color = catColor;
            spriteRenderer.sortingOrder = 0; // 鍦ㄧ尗鍜悗闈?
            
            // 鍒涘缓涓€涓畝鍗曠殑鍦嗗舰绮剧伒
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
            
            Debug.Log("鍗犱綅绮剧伒鍒涘缓瀹屾垚");
        }
        
        /// <summary>
        /// 鎵嬪姩璋冪敤璁剧疆鍦烘櫙
        /// </summary>
        [ContextMenu("鎵嬪姩璁剧疆鍦烘櫙")]
        private void ManualSetup()
        {
            SetupScene();
        }
        
        /// <summary>
        /// 閲嶇疆鍦烘櫙
        /// </summary>
        [ContextMenu("閲嶇疆鍦烘櫙")]
        private void ResetScene()
        {
            Debug.Log("閲嶇疆鍦烘櫙...");
            
            // 鍒犻櫎TapCat瀵硅薄
            GameObject tapCat = GameObject.Find("TapCat");
            if (tapCat != null)
            {
                DestroyImmediate(tapCat);
            }
            
            // 鍒犻櫎Canvas
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                DestroyImmediate(canvas);
            }
            
            // 鍒犻櫎鍗犱綅绮剧伒
            GameObject placeholder = GameObject.Find("PlaceholderCat");
            if (placeholder != null)
            {
                DestroyImmediate(placeholder);
            }
            
            Debug.Log("鍦烘櫙閲嶇疆瀹屾垚");
        }
    }
}
