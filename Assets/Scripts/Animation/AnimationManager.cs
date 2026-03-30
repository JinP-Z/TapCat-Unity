using System;
using UnityEngine;
using TapCat.Input;

namespace TapCat.Animation
{
    /// <summary>
    /// 动画管理器，负责管理猫咪的序列帧动画播放
    /// 符合技术宪法第七章：动画系统规范
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        /// <summary>
        /// 动画事件委托
        /// </summary>
        public event Action<int> OnFrameChanged;          // 帧索引变化
        public event Action<int> OnLoopCompleted;         // 循环完成
        public event Action<string> OnStatusChanged;      // 状态变化

        [Header("资源设置")]
        [SerializeField] private string spriteBaseName = "cat_anim_";
        [SerializeField] private int totalFrames = 10;
        [SerializeField] private SpriteRenderer catRenderer;

        [Header("播放设置")]
        [SerializeField] private bool playOnInput = true;
        [SerializeField] private bool loopAnimation = true;

        [Header("状态")]
        [SerializeField] private int currentFrameIndex = 0;
        [SerializeField] private int completedLoops = 0;
        [SerializeField] private bool isPlaying = false;

        private Sprite[] animationFrames;
        private InputManager inputManager;
        private bool hasValidFrames;

        /// <summary>
        /// 当前帧索引（0-9）
        /// </summary>
        public int CurrentFrameIndex => currentFrameIndex;

        /// <summary>
        /// 完成的循环次数
        /// </summary>
        public int CompletedLoops => completedLoops;

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying => isPlaying;

        /// <summary>
        /// 总帧数
        /// </summary>
        public int TotalFrames => totalFrames;

        /// <summary>
        /// 是否有有效的动画帧
        /// </summary>
        public bool HasValidFrames => hasValidFrames;

        /// <summary>
        /// 获取状态信息
        /// </summary>
        public string StatusInfo => $"循环{completedLoops}，帧{currentFrameIndex + 1}/{totalFrames}";

