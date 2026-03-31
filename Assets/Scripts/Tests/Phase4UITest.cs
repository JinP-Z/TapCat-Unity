using UnityEngine;
using TapCat.UI;
using TapCat.Input;
using TapCat.Core;

namespace TapCat.Tests
{
    /// <summary>
    /// Phase4 UI System Test
    /// This script tests the Phase4 UI components integration
    /// </summary>
    public class Phase4UITest : MonoBehaviour
    {
        [Header("Test Configuration")]
        [SerializeField] private bool autoStartTest = true;
        [SerializeField] private float testDuration = 10f;
        
        [Header("References")]
        [SerializeField] private UIManager uiManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private AnimationController animationController;
        
        private float testTimer;
        private bool testRunning;
        private int testClickCount;
        
        void Start()
        {
            if (autoStartTest)
            {
                StartTest();
            }
        }
        
        void Update()
        {
            if (!testRunning) return;
            
            testTimer -= Time.deltaTime;
            
            // Simulate clicks for testing
            if (Time.frameCount % 30 == 0) // Every half second at 60 FPS
            {
                testClickCount++;
                Debug.Log($"Test Click #{testClickCount} - UI should update");
            }
            
            if (testTimer <= 0)
            {
                EndTest();
            }
        }
        
        /// <summary>
        /// Start the Phase4 UI test
        /// </summary>
        public void StartTest()
        {
            if (testRunning)
            {
                Debug.LogWarning("Test is already running!");
                return;
            }
            
            // Validate references
            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();
                if (uiManager == null)
                {
                    Debug.LogError("UIManager not found in scene!");
                    return;
                }
            }
            
            if (inputManager == null)
            {
                inputManager = FindObjectOfType<InputManager>();
                if (inputManager == null)
                {
                    Debug.LogError("InputManager not found in scene!");
                    return;
                }
            }
            
            if (animationController == null)
            {
                animationController = FindObjectOfType<AnimationController>();
                if (animationController == null)
                {
                    Debug.LogError("AnimationController not found in scene!");
                    return;
                }
            }
            
            // Initialize UI Manager
            if (!uiManager.IsInitialized)
            {
                uiManager.Initialize(inputManager, animationController);
            }
            
            // Start test
            testTimer = testDuration;
            testRunning = true;
            testClickCount = 0;
            
            Debug.Log($"🚀 Phase4 UI Test Started - Duration: {testDuration} seconds");
            Debug.Log($"UI Manager Initialized: {uiManager.IsInitialized}");
            Debug.Log($"Input Manager: {inputManager != null}");
            Debug.Log($"Animation Controller: {animationController != null}");
        }
        
        /// <summary>
        /// End the test and report results
        /// </summary>
        public void EndTest()
        {
            if (!testRunning) return;
            
            testRunning = false;
            
            // Report test results
            Debug.Log($"✅ Phase4 UI Test Completed");
            Debug.Log($"Test Duration: {testDuration} seconds");
            Debug.Log($"Simulated Clicks: {testClickCount}");
            Debug.Log($"UI Manager Status: {(uiManager != null ? "Active" : "Missing")}");
            Debug.Log($"UI Initialized: {(uiManager != null ? uiManager.IsInitialized.ToString() : "N/A")}");
            
            // Check for UI components
            var clickCounter = FindObjectOfType<ClickCounterUI>();
            var statusIndicator = FindObjectOfType<StatusIndicatorUI>();
            
            Debug.Log($"ClickCounterUI Found: {clickCounter != null}");
            Debug.Log($"StatusIndicatorUI Found: {statusIndicator != null}");
            
            if (clickCounter != null && statusIndicator != null && uiManager != null && uiManager.IsInitialized)
            {
                Debug.Log($"🎉 Phase4 UI System Test PASSED!");
            }
            else
            {
                Debug.Log($"⚠️ Phase4 UI System Test has issues - check logs above");
            }
        }
        
        /// <summary>
        /// Manual test trigger
        /// </summary>
        [ContextMenu("Run Phase4 UI Test")]
        private void RunTestContext()
        {
            StartTest();
        }
        
        /// <summary>
        /// Manual test stop
        /// </summary>
        [ContextMenu("Stop Phase4 UI Test")]
        private void StopTestContext()
        {
            EndTest();
        }
        
        void OnGUI()
        {
            if (!testRunning) return;
            
            // Display test status in GUI
            GUILayout.BeginArea(new Rect(10, 10, 300, 200));
            GUILayout.Box("Phase4 UI System Test");
            GUILayout.Label($"Time Remaining: {testTimer:F1}s");
            GUILayout.Label($"Simulated Clicks: {testClickCount}");
            GUILayout.Label($"UI Initialized: {uiManager.IsInitialized}");
            
            if (GUILayout.Button("Stop Test Early"))
            {
                EndTest();
            }
            
            GUILayout.EndArea();
        }
    }
}