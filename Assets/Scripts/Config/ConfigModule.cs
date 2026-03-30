using System;
using System.IO;
using UnityEngine;

namespace TapCat.Config
{
    /// <summary>
    /// Configuration data structure for TapCat settings.
    /// </summary>
    [Serializable]
    public class TapCatConfig
    {
        // Input settings
        public float cooldownTime = 0.2f;
        public bool enableCooldown = true;
        
        // Window settings
        public Vector2 windowPosition = new Vector2(100, 100);
        public bool alwaysOnTop = true;
        public float windowOpacity = 1.0f;
        public float windowScale = 1.0f;
        
        // Animation settings
        public bool showStatusInfo = true;
        
        // UI settings
        public bool showClickCounter = true;
        public bool showPerformanceStats = false;
        
        // Default constructor
        public TapCatConfig() { }
        
        // Copy constructor
        public TapCatConfig(TapCatConfig other)
        {
            if (other == null) return;
            
            cooldownTime = other.cooldownTime;
            enableCooldown = other.enableCooldown;
            windowPosition = other.windowPosition;
            alwaysOnTop = other.alwaysOnTop;
            windowOpacity = other.windowOpacity;
            windowScale = other.windowScale;
            showStatusInfo = other.showStatusInfo;
            showClickCounter = other.showClickCounter;
            showPerformanceStats = other.showPerformanceStats;
        }
    }

    /// <summary>
    /// Configuration manager that handles saving and loading settings to/from JSON file.
    /// </summary>
    public class ConfigModule : MonoBehaviour
    {
        /// <summary>
        /// Event fired when configuration changes.
        /// </summary>
        public event Action<TapCatConfig> OnConfigChanged;

        [Header("File Settings")]
        [SerializeField] private string configFileName = "tapcat_config.json";
        
        private TapCatConfig currentConfig;
        private string filePath;
        private bool isInitialized = false;

        /// <summary>
        /// Current configuration (read-only).
        /// </summary>
        public TapCatConfig CurrentConfig => currentConfig;

        /// <summary>
        /// Whether the module is initialized.
        /// </summary>
        public bool IsInitialized => isInitialized;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the configuration module.
        /// </summary>
        public void Initialize()
        {
            if (isInitialized) return;

            SetupFilePath();
            LoadConfig();
            isInitialized = true;
            
            Debug.Log($"ConfigModule: Initialized with config file at {filePath}");
        }

        /// <summary>
        /// Set up the configuration file path.
        /// </summary>
        private void SetupFilePath()
        {
            // Use Application.persistentDataPath for cross-platform support
            string configDirectory = Path.Combine(Application.persistentDataPath, "TapCat");
            EnsureDirectoryExists(configDirectory);
            filePath = Path.Combine(configDirectory, configFileName);
        }

        /// <summary>
        /// Ensure the configuration directory exists.
        /// </summary>
        private void EnsureDirectoryExists(string directory)
        {
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                    Debug.Log($"ConfigModule: Created directory {directory}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"ConfigModule: Failed to create directory {directory}: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Load configuration from file, or create default if file doesn't exist.
        /// </summary>
        public void LoadConfig()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    currentConfig = JsonUtility.FromJson<TapCatConfig>(json);
                    Debug.Log($"ConfigModule: Loaded config from {filePath}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"ConfigModule: Failed to load config from {filePath}: {ex.Message}");
                    CreateDefaultConfig();
                }
            }
            else
            {
                CreateDefaultConfig();
                SaveConfig(); // Save default config for first-time users
            }
        }

        /// <summary>
        /// Create default configuration.
        /// </summary>
        private void CreateDefaultConfig()
        {
            currentConfig = new TapCatConfig();
            Debug.Log("ConfigModule: Created default configuration");
        }