        private void Awake()
        {
            InitializeComponents();
            LoadAnimationFrames();
            SetupInputConnection();
            ApplyFirstFrame();
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponents()
        {
            if (catRenderer == null)
            {
                catRenderer = GetComponent<SpriteRenderer>();
                if (catRenderer == null)
                {
                    catRenderer = gameObject.AddComponent<SpriteRenderer>();
                    Debug.LogWarning("AnimationManager: 添加了 SpriteRenderer 组件");
                }
            }

            // 查找输入管理器
            inputManager = FindObjectOfType<InputManager>();
            if (inputManager == null)
            {
                Debug.LogError("AnimationManager: 未找到 InputManager 组件");
            }
        }

        /// <summary>
        /// 加载动画帧
        /// </summary>
        private void LoadAnimationFrames()
        {
            animationFrames = new Sprite[totalFrames];
            int loadedFrames = 0;

            for (int i = 0; i < totalFrames; i++)
            {
                string frameName = $"{spriteBaseName}{i:00}";
                Sprite frame = Resources.Load<Sprite>($"CatAnimation/{frameName}");
                
                if (frame != null)
                {
                    animationFrames[i] = frame;
                    loadedFrames++;
                }
                else
                {
                    Debug.LogWarning($"AnimationManager: 无法加载帧 {frameName}");
                }
            }

            hasValidFrames = loadedFrames > 0;
            
            if (hasValidFrames)
            {
                Debug.Log($"AnimationManager: 成功加载 {loadedFrames}/{totalFrames} 帧");
                OnStatusChanged?.Invoke($"已加载 {loadedFrames}/{totalFrames} 帧");
            }
            else
            {
                Debug.LogError("AnimationManager: 未加载到任何动画帧");
                OnStatusChanged?.Invoke("错误：未找到动画资源");
            }
        }

        /// <summary>
        /// 设置输入连接
        /// </summary>
        private void SetupInputConnection()
        {
            if (inputManager != null && playOnInput)
            {
                inputManager.OnInputTriggered += PlayNextFrame;
                Debug.Log("AnimationManager: 已连接到输入系统");
            }
        }

        /// <summary>
        /// 应用第一帧
        /// </summary>
        private void ApplyFirstFrame()
        {
            if (hasValidFrames && catRenderer != null)
            {
                catRenderer.sprite = animationFrames[0];
                currentFrameIndex = 0;
                isPlaying = false;
                OnFrameChanged?.Invoke(0);
                OnStatusChanged?.Invoke("待机状态");
            }
        }

        /// <summary>
        /// 播放下一帧
        /// </summary>
        public void PlayNextFrame()
        {
            if (!hasValidFrames || animationFrames == null)
            {
                Debug.LogWarning("AnimationManager: 没有有效的动画帧可播放");
                return;
            }

            // 更新帧索引
            currentFrameIndex++;
            
            // 检查是否完成一个循环
            if (currentFrameIndex >= totalFrames)
            {
                completedLoops++;
                currentFrameIndex = 0; // 循环回到第一帧
                
                // 触发循环完成事件
                OnLoopCompleted?.Invoke(completedLoops);
                
                if (!loopAnimation)
                {
                    // 如果不循环，停止播放
                    isPlaying = false;
                    OnStatusChanged?.Invoke($"循环完成 {completedLoops} 次");
                    return;
                }
            }

            // 应用当前帧
            catRenderer.sprite = animationFrames[currentFrameIndex];
            isPlaying = true;
            
            // 触发事件
            OnFrameChanged?.Invoke(currentFrameIndex);
            OnStatusChanged?.Invoke(StatusInfo);
            
            Debug.Log($"AnimationManager: 播放帧 {currentFrameIndex + 1}/{totalFrames}, 循环 {completedLoops}");
        }

        /// <summary>
        /// 重置动画状态
        /// </summary>
        public void ResetAnimation()
        {
            currentFrameIndex = 0;
            completedLoops = 0;
            isPlaying = false;
            
            if (hasValidFrames && catRenderer != null)
            {
                catRenderer.sprite = animationFrames[0];
            }
            
            OnFrameChanged?.Invoke(0);
            OnStatusChanged?.Invoke("已重置");
            
            Debug.Log("AnimationManager: 动画已重置");
        }

        /// <summary>
        /// 手动设置帧
        /// </summary>
        public void SetFrame(int frameIndex)
        {
            if (!hasValidFrames || animationFrames == null)
            {
                return;
            }

            frameIndex = Mathf.Clamp(frameIndex, 0, totalFrames - 1);
            currentFrameIndex = frameIndex;
            
            if (catRenderer != null)
            {
                catRenderer.sprite = animationFrames[frameIndex];
            }
            
            OnFrameChanged?.Invoke(frameIndex);
        }

        /// <summary>
        /// 启用/禁用输入播放
        /// </summary>
        public void SetPlayOnInput(bool enable)
        {
            playOnInput = enable;
            
            if (inputManager != null)
            {
                if (enable)
                {
                    inputManager.OnInputTriggered += PlayNextFrame;
                }
                else
                {
                    inputManager.OnInputTriggered -= PlayNextFrame;
                }
            }
        }

        /// <summary>
        /// 获取详细的动画信息
        /// </summary>
        public string GetAnimationInfo()
        {
            return $"帧: {currentFrameIndex + 1}/{totalFrames}, 循环: {completedLoops}, 状态: {(isPlaying ? "播放中" : "待机")}";
        }

        private void OnDestroy()
        {
            // 清理事件订阅
            if (inputManager != null)
            {
                inputManager.OnInputTriggered -= PlayNextFrame;
            }
        }

        /// <summary>
        /// 用于调试：手动触发下一帧
        /// </summary>
        [ContextMenu("手动播放下一帧")]
        private void DebugPlayNextFrame()
        {
            PlayNextFrame();
        }

        /// <summary>
        /// 用于调试：重置动画
        /// </summary>
        [ContextMenu("重置动画")]
        private void DebugResetAnimation()
        {
            ResetAnimation();
        }
    }
}