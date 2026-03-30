using UnityEngine;
using TapCat.Input;
using TapCat.UI;

namespace TapCat.Core
{
    /// <summary>
    /// Integrates input events with animation playback and tap tracking.
    /// Performance & lifecycle: event-driven input, lightweight periodic performance checks.
    /// </summary>
    [DisallowMultipleComponent]
    public class TapCatPhase3Manager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputManager inputManager;
        [SerializeField] private AnimationController animationController;
        [SerializeField] private TapCat.TapCatController tapCatController;
        [SerializeField] private UIManager uiManager;

        [Header("Performance Monitoring")]
        [SerializeField] private bool enablePerformanceMonitoring = true;
        [SerializeField, Range(0.5f, 5f)] private float performanceCheckInterval = 1f;

        private float performanceTimer;
        private int frameCounter;
        private bool wasBelowFps;
        private bool wasHighMemory;
        private bool lastCooldownState;

        /// <summary>
        /// 注入 Phase3 依赖引用，由引导器统一协调。
        /// </summary>
        public void Inject(InputManager input, AnimationController animation, TapCat.TapCatController controller, UIManager ui = null)
        {
            inputManager = input;
            animationController = animation;
            tapCatController = controller;
            uiManager = ui;

            InitializeUI();
        }

        private void Awake()
        {
            EnsureReferences();
            InitializeUI();
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void Update()
        {
            SyncCooldownState();
            UpdatePerformance();
        }

        private void EnsureReferences()
        {
            if (inputManager == null)
            {
                inputManager = FindObjectOfType<InputManager>();
            }

            if (animationController == null)
            {
                animationController = FindObjectOfType<AnimationController>();
            }

            if (tapCatController == null)
            {
                tapCatController = FindObjectOfType<TapCat.TapCatController>();
            }

            if (uiManager == null)
            {
                uiManager = FindObjectOfType<UIManager>();
            }
        }

        private void InitializeUI()
        {
            if (uiManager != null)
            {
                uiManager.Initialize(inputManager, animationController);
            }
        }

        private void SubscribeToEvents()
        {
            if (inputManager != null)
            {
                inputManager.OnInputTriggered += HandleInputTriggered;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (inputManager != null)
            {
                inputManager.OnInputTriggered -= HandleInputTriggered;
            }
        }

        private void HandleInputTriggered()
        {
            if (animationController != null)
            {
                animationController.PlayNextFrame();
            }

            if (tapCatController != null)
            {
                tapCatController.OnCatTapped();
            }
        }

        private void SyncCooldownState()
        {
            if (inputManager == null || animationController == null)
            {
                return;
            }

            bool isCooldown = inputManager.IsInCooldown;
            if (isCooldown != lastCooldownState)
            {
                animationController.SetCooldownState(isCooldown);
                lastCooldownState = isCooldown;
            }
        }

        private void UpdatePerformance()
        {
            if (!enablePerformanceMonitoring)
            {
                return;
            }

            frameCounter++;
            performanceTimer += Time.unscaledDeltaTime;

            if (performanceTimer < performanceCheckInterval)
            {
                return;
            }

            float fps = frameCounter / performanceTimer;
            frameCounter = 0;
            performanceTimer = 0f;

            bool belowFps = fps < 55f;
            if (belowFps && !wasBelowFps)
            {
                Debug.LogWarning($"TapCatPhase3Manager: FPS below target: {fps:F1}");
            }
            wasBelowFps = belowFps;

            long memoryMb = System.GC.GetTotalMemory(false) / (1024 * 1024);
            bool highMemory = memoryMb > 150;
            if (highMemory && !wasHighMemory)
            {
                Debug.LogWarning($"TapCatPhase3Manager: Memory high: {memoryMb}MB");
            }
            wasHighMemory = highMemory;
        }
    }
}