        /// <summary>
        /// Save current configuration to file.
        /// </summary>
        public void SaveConfig()
        {
            if (currentConfig == null)
            {
                Debug.LogWarning("ConfigModule: Cannot save null configuration");
                return;
            }

            try
            {
                string json = JsonUtility.ToJson(currentConfig, true);
                File.WriteAllText(filePath, json);
                Debug.Log($"ConfigModule: Saved config to {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"ConfigModule: Failed to save config to {filePath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Update configuration with new values.
        /// </summary>
        public void UpdateConfig(TapCatConfig newConfig)
        {
            if (newConfig == null)
            {
                Debug.LogWarning("ConfigModule: Cannot update with null config");
                return;
            }

            currentConfig = new TapCatConfig(newConfig);
            SaveConfig();
            OnConfigChanged?.Invoke(currentConfig);
            
            Debug.Log("ConfigModule: Configuration updated and saved");
        }

        /// <summary>
        /// Update specific configuration values.
        /// </summary>
        public void UpdateConfig(Action<TapCatConfig> updateAction)
        {
            if (updateAction == null)
            {
                Debug.LogWarning("ConfigModule: Cannot update with null action");
                return;
            }

            updateAction(currentConfig);
            SaveConfig();
            OnConfigChanged?.Invoke(currentConfig);
            
            Debug.Log("ConfigModule: Configuration partially updated and saved");
        }

        /// <summary>
        /// Reset configuration to defaults.
        /// </summary>
        public void ResetToDefaults()
        {
            CreateDefaultConfig();
            SaveConfig();
            OnConfigChanged?.Invoke(currentConfig);
            
            Debug.Log("ConfigModule: Configuration reset to defaults");
        }

        /// <summary>
        /// Get configuration file path.
        /// </summary>
        public string GetConfigFilePath()
        {
            return filePath;
        }

        /// <summary>
        /// Get configuration summary.
        /// </summary>
        public string GetConfigSummary()
        {
            if (currentConfig == null) return "Configuration not loaded";
            
            return $"冷却: {currentConfig.cooldownTime:F2}s, " +
                   $"置顶: {currentConfig.alwaysOnTop}, " +
                   $"透明度: {currentConfig.windowOpacity:F2}, " +
                   $"缩放: {currentConfig.windowScale:F2}";
        }

        private void OnDestroy()
        {
            // Auto-save on destruction
            if (isInitialized && currentConfig != null)
            {
                SaveConfig();
            }
        }

        /// <summary>
        /// For debugging: Print current configuration.
        /// </summary>
        [ContextMenu("Print Current Config")]
        private void DebugPrintConfig()
        {
            if (currentConfig == null)
            {
                Debug.Log("ConfigModule: No configuration loaded");
                return;
            }

            Debug.Log($"ConfigModule: Current Configuration\n" +
                     $"File: {filePath}\n" +
                     $"Cooldown: {currentConfig.cooldownTime:F2}s\n" +
                     $"Enable Cooldown: {currentConfig.enableCooldown}\n" +
                     $"Window Position: {currentConfig.windowPosition}\n" +
                     $"Always On Top: {currentConfig.alwaysOnTop}\n" +
                     $"Window Opacity: {currentConfig.windowOpacity:F2}\n" +
                     $"Window Scale: {currentConfig.windowScale:F2}\n" +
                     $"Show Status: {currentConfig.showStatusInfo}\n" +
                     $"Show Counter: {currentConfig.showClickCounter}\n" +
                     $"Show Performance: {currentConfig.showPerformanceStats}");
        }

        /// <summary>
        /// For debugging: Open config directory.
        /// </summary>
        [ContextMenu("Open Config Directory")]
        private void DebugOpenConfigDirectory()
        {
            string directory = Path.GetDirectoryName(filePath);
            if (Directory.Exists(directory))
            {
                System.Diagnostics.Process.Start("explorer.exe", directory);
            }
            else
            {
                Debug.LogWarning($"ConfigModule: Directory does not exist: {directory}");
            }
        }
    }
}