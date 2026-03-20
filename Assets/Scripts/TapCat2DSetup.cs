using UnityEngine;
using UnityEngine.UI;

namespace TapCat
{
    /// <summary>
    /// TapCat2D自动设置脚本
    /// 确保游戏在点击Play后立即运行，无需任何配置
    /// </summary>
    public class TapCat2DSetup : MonoBehaviour
    {
        [Header("预制件引用")]
        [SerializeField] private GameObject tapCat2DPrefab;
        [SerializeField] private GameObject uiCanvasPrefab;
        
        [Header("猫咪动画帧")]
        [SerializeField] private Sprite[] catAnimationSprites;
        
        [Header("生成位置")]
        [SerializeField] private Vector3 catPosition = new Vector3(0, 0, 0);
        
        private GameObject currentCat;
        private GameObject currentCanvas;
        
        /// <summary>
        /// 游戏开始时自动设置
        /// </summary>
        private void Start()
        {
            SetupGame();
        }
        
        /// <summary>
        /// 设置完整游戏环境
        /// </summary>
        private void SetupGame()
        {
            // 1. 创建UI画布
            CreateUICanvas();
            
            // 2. 创建猫咪对象
            CreateCatObject();
            
            // 3. 连接UI和猫咪控制器
            ConnectUIAndCat();
            
            // 4. 设置相机
            SetupCamera();
            
            Debug.Log("TapCat 2D游戏设置完成！点击空格键或鼠标左键播放动画，按R键重置");
        }
        
        /// <summary>
        /// 创建UI画布
        /// </summary>
        private void CreateUICanvas()
        {
            if (uiCanvasPrefab != null)
            {
                currentCanvas = Instantiate(uiCanvasPrefab);
                currentCanvas.name = "TapCat2D_UI";
            }
            else
            {
                // 动态创建UI
                currentCanvas = new GameObject("TapCat2D_UI");
                Canvas canvas = currentCanvas.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                currentCanvas.AddComponent<CanvasScaler>();
                currentCanvas.AddComponent<GraphicRaycaster>();
                
                // 创建UI元素
                CreateUIElements();
            }
        }
        
        /// <summary>
        /// 动态创建UI元素
        /// </summary>
        private void CreateUIElements()
        {
            // 创建标题
            GameObject titleObj = new GameObject("TitleText");
            titleObj.transform.SetParent(currentCanvas.transform);
            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = "TapCat 2D 动画游戏";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 32;
            titleText.alignment = TextAnchor.UpperCenter;
            titleText.color = Color.white;
            
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.9f);
            titleRect.anchorMax = new Vector2(0.5f, 0.9f);
            titleRect.pivot = new Vector2(0.5f, 0.5f);
            titleRect.sizeDelta = new Vector2(400, 50);
            titleRect.anchoredPosition = Vector2.zero;
            
            // 创建点击计数文本
            GameObject countObj = new GameObject("TapCountText");
            countObj.transform.SetParent(currentCanvas.transform);
            Text countText = countObj.AddComponent<Text>();
            countText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            countText.fontSize = 24;
            countText.alignment = TextAnchor.UpperLeft;
            countText.color = Color.yellow;
            
            RectTransform countRect = countObj.GetComponent<RectTransform>();
            countRect.anchorMin = new Vector2(0.05f, 0.85f);
            countRect.anchorMax = new Vector2(0.05f, 0.85f);
            countRect.pivot = new Vector2(0, 0.5f);
            countRect.sizeDelta = new Vector2(300, 40);
            countRect.anchoredPosition = Vector2.zero;
            
            // 创建状态文本
            GameObject statusObj = new GameObject("StatusText");
            statusObj.transform.SetParent(currentCanvas.transform);
            Text statusText = statusObj.AddComponent<Text>();
            statusText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            statusText.fontSize = 20;
            statusText.alignment = TextAnchor.UpperCenter;
            statusText.color = Color.green;
            
            RectTransform statusRect = statusObj.GetComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0.5f, 0.1f);
            statusRect.anchorMax = new Vector2(0.5f, 0.1f);
            statusRect.pivot = new Vector2(0.5f, 0.5f);
            statusRect.sizeDelta = new Vector2(600, 100);
            statusRect.anchoredPosition = Vector2.zero;
            
            // 创建点击提示
            GameObject hintObj = new GameObject("TapHintUI");
            hintObj.transform.SetParent(currentCanvas.transform);
            Image hintImage = hintObj.AddComponent<Image>();
            hintImage.color = new Color(1, 1, 1, 0.3f);
            
            GameObject hintTextObj = new GameObject("HintText");
            hintTextObj.transform.SetParent(hintObj.transform);
            Text hintText = hintTextObj.AddComponent<Text>();
            hintText.text = "点击这里！";
            hintText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            hintText.fontSize = 28;
            hintText.alignment = TextAnchor.MiddleCenter;
            hintText.color = Color.white;
            
            RectTransform hintRect = hintObj.GetComponent<RectTransform>();
            hintRect.anchorMin = new Vector2(0.5f, 0.5f);
            hintRect.anchorMax = new Vector2(0.5f, 0.5f);
            hintRect.pivot = new Vector2(0.5f, 0.5f);
            hintRect.sizeDelta = new Vector2(200, 60);
            hintRect.anchoredPosition = Vector2.zero;
            
            RectTransform hintTextRect = hintTextObj.GetComponent<RectTransform>();
            hintTextRect.anchorMin = new Vector2(0, 0);
            hintTextRect.anchorMax = new Vector2(1, 1);
            hintTextRect.sizeDelta = Vector2.zero;
            hintTextRect.anchoredPosition = Vector2.zero;
        }
        
        /// <summary>
        /// 创建猫咪对象
        /// </summary>
        private void CreateCatObject()
        {
            if (tapCat2DPrefab != null)
            {
                currentCat = Instantiate(tapCat2DPrefab, catPosition, Quaternion.identity);
                currentCat.name = "TapCat2D";
            }
            else
            {
                // 动态创建猫咪对象
                currentCat = new GameObject("TapCat2D");
                currentCat.transform.position = catPosition;
                
                // 添加SpriteRenderer
                SpriteRenderer spriteRenderer = currentCat.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingLayerName = "Default";
                spriteRenderer.sortingOrder = 0;
                
                // 添加TapCat2D脚本
                TapCat2D tapCat2D = currentCat.AddComponent<TapCat2D>();
                
                // 设置动画帧
                if (catAnimationSprites != null && catAnimationSprites.Length > 0)
                {
                    tapCat2D.SetAnimationFrames(catAnimationSprites);
                }
            }
        }
        
        /// <summary>
        /// 连接UI和猫咪控制器
        /// </summary>
        private void ConnectUIAndCat()
        {
            if (currentCat == null || currentCanvas == null) return;
            
            TapCat2D tapCat2D = currentCat.GetComponent<TapCat2D>();
            if (tapCat2D == null) return;
            
            // 查找UI元素
            Text tapCountText = null;
            Text statusText = null;
            GameObject tapHintUI = null;
            
            foreach (Transform child in currentCanvas.transform)
            {
                if (child.name == "TapCountText")
                {
                    tapCountText = child.GetComponent<Text>();
                }
                else if (child.name == "StatusText")
                {
                    statusText = child.GetComponent<Text>();
                }
                else if (child.name == "TapHintUI")
                {
                    tapHintUI = child.gameObject;
                }
            }
            
            // 设置UI引用
            if (tapCat2D != null)
            {
                tapCat2D.SetUIRefs(tapCountText, statusText, tapHintUI);
            }
        }
        
        /// <summary>
        /// 设置相机
        /// </summary>
        private void SetupCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                GameObject cameraObj = new GameObject("MainCamera");
                mainCamera = cameraObj.AddComponent<Camera>();
                cameraObj.AddComponent<AudioListener>();
                cameraObj.tag = "MainCamera";
            }
            
            // 设置2D相机
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5;
            mainCamera.transform.position = new Vector3(0, 0, -10);
            mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.2f);
        }
        
        /// <summary>
        /// 清理游戏对象
        /// </summary>
        private void OnDestroy()
        {
            if (currentCat != null) Destroy(currentCat);
            if (currentCanvas != null) Destroy(currentCanvas);
        }
        
        /// <summary>
        /// 在编辑器中测试设置
        /// </summary>
        [ContextMenu("测试游戏设置")]
        private void TestSetup()
        {
            if (Application.isPlaying)
            {
                SetupGame();
            }
            else
            {
                Debug.Log("请在播放模式下测试设置");
            }
        }
    }
}