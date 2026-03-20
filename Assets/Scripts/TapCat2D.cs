using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TapCat
{
    /// <summary>
    /// TapCat 2D动画主控制器
    /// 完全遵守2D游戏规范，使用Sprite序列帧动画
    /// </summary>
    public class TapCat2D : MonoBehaviour
    {
        [Header("Sprite动画设置")]
        [SerializeField] private Sprite[] catAnimationFrames; // 猫咪序列帧 (cat_anim_00.png 到 cat_anim_09.png)
        [SerializeField] private float frameRate = 0.1f; // 0.1秒/帧 (10 FPS)
        [SerializeField] private SpriteRenderer catSpriteRenderer;
        
        [Header("UI显示")]
        [SerializeField] private Text tapCountText;
        [SerializeField] private Text statusText;
        [SerializeField] private GameObject tapHintUI;
        
        [Header("调试设置")]
        [SerializeField] private bool showDebugLogs = true;
        
        [Header("游戏状态")]
        [SerializeField] private int tapCount = 0;
        [SerializeField] private bool isAnimating = false;
        [SerializeField] private bool isInitialized = false;
        
        // 动画相关变量
        private Coroutine animationCoroutine;
        private int currentFrameIndex = 0;
        
        // 输入设置
        private KeyCode tapKey = KeyCode.Space;
        private KeyCode resetKey = KeyCode.R;
        
        /// <summary>
        /// 初始化组件
        /// </summary>
        private void Start()
        {
            InitializeComponents();
            SetupUI();
            UpdateStatusDisplay();
        }
        
        /// <summary>
        /// 初始化所有必要组件
        /// </summary>
        private void InitializeComponents()
        {
            // 确保有SpriteRenderer组件
            if (catSpriteRenderer == null)
            {
                catSpriteRenderer = GetComponent<SpriteRenderer>();
                if (catSpriteRenderer == null)
                {
                    catSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    Debug.Log("已添加SpriteRenderer组件");
                }
            }
            
            // 检查动画帧数组
            if (catAnimationFrames == null || catAnimationFrames.Length == 0)
            {
                Debug.LogWarning("猫咪动画帧数组为空！请将cat_anim_00.png到cat_anim_09.png拖入数组");
            }
            else if (catAnimationFrames.Length < 10)
            {
                Debug.LogWarning($"猫咪动画帧不足10帧，当前只有{catAnimationFrames.Length}帧");
            }
            else
            {
                Debug.Log($"已加载{catAnimationFrames.Length}帧猫咪动画");
            }
            
            isInitialized = true;
        }
        
        /// <summary>
        /// 设置UI元素
        /// </summary>
        private void SetupUI()
        {
            // 如果UI元素未在Inspector中设置，尝试查找
            if (tapCountText == null)
            {
                GameObject tapCountObj = GameObject.Find("TapCountText");
                if (tapCountObj != null) tapCountText = tapCountObj.GetComponent<Text>();
            }
            
            if (statusText == null)
            {
                GameObject statusObj = GameObject.Find("StatusText");
                if (statusObj != null) statusText = statusObj.GetComponent<Text>();
            }
            
            if (tapHintUI == null)
            {
                tapHintUI = GameObject.Find("TapHintUI");
            }
            
            UpdateTapCountDisplay();
        }
        
        /// <summary>
        /// 每帧更新输入检测
        /// </summary>
        private void Update()
        {
            if (!isInitialized) return;
            
            // 检测空格键点击
            if (Input.GetKeyDown(tapKey) && !isAnimating)
            {
                StartCatAnimation();
            }
            
            // 检测鼠标左键点击
            if (Input.GetMouseButtonDown(0) && !isAnimating)
            {
                StartCatAnimation();
            }
            
            // 检测R键重置
            if (Input.GetKeyDown(resetKey))
            {
                ResetGame();
            }
        }
        
        /// <summary>
        /// 开始播放猫咪动画
        /// </summary>
        public void StartCatAnimation()
        {
            if (!isInitialized || isAnimating) return;
            
            // 增加点击计数
            tapCount++;
            UpdateTapCountDisplay();
            
            // 开始动画
            isAnimating = true;
            
            // 停止之前的动画协程（如果有）
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }
            
            // 开始新的动画协程
            animationCoroutine = StartCoroutine(PlayCatAnimation());
            
            // 更新状态显示
            UpdateStatusDisplay();
            
            if (showDebugLogs)
                Debug.Log($"开始播放猫咪动画！点击次数: {tapCount}");
        }
        
        /// <summary>
        /// 播放猫咪序列帧动画协程
        /// </summary>
        private IEnumerator PlayCatAnimation()
        {
            if (catAnimationFrames == null || catAnimationFrames.Length == 0)
            {
                Debug.LogError("无法播放动画：动画帧数组为空");
                isAnimating = false;
                yield break;
            }
            
            // 播放完整动画序列（10帧）
            for (int i = 0; i < catAnimationFrames.Length; i++)
            {
                currentFrameIndex = i;
                catSpriteRenderer.sprite = catAnimationFrames[i];
                
                // 等待帧率时间
                yield return new WaitForSeconds(frameRate);
            }
            
            // 动画播放完成
            isAnimating = false;
            UpdateStatusDisplay();
            
            if (showDebugLogs)
                Debug.Log("猫咪动画播放完成");
        }
        
        /// <summary>
        /// 重置游戏状态
        /// </summary>
        public void ResetGame()
        {
            // 停止动画
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }
            
            // 重置状态
            isAnimating = false;
            tapCount = 0;
            currentFrameIndex = 0;
            
            // 重置Sprite显示
            if (catAnimationFrames != null && catAnimationFrames.Length > 0)
            {
                catSpriteRenderer.sprite = catAnimationFrames[0];
            }
            
            // 更新UI
            UpdateTapCountDisplay();
            UpdateStatusDisplay();
            
            if (showDebugLogs)
                Debug.Log("游戏已重置");
        }
        
        /// <summary>
        /// 更新点击计数显示
        /// </summary>
        private void UpdateTapCountDisplay()
        {
            if (tapCountText != null)
            {
                tapCountText.text = $"点击次数: {tapCount}";
            }
        }
        
        /// <summary>
        /// 更新状态显示
        /// </summary>
        private void UpdateStatusDisplay()
        {
            if (statusText != null)
            {
                string status = isAnimating ? "播放中..." : "等待点击";
                statusText.text = $"状态: {status}\n按空格键或鼠标左键播放动画\n按R键重置";
            }
            
            // 显示/隐藏点击提示
            if (tapHintUI != null)
            {
                tapHintUI.SetActive(!isAnimating);
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
        /// 检查是否正在播放动画
        /// </summary>
        public bool IsAnimating()
        {
            return isAnimating;
        }
        
        /// <summary>
        /// 设置动画帧率
        /// </summary>
        public void SetFrameRate(float newFrameRate)
        {
            frameRate = Mathf.Clamp(newFrameRate, 0.01f, 1.0f);
            Debug.Log($"帧率已设置为: {frameRate}秒/帧 ({(int)(1.0f / frameRate)} FPS)");
        }
        
        /// <summary>
        /// 手动设置动画帧数组（供编辑器使用）
        /// </summary>
        public void SetAnimationFrames(Sprite[] frames)
        {
            catAnimationFrames = frames;
            if (frames != null && frames.Length > 0 && catSpriteRenderer != null && !isAnimating)
            {
                catSpriteRenderer.sprite = frames[0];
            }
        }
        
        /// <summary>
        /// 设置UI引用（供自动设置使用）
        /// </summary>
        public void SetUIRefs(Text tapCountTextRef, Text statusTextRef, GameObject tapHintUIRef)
        {
            tapCountText = tapCountTextRef;
            statusText = statusTextRef;
            tapHintUI = tapHintUIRef;
            
            UpdateTapCountDisplay();
            UpdateStatusDisplay();
        }
        
        /// <summary>
        /// 在编辑器中测试动画
        /// </summary>
        [ContextMenu("测试动画播放")]
        private void TestAnimation()
        {
            if (Application.isPlaying)
            {
                StartCatAnimation();
            }
            else
            {
                Debug.Log("请在播放模式下测试动画");
            }
        }
        
        /// <summary>
        /// 在编辑器中测试重置
        /// </summary>
        [ContextMenu("测试重置")]
        private void TestReset()
        {
            if (Application.isPlaying)
            {
                ResetGame();
            }
            else
            {
                Debug.Log("请在播放模式下测试重置");
            }
        }
    }
}