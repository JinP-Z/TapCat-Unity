using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TapCat.Animation;

namespace TapCat
{
    /// <summary>
    /// 鐚挭鏄剧ず淇宸ュ叿
    /// 纭繚鐚挭涓€瀹氭樉绀哄湪灞忓箷涓?
    /// </summary>
    public class CatFixer : MonoBehaviour
    {
        [Header("鐚挭璁剧疆")]
        [SerializeField] private Color catColor = new Color(1f, 0.5f, 0f, 1f); // 姗欒壊
        [SerializeField] private Vector3 catPosition = new Vector3(0f, 0f, 0f);
        [SerializeField] private Vector3 catScale = new Vector3(1f, 1f, 1f);
        
        [Header("UI璁剧疆")]
        [SerializeField] private bool createUI = true;
        [SerializeField] private Vector2 uiPosition = new Vector2(-20f, -20f);
        
        [Header("璋冭瘯")]
        [SerializeField] private bool debugMode = false;
        
        private GameObject tapCatObject;
        
        void Start()
        {
            FixCatDisplay();
        }
        
        /// <summary>
        /// 淇鐚挭鏄剧ず
        /// </summary>
        public void FixCatDisplay()
        {
            Debug.Log("寮€濮嬩慨澶嶇尗鍜樉绀?..");
            
            // 1. 纭繚TapCat瀵硅薄瀛樺湪
            EnsureTapCatObject();
            
            // 2. 纭繚SpriteRenderer瀛樺湪骞惰缃?
            EnsureSpriteRenderer();
            
            // 3. 纭繚浣嶇疆姝ｇ‘
            EnsurePosition();
            
            // 4. 鍒涘缓UI锛堝鏋滈渶瑕侊級
            if (createUI)
            {
                EnsureUI();
            }
            
            Debug.Log("Cat display fix complete.");
            
            if (debugMode)
            {
                LogDebugInfo();
            }
        }
        
        /// <summary>
        /// 纭繚TapCat瀵硅薄瀛樺湪
        /// </summary>
        private void EnsureTapCatObject()
        {
            tapCatObject = GameObject.Find("TapCat");
            
            if (tapCatObject == null)
            {
                Debug.Log("鍒涘缓TapCat瀵硅薄");
                tapCatObject = new GameObject("TapCat");
                tapCatObject.transform.position = catPosition;
                tapCatObject.transform.localScale = catScale;
                
                // 娣诲姞蹇呰缁勪欢
                tapCatObject.AddComponent<TapCatController>();
                tapCatObject.AddComponent<AnimationManager>();
                tapCatObject.AddComponent<InputHandler>();
                tapCatObject.AddComponent<Animator>();
            }
            else
            {
                Debug.Log("鎵惧埌宸插瓨鍦ㄧ殑TapCat瀵硅薄");
            }
        }
        
        /// <summary>
        /// 纭繚SpriteRenderer瀛樺湪骞惰缃?
        /// </summary>
        private void EnsureSpriteRenderer()
        {
            SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
            
            if (spriteRenderer == null)
            {
                Debug.Log("娣诲姞SpriteRenderer缁勪欢");
                spriteRenderer = tapCatObject.AddComponent<SpriteRenderer>();
            }
            
            // 灏濊瘯鍔犺浇鐚挭鍥剧墖
            bool spriteLoaded = false;
            
            // 鏂规硶1锛氫粠Resources鍔犺浇
            Sprite catSprite = Resources.Load<Sprite>("Sprites/PlaceholderCat");
            if (catSprite != null)
            {
                spriteRenderer.sprite = catSprite;
                spriteLoaded = true;
                Debug.Log("鎴愬姛鍔犺浇鐚挭鍥剧墖");
            }
            else
            {
                Debug.LogWarning("鐚挭鍥剧墖鏈壘鍒帮紝浣跨敤褰╄壊鏂瑰潡");
            }
            
            // 濡傛灉鍥剧墖鏈姞杞斤紝璁剧疆棰滆壊
            if (!spriteLoaded)
            {
                spriteRenderer.color = catColor;
            }
            
            // 纭繚鎺掑簭姝ｇ‘
            spriteRenderer.sortingOrder = 0;
        }
        
        /// <summary>
        /// 纭繚浣嶇疆姝ｇ‘
        /// </summary>
        private void EnsurePosition()
        {
            tapCatObject.transform.position = catPosition;
            tapCatObject.transform.localScale = catScale;
            
            Debug.Log($"璁剧疆鐚挭浣嶇疆: {catPosition}, 缂╂斁: {catScale}");
        }
        
        /// <summary>
        /// 纭繚UI瀛樺湪
        /// </summary>
        private void EnsureUI()
        {
            // 鏌ユ壘鎴栧垱寤篊anvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                Debug.Log("鍒涘缓Canvas");
            }
            
            // 妫€鏌ounterUI鏄惁瀛樺湪
            GameObject counterUI = GameObject.Find("CounterUI");
            if (counterUI == null)
            {
                CreateSimpleUI(canvas);
            }
        }
        
        /// <summary>
        /// 鍒涘缓绠€鍗昒I
        /// </summary>
        private void CreateSimpleUI(Canvas canvas)
        {
            // 鍒涘缓UI瀹瑰櫒
            GameObject uiContainer = new GameObject("CounterUI");
            uiContainer.transform.SetParent(canvas.transform);
            
            // 娣诲姞RectTransform
            RectTransform rectTransform = uiContainer.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(1, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(1, 1);
            rectTransform.anchoredPosition = uiPosition;
            rectTransform.sizeDelta = new Vector2(200, 80);
            
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
            
            Debug.Log("鍒涘缓绠€鍗昒I");
        }
        
        /// <summary>
        /// 璁板綍璋冭瘯淇℃伅
        /// </summary>
        private void LogDebugInfo()
        {
            Debug.Log("=== 鐚挭鏄剧ず璋冭瘯淇℃伅 ===");
            Debug.Log($"瀵硅薄鍚嶇О: {tapCatObject.name}");
            Debug.Log($"浣嶇疆: {tapCatObject.transform.position}");
            Debug.Log($"缂╂斁: {tapCatObject.transform.localScale}");
            
            SpriteRenderer spriteRenderer = tapCatObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Debug.Log("SpriteRenderer present: yes");
                Debug.Log($"Sprite: {(spriteRenderer.sprite != null ? spriteRenderer.sprite.name : "None")}");
                Debug.Log($"棰滆壊: {spriteRenderer.color}");
            }
            else
            {
                Debug.LogError("SpriteRenderer涓嶅瓨鍦紒");
            }
            
            Debug.Log("======================");
        }
        
        /// <summary>
        /// 缂栬緫鍣ㄥ伐鍏凤細蹇€熶慨澶?
        /// </summary>
        [ContextMenu("Quick Fix Cat Display")]
        private void QuickFix()
        {
            FixCatDisplay();
        }
    }
}
