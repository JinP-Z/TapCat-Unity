using UnityEngine;
using TapCat.Input;
using TapCat.Animation;
using TapCat.UI;

namespace TapCat.Core
{
    /// <summary>
    /// TapCat主控制器，负责协调所有系统模块
    /// 符合技术宪法第三章：开发规范
    /// </summary>
    public class TapCatMainController : MonoBehaviour
    {
        [Header("系统引用")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private AnimationManager animationManager;
        [SerializeField] private UIManager uiManager;
        
        [Header("性能设置")]
        [SerializeField, Range(30, 120)] private int targetFrameRate = 60;
        [SerializeField] private bool vSyncEnabled = false;
        
        [Header("调试设置")]
        [SerializeField] private bool enableDebugLog = true;
        [SerializeField] private bool showPerformanceOverlay = true;
        
        private bool isInitialized = false;
        private float startupTime;
        
        /// <summary>
        /// 初始化状态
        /// </summary>
        public bool IsInitialized => isInitialized;
        
        /// <summary>
        /// 启动时间
        /// </summary>
        public float StartupTime => startupTime;
        
        private void Awake()
        {
            startupTime = Time.realtimeSinceStartup;
            InitializeSystems();
            ApplyPerformanceSettings();
            LogStartupInfo();
        }
        
        private void Start()
        {
            ConnectSystems();
            LogSystemStatus();
        }
        
        private void Update()
        {
            // 性能监控
            MonitorPerformance();
        }
        
        /// <summary>
        /// 初始化所有系统
        /// </summary>
        private void InitializeSystems()
        {
            // 确保必要的组件存在
            EnsureRequiredComponents();
            
            // 初始化输入系统
            if (inputManager == null)
            {
                inputManager = FindObjectOfType<InputManager>();
                if (inputManager == null)
                {
                    GameObject inputObj = new GameObject("InputManager");
                    inputManager = inputObj.AddComponent<InputManager>();
                    inputObj.transform.SetParent(transform);
                    Debug.Log("TapCatMainController: 创建了 InputManager");
                }
            }
            
            // 初始化动画系统
            if (animationManager == null)
            {
                animationManager = FindObjectOfType<AnimationManager>();
                if (animationManager == null)
                {
                    Debug.LogError("TapCatMainController: 未找到 AnimationManager，请确保场景中有猫咪对象");
                }
            }
            
            // 初始化UI系统
            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();
                if (uiManager == null)
                {
                    Debug.LogWarning("TapCatMainController: 未找到 UIManager，UI功能将不可用");
                }
            }
            
            isInitialized = true;
        }
        
        /// <summary>
        /// 确保必要的组件存在
        /// </summary>
        private void EnsureRequiredComponents()
        {
            // 确保有事件系统
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Debug.Log("TapCatMainController: 创建了 EventSystem");
            }
        }
        
        /// <summary>
        /// 应用性能设置
        /// </summary>
        private void ApplyPerformanceSettings()
        {
            // 设置目标帧率（技术宪法第八条：稳定60FPS）
            Application.targetFrameRate = targetFrameRate;
            
            // 设置垂直同步
            QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
            
            // 禁用不必要的行为以提高性能
            QualitySettings.maxQueuedFrames = 2;
            
            if (enableDebugLog)
            {
                Debug.Log($"TapCatMainController: 性能设置 - 目标帧率: {targetFrameRate}, VSync: {(vSyncEnabled ? "启用" : "禁用")}");
            }
        }
        
        /// <summary>
        /// 连接所有系统
        /// </summary>
        private void ConnectSystems()
        {
            // 连接输入系统到动画系统
            if (inputManager != null && animationManager != null)
            {
                // 输入管理器已经通过事件连接动画管理器
                if (enableDebugLog)
                {
                    Debug.Log("TapCatMainController: 已连接输入系统到动画系统");
                }
            }
            
            // 连接动画系统到UI系统（通过事件）
            if (animationManager != null && uiManager != null)
            {
                // UI管理器已经订阅了动画管理器的事件
                if (enableDebugLog)
                {
                    Debug.Log("TapCatMainController: 已连接动画系统到UI系统");
                }
            }
            
            // 连接输入系统到UI系统
            if (inputManager != null && uiManager != null)
            {
                // UI管理器已经订阅了输入管理器的事件
                if (enableDebugLog)
                {
                    Debug.Log("TapCatMainController: 已连接输入系统到UI系统");
                }
            }
        }
        
        /// <summary>
        /// 监控性能
        /// </summary>
        private void MonitorPerformance()
        {
            // 检查帧率是否符合要求
            float currentFPS = 1f / Time.deltaTime;
            
            // 如果帧率低于55FPS，记录警告（技术宪法第八条：不允许低于55FPS）
            if (currentFPS < 55f && enableDebugLog)
            {
                Debug.LogWarning($"TapCatMainController: 帧率低于要求: {currentFPS:F1}FPS (要求: 稳定60FPS)");
            }
            
            // 检查内存使用
            long totalMemory = System.GC.GetTotalMemory(false) / 1024 / 1024; // MB
            
            // 如果内存超过150MB，记录警告（技术宪法第八条：峰值<150MB）
            if (totalMemory > 150 && enableDebugLog)
            {
                Debug.LogWarning($"TapCatMainController: 内存使用过高: {totalMemory}MB (要求: 峰值<150MB)");
            }
        }
        
