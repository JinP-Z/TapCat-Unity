using UnityEngine;

namespace TapCat
{
    /// <summary>
    /// TapCat2D最终设置脚本
    /// 确保游戏在点击Play后立即运行，无需任何配置
    /// 这是用户需要添加到场景的唯一脚本
    /// </summary>
    public class TapCat2DFinalSetup : MonoBehaviour
    {
        [Header("自动设置选项")]
        [SerializeField] private bool autoCreateCat = true;
        [SerializeField] private bool autoCreateUI = true;
        [SerializeField] private bool autoSetupCamera = true;
        [SerializeField] private bool createPlaceholdersIfNeeded = true;
        
        [Header("猫咪设置")]
        [SerializeField] private Vector3 catPosition = Vector3.zero;
        [SerializeField] private float catScale = 1.0f;
        
        [Header("UI设置")]
        [SerializeField] private Color uiTextColor = Color.yellow;
        [SerializeField] private int titleFontSize = 32;
        [SerializeField] private int countFontSize = 24;
        
        private GameObject catObject;
        private GameObject uiCanvas;
        
        private void Start()
        {
            Debug.Log("=== TapCat2D 自动设置开始 ===");
            
            // 执行自动设置
            AutoSetupCompleteGame();
            
            Debug.Log("=== TapCat2D 自动设置完成 ===");
            Debug.Log("游戏已准备就绪！");
            Debug.Log("控制方式：");
            Debug.Log("- 空格键 或 鼠标左键：播放猫咪动画");
            Debug.Log("- R键：重置游戏");
            Debug.Log("- 帧率：0.1秒/帧 (10 FPS)");
        }
        
        /// <summary>
        /// 自动设置完整游戏
        /// </summary>
        private void AutoSetupCompleteGame()
        {
            // 1. 设置2D相机
            if (autoSetupCamera)
            {
                Setup2DCamera();
            }
            
            // 2. 创建猫咪对象
            if (autoCreateCat)
            {
                CreateCatObject();
            }
            
            // 3. 创建UI界面
            if (autoCreateUI)
            {
                CreateUInterface();
            }
            
            // 4. 连接所有组件
            ConnectAllComponents();
            
            // 5. 检查资源并创建占位图
            if (createPlaceholdersIfNeeded)
            {
                CheckResources();
            }
        }
        
        /// <summary>
        /// 设置2D相机
        /// </summary>
        private void Setup2DCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                GameObject cameraObj = new GameObject("MainCamera");
                cameraObj.tag = "MainCamera";
                mainCamera = cameraObj.AddComponent<Camera>();
                cameraObj.AddComponent<AudioListener>();
            }
            
