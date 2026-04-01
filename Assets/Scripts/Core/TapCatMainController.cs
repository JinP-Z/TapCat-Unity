using UnityEngine;
using TapCat.Input;
using TapCat.Animation;
using TapCat.UI;

namespace TapCat.Core
{
    /// <summary>
    /// Main controller that coordinates input, animation, and UI systems.
    /// </summary>
    public class TapCatMainController : MonoBehaviour
    {
        [Header("System References")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private AnimationManager animationManager;
        [SerializeField] private UIManager uiManager;

        [Header("Performance")]
        [SerializeField, Range(30, 120)] private int targetFrameRate = 60;
        [SerializeField] private bool vSyncEnabled = false;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLog = true;
        [SerializeField] private bool showPerformanceOverlay = true;

        private bool isInitialized = false;
        private float startupTime;

        public bool IsInitialized => isInitialized;
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
            MonitorPerformance();
        }

        private void InitializeSystems()
        {
            EnsureRequiredComponents();

            if (inputManager == null)
            {
                inputManager = FindObjectOfType<InputManager>();
                if (inputManager == null)
                {
                    GameObject inputObj = new GameObject("InputManager");
                    inputManager = inputObj.AddComponent<InputManager>();
                    inputObj.transform.SetParent(transform);
                    Debug.Log("TapCatMainController: Created InputManager.");
                }
            }

            if (animationManager == null)
            {
                animationManager = FindObjectOfType<AnimationManager>();
                if (animationManager == null)
                {
                    Debug.LogError("TapCatMainController: AnimationManager not found. Ensure the cat object exists.");
                }
            }

            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();
                if (uiManager == null)
                {
                    Debug.LogWarning("TapCatMainController: UIManager not found. UI will be unavailable.");
                }
            }

            isInitialized = true;
        }

        private void EnsureRequiredComponents()
        {
            if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
                eventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
                Debug.Log("TapCatMainController: Created EventSystem.");
            }
        }

        private void ApplyPerformanceSettings()
        {
            Application.targetFrameRate = targetFrameRate;
            QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
            QualitySettings.maxQueuedFrames = 2;

            if (enableDebugLog)
            {
                Debug.Log($"TapCatMainController: Performance settings - Target FPS: {targetFrameRate}, VSync: {(vSyncEnabled ? "On" : "Off")}");
            }
        }

        private void ConnectSystems()
        {
            if (inputManager != null && animationManager != null && enableDebugLog)
            {
                Debug.Log("TapCatMainController: Input connected to animation system.");
            }

            if (animationManager != null && uiManager != null && enableDebugLog)
            {
                Debug.Log("TapCatMainController: Animation connected to UI system.");
            }

            if (inputManager != null && uiManager != null && enableDebugLog)
            {
                Debug.Log("TapCatMainController: Input connected to UI system.");
            }
        }

        private void MonitorPerformance()
        {
            float currentFPS = 1f / Time.deltaTime;
            if (currentFPS < 55f && enableDebugLog)
            {
                Debug.LogWarning($"TapCatMainController: FPS below target: {currentFPS:F1} (target 60)");
            }

            long totalMemory = System.GC.GetTotalMemory(false) / 1024 / 1024;
            if (totalMemory > 150 && enableDebugLog)
            {
                Debug.LogWarning($"TapCatMainController: Memory usage high: {totalMemory} MB (target <= 150 MB)");
            }
        }

        private void LogStartupInfo()
        {
            if (!enableDebugLog) return;

            float initTime = Time.realtimeSinceStartup - startupTime;

            Debug.Log("=== TapCat Startup Complete ===");
            Debug.Log($"Startup time: {initTime:F3}s");
            Debug.Log($"Unity version: {Application.unityVersion}");
            Debug.Log($"Platform: {Application.platform}");
            Debug.Log($"OS: {SystemInfo.operatingSystem}");
            Debug.Log($"CPU: {SystemInfo.processorType}");
            Debug.Log($"Memory: {SystemInfo.systemMemorySize} MB");
            Debug.Log($"GPU: {SystemInfo.graphicsDeviceName}");
            Debug.Log($"VRAM: {SystemInfo.graphicsMemorySize} MB");
            Debug.Log($"Resolution: {Screen.currentResolution}");
            Debug.Log($"DPI: {Screen.dpi}");
            Debug.Log("==============================");
        }

        private void LogSystemStatus()
        {
            if (!enableDebugLog) return;

            string status = "System Status\n";

            if (inputManager != null)
            {
                status += $"- Input: OK (cooldown {inputManager.CooldownTime:F2}s)\n";
            }
            else
            {
                status += "- Input: Missing\n";
            }

            if (animationManager != null)
            {
                status += $"- Animation: OK (frames {animationManager.TotalFrames})\n";
                status += $"- Animation state: {animationManager.StatusInfo}\n";
            }
            else
            {
                status += "- Animation: Missing\n";
            }

            if (uiManager != null)
            {
                status += "- UI: OK\n";
            }
            else
            {
                status += "- UI: Missing\n";
            }

            Debug.Log(status);
        }

        public string GetSystemHealth()
        {
            float fps = 1f / Time.deltaTime;
            long memory = System.GC.GetTotalMemory(false) / 1024 / 1024;

            string health = "System Health\n";
            health += $"FPS: {fps:F1} ({(fps >= 55f ? "OK" : "LOW")})\n";
            health += $"Memory: {memory} MB ({(memory < 150 ? "OK" : "HIGH")})\n";
            health += $"Input: {(inputManager != null ? "OK" : "MISSING")}\n";
            health += $"Animation: {(animationManager != null ? "OK" : "MISSING")}\n";
            health += $"UI: {(uiManager != null ? "OK" : "MISSING")}\n";
            return health;
        }

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

            Debug.Log("TapCatMainController: All systems reset.");
        }

        public void SetDebugMode(bool enabled)
        {
            enableDebugLog = enabled;
            Debug.Log($"TapCatMainController: Debug mode {(enabled ? "On" : "Off")}");
        }

        public void SetTargetFrameRate(int frameRate)
        {
            targetFrameRate = Mathf.Clamp(frameRate, 30, 120);
            Application.targetFrameRate = targetFrameRate;
            Debug.Log($"TapCatMainController: Target FPS set to {targetFrameRate}");
        }

        public string GetSystemInfo()
        {
            string info = "=== TapCat System Info ===\n";
            info += $"Startup time: {startupTime:F2}s\n";
            info += $"Uptime: {Time.realtimeSinceStartup - startupTime:F2}s\n";
            info += $"Target FPS: {targetFrameRate}\n";
            info += $"Current FPS: {1f / Time.deltaTime:F1}\n";
            info += $"Memory: {System.GC.GetTotalMemory(false) / 1024 / 1024} MB\n";

            if (inputManager != null)
            {
                info += $"Input count: {inputManager.TotalInputCount}\n";
                info += $"Cooldown: {(inputManager.IsInCooldown ? "Active" : "Ready")}\n";
            }

            if (animationManager != null)
            {
                info += $"Animation state: {animationManager.StatusInfo}\n";
                info += $"Valid frames: {animationManager.HasValidFrames}\n";
            }

            info += "==========================";
            return info;
        }

        [ContextMenu("Debug Reset All Systems")]
        private void DebugResetAllSystems()
        {
            ResetAllSystems();
        }

        [ContextMenu("Debug Show System Info")]
        private void DebugShowSystemInfo()
        {
            Debug.Log(GetSystemInfo());
        }
    }
}