        /// <summary>
        /// 记录启动信息
        /// </summary>
        private void LogStartupInfo()
        {
            if (!enableDebugLog) return;
            
            float initTime = Time.realtimeSinceStartup - startupTime;
            
            Debug.Log($"=== TapCat 启动完成 ===");
            Debug.Log($"启动时间: {initTime:F3}秒");
            Debug.Log($"Unity版本: {Application.unityVersion}");
            Debug.Log($"平台: {Application.platform}");
            Debug.Log($"系统: {SystemInfo.operatingSystem}");
            Debug.Log($"处理器: {SystemInfo.processorType}");
            Debug.Log($"内存: {SystemInfo.systemMemorySize}MB");
            Debug.Log($"显卡: {SystemInfo.graphicsDeviceName}");
            Debug.Log($"显存: {SystemInfo.graphicsMemorySize}MB");
            Debug.Log($"分辨率: {Screen.currentResolution}");
            Debug.Log($"DPI缩放: {Screen.dpi}");
            Debug.Log($"=========================");
        }
        
        /// <summary>
        /// 记录系统状态
        /// </summary>
        private void LogSystemStatus()
        {
            if (!enableDebugLog) return;
            
            string status = "系统状态:\n";
            
            if (inputManager != null)
            {
                status += $"- 输入系统: 正常 (冷却时间: {inputManager.CooldownTime:F2}s)\n";
            }
            else
            {
                status += "- 输入系统: 未找到\n";
            }
            
            if (animationManager != null)
            {
                status += $"- 动画系统: 正常 (帧数: {animationManager.TotalFrames})\n";
                status += $"- 动画状态: {animationManager.StatusInfo}\n";
            }
            else
            {
                status += "- 动画系统: 未找到\n";
            }
            
            if (uiManager != null)
            {
                status += "- UI系统: 正常\n";
            }
            else
            {
                status += "- UI系统: 未找到\n";
            }
            
            Debug.Log(status);
        }
        
        /// <summary>
        /// 获取系统健康状态
        /// </summary>
        public string GetSystemHealth()
        {
            string health = "系统健康检查:\n";
            
            // 检查帧率
            float fps = 1f / Time.deltaTime;
            health += $"帧率: {fps:F1}FPS ({(fps >= 55f ? "✓" : "✗")})\n";
            
            // 检查内存
            long memory = System.GC.GetTotalMemory(false) / 1024 / 1024;
            health += $"内存: {memory}MB ({(memory < 150 ? "✓" : "✗")})\n";
            
            // 检查系统组件
            health += $"输入系统: {(inputManager != null ? "✓" : "✗")}\n";
            health += $"动画系统: {(animationManager != null ? "✓" : "✗")}\n";
            health += $"UI系统: {(uiManager != null ? "✓" : "✗")}\n";
            
            return health;
        }
        
        /// <summary>
        /// 重置所有系统
        /// </summary>
        public void ResetAllSystems()
        {
            if (inputManager != null)
            {
                inputManager.ResetInputCount();
            }
            
            if (animationManager != null)
            {
                animationManager.ResetAnimation();
            }
            
            Debug.Log("TapCatMainController: 所有系统已重置");
        }
        
        /// <summary>
        /// 设置调试模式
        /// </summary>
        public void SetDebugMode(bool enabled)
        {
            enableDebugLog = enabled;
            Debug.Log($"TapCatMainController: 调试模式 {(enabled ? "启用" : "禁用")}");
        }
        
        /// <summary>
        /// 设置目标帧率
        /// </summary>
        public void SetTargetFrameRate(int frameRate)
        {
            targetFrameRate = Mathf.Clamp(frameRate, 30, 120);
            Application.targetFrameRate = targetFrameRate;
            Debug.Log($"TapCatMainController: 目标帧率设置为 {targetFrameRate}FPS");
        }
        
        /// <summary>
        /// 获取详细系统信息
        /// </summary>
        public string GetSystemInfo()
        {
            string info = "=== TapCat 系统信息 ===\n";
            
            info += $"启动时间: {startupTime:F2}s\n";
            info += $"运行时间: {Time.realtimeSinceStartup - startupTime:F2}s\n";
            info += $"目标帧率: {targetFrameRate}FPS\n";
            info += $"实际帧率: {1f / Time.deltaTime:F1}FPS\n";
            info += $"内存使用: {System.GC.GetTotalMemory(false) / 1024 / 1024}MB\n";
            
            if (inputManager != null)
            {
                info += $"输入次数: {inputManager.TotalInputCount}\n";
                info += $"冷却状态: {(inputManager.IsInCooldown ? "冷却中" : "就绪")}\n";
            }
            
            if (animationManager != null)
            {
                info += $"动画状态: {animationManager.StatusInfo}\n";
                info += $"有效帧: {animationManager.HasValidFrames}\n";
            }
            
            info += "=======================";
            
            return info;
        }
        
        /// <summary>
        /// 用于调试：重置所有系统
        /// </summary>
        [ContextMenu("重置所有系统")]
        private void DebugResetAllSystems()
        {
            ResetAllSystems();
        }
        
        /// <summary>
        /// 用于调试：获取系统信息
        /// </summary>
        [ContextMenu("显示系统信息")]
        private void DebugShowSystemInfo()
        {
            Debug.Log(GetSystemInfo());
        }
    }
}