            // 设置为2D正交相机
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5.0f;
            mainCamera.transform.position = new Vector3(0, 0, -10);
            mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.2f);
            
            Debug.Log("✓ 2D相机设置完成");
        }
        
        /// <summary>
        /// 创建猫咪对象
        /// </summary>
        private void CreateCatObject()
        {
            catObject = new GameObject("TapCat2D");
            catObject.transform.position = catPosition;
            catObject.transform.localScale = Vector3.one * catScale;
            
            // 添加SpriteRenderer
            SpriteRenderer spriteRenderer = catObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.sortingOrder = 0;
            
            // 添加TapCat2D主控制器
            TapCat2D tapCat2D = catObject.AddComponent<TapCat2D>();
            
            // 设置默认帧率（0.1秒/帧 = 10 FPS）
            tapCat2D.SetFrameRate(0.1f);
            
            Debug.Log("✓ 猫咪对象创建完成");
        }
        
        /// <summary>
        /// 创建UI界面
        /// </summary>
        private void CreateUInterface()
        {
            // 创建Canvas
            uiCanvas = new GameObject("TapCat2D_Canvas");
            Canvas canvas = uiCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiCanvas.AddComponent<CanvasScaler>();
            uiCanvas.AddComponent<GraphicRaycaster>();
            
            // 创建标题
            CreateTextElement("TitleText", "TapCat 2D 动画游戏", 
                new Vector2(0.5f, 0.9f), new Vector2(400, 50), 
                titleFontSize, TextAnchor.UpperCenter, Color.white);
            
            // 创建点击计数显示
            CreateTextElement("TapCountText", "点击次数: 0", 
                new Vector2(0.05f, 0.85f), new Vector2(300, 40), 
                countFontSize, TextAnchor.UpperLeft, uiTextColor);
            
            // 创建状态显示
            CreateTextElement("StatusText", "状态: 等待点击\n按空格键或鼠标左键播放动画\n按R键重置", 
                new Vector2(0.5f, 0.1f), new Vector2(600, 100), 
                20, TextAnchor.UpperCenter, Color.green);
            
            // 创建点击提示
            CreateTapHint();
            
            Debug.Log("✓ UI界面创建完成");
        }
        
        /// <summary>
        /// 创建文本元素
        /// </summary>
        private GameObject CreateTextElement(string name, string text, Vector2 anchor, Vector2 size, 
            int fontSize, TextAnchor alignment, Color color)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(uiCanvas.transform);
            
            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = text;
            textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            textComponent.fontSize = fontSize;
            textComponent.alignment = alignment;
            textComponent.color = color;
            
            RectTransform rect = textObj.GetComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = size;
            rect.anchoredPosition = Vector2.zero;
            
            return textObj;
        }
        
        /// <summary>
        /// 创建点击提示
        /// </summary>
        private void CreateTapHint()
        {
            GameObject hintObj = new GameObject("TapHintUI");
            hintObj.transform.SetParent(uiCanvas.transform);
            
            // 添加背景
            UnityEngine.UI.Image hintImage = hintObj.AddComponent<UnityEngine.UI.Image>();
            hintImage.color = new Color(1, 1, 1, 0.3f);
            
            // 添加提示文本
            GameObject hintTextObj = new GameObject("HintText");
            hintTextObj.transform.SetParent(hintObj.transform);
            
            Text hintText = hintTextObj.AddComponent<Text>();
            hintText.text = "点击这里！";
            hintText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            hintText.fontSize = 28;
            hintText.alignment = TextAnchor.MiddleCenter;
            hintText.color = Color.white;
            
            // 设置RectTransform
            RectTransform hintRect = hintObj.GetComponent<RectTransform>();
            hintRect.anchorMin = new Vector2(0.5f, 0.5f);
            hintRect.anchorMax = new Vector2(0.5f, 0.5f);
            hintRect.pivot = new Vector2(0.5f, 0.5f);
            hintRect.sizeDelta = new Vector2(200, 60);
            hintRect.anchoredPosition = Vector2.zero;
            
            RectTransform hintTextRect = hintTextObj.GetComponent<RectTransform>();
            hintTextRect.anchorMin = Vector2.zero;
            hintTextRect.anchorMax = Vector2.one;
            hintTextRect.sizeDelta = Vector2.zero;
            hintTextRect.anchoredPosition = Vector2.zero;
        }
        
        /// <summary>
        /// 连接所有组件
        /// </summary>
        private void ConnectAllComponents()
        {
            if (catObject == null || uiCanvas == null) return;
            
            TapCat2D tapCat2D = catObject.GetComponent<TapCat2D>();
            if (tapCat2D == null) return;
            
            // 查找UI元素
            Text tapCountText = null;
            Text statusText = null;
            GameObject tapHintUI = null;
            
            foreach (Transform child in uiCanvas.transform)
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
            
            // 连接UI到控制器
            if (tapCountText != null && statusText != null)
            {
                tapCat2D.SetUIRefs(tapCountText, statusText, tapHintUI);
                Debug.Log("✓ 组件连接完成");
            }
            else
            {
                Debug.LogWarning("⚠ 部分UI元素未找到，连接不完整");
            }
        }
        
        /// <summary>
        /// 检查资源并创建占位图
        /// </summary>
        private void CheckResources()
        {
            // 检查是否有CreatePlaceholderSprites脚本
            CreatePlaceholderSprites placeholderCreator = FindObjectOfType<CreatePlaceholderSprites>();
            if (placeholderCreator == null)
            {
                GameObject placeholderObj = new GameObject("PlaceholderCreator");
                placeholderCreator = placeholderObj.AddComponent<CreatePlaceholderSprites>();
            }
            
            // 检查资源
            placeholderCreator.CheckAndCreatePlaceholders();
        }
        
        /// <summary>
        /// 手动触发设置
        /// </summary>
        [ContextMenu("运行自动设置")]
        private void RunAutoSetupManual()
        {
            if (Application.isPlaying)
            {
                AutoSetupCompleteGame();
            }
            else
            {
                Debug.Log("自动设置将在播放模式下运行");
            }
        }
        
        /// <summary>
        /// 测试游戏功能
        /// </summary>
        [ContextMenu("测试游戏功能")]
        private void TestGameFunction()
        {
            if (!Application.isPlaying)
            {
                Debug.Log("请在播放模式下测试");
                return;
            }
            
            if (catObject == null)
            {
                Debug.LogError("猫咪对象未创建");
                return;
            }
            
            TapCat2D tapCat2D = catObject.GetComponent<TapCat2D>();
            if (tapCat2D != null)
            {
                // 测试动画播放
                tapCat2D.StartCatAnimation();
                Debug.Log("测试：开始播放动画");
                
                // 稍后测试重置
                Invoke("TestResetFunction", 1.5f);
            }
        }
        
        /// <summary>
        /// 测试重置功能
        /// </summary>
        private void TestResetFunction()
        {
            if (catObject == null) return;
            
            TapCat2D tapCat2D = catObject.GetComponent<TapCat2D>();
            if (tapCat2D != null)
            {
                tapCat2D.ResetGame();
                Debug.Log("测试：重置游戏");
            }
        }
        
        /// <summary>
        /// 显示控制说明
        /// </summary>
        [ContextMenu("显示控制说明")]
        private void ShowControlInstructions()
        {
            string instructions = @"
            TapCat 2D 控制说明
            =================
            
            基本控制：
            ---------
            • 播放动画：空格键 或 鼠标左键
            • 重置游戏：R键
            • 帧率：0.1秒/帧 (10 FPS)
            
            游戏功能：
            ---------
            • 每次点击播放完整10帧动画
            • 实时显示点击次数
            • 动画播放期间不接受新输入
            • 重置功能清空计数和状态
            
            技术规格：
            ---------
            • 纯2D Sprite系统
            • 无3D元素，无旋转
            • 序列帧动画播放
            • 自动UI生成
            
            资源要求：
            ---------
            • 10帧猫咪PNG图片
            • 命名：cat_anim_00.png 到 cat_anim_09.png
            • 位置：Assets/Sprites/CatAnimation/
            
            用户操作：
            ---------
            • 零配置：点击Play直接运行
            • 自动设置：场景自动配置
            • 占位图：缺少资源时自动生成
            
            验证方法：
            ---------
            1. 点击Play按钮
            2. 按空格键测试动画
            3. 按R键测试重置
            4. 检查UI显示是否正常
            ";
            
            Debug.Log(instructions);
        }
        
        /// <summary>
        /// 清理场景对象
        /// </summary>
        private void OnDestroy()
        {
            // 清理创建的对象
            if (!Application.isPlaying) return;
            
            if (catObject != null) Destroy(catObject);
            if (uiCanvas != null) Destroy(uiCanvas);
        }
    }
}