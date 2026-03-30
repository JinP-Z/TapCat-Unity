using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TapCat.Core;
using TapCat.Input;

namespace TapCat.UI
{
    /// <summary>
    /// Phase4 UI manager.
    /// Performance & lifecycle: event-driven UI, no Update loop, UI objects created once and reused.
    /// </summary>
    [DisallowMultipleComponent]
    public class UIManager : MonoBehaviour
    {
        [Header("Prefabs (Optional)")]
        [SerializeField] private ClickCounterUI clickCounterPrefab;
        [SerializeField] private StatusIndicatorUI statusIndicatorPrefab;

        [Header("Layout")]
        [SerializeField] private Vector2 panelAnchor = new Vector2(0f, 1f);
        [SerializeField] private Vector2 panelPivot = new Vector2(0f, 1f);
        [SerializeField] private Vector2 panelPosition = new Vector2(16f, -16f);
        [SerializeField] private Vector2 elementSize = new Vector2(240f, 28f);
        [SerializeField, Range(0f, 24f)] private float elementSpacing = 6f;

        private InputManager inputManager;
        private AnimationController animationController;
        private Canvas uiCanvas;
        private RectTransform panelTransform;
        private ClickCounterUI clickCounter;
        private StatusIndicatorUI statusIndicator;
        private bool isInitialized;

        /// <summary>
        /// Whether the UI manager has completed initialization.
        /// </summary>
        public bool IsInitialized => isInitialized;

        /// <summary>
        /// Initialize the UI manager with Phase3 dependencies.
        /// </summary>
        public void Initialize(InputManager input, AnimationController animation)
        {
            inputManager = input;
            animationController = animation;

            EnsureEventSystem();
            EnsureCanvas();
            EnsurePanel();
            EnsureWidgets();
            BindWidgets();

            isInitialized = true;
        }

        private void Awake()
        {
            if (!isInitialized)
            {
                InputManager input = FindObjectOfType<InputManager>();
                AnimationController animation = FindObjectOfType<AnimationController>();
                Initialize(input, animation);
            }
        }

        private void EnsureEventSystem()
        {
            if (FindObjectOfType<EventSystem>() != null)
            {
                return;
            }

            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        private void EnsureCanvas()
        {
            if (uiCanvas == null)
            {
                uiCanvas = GetComponentInChildren<Canvas>(true);
            }

            if (uiCanvas != null)
            {
                return;
            }

            GameObject canvasObject = new GameObject("TapCatUICanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            uiCanvas = canvasObject.GetComponent<Canvas>();
            uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
        }

        private void EnsurePanel()
        {
            if (panelTransform == null && uiCanvas != null)
            {
                Transform existing = uiCanvas.transform.Find("TapCatUIPanel");
                if (existing != null)
                {
                    panelTransform = existing as RectTransform;
                }
            }

            if (panelTransform != null || uiCanvas == null)
            {
                return;
            }

            GameObject panelObject = new GameObject("TapCatUIPanel", typeof(RectTransform));
            panelObject.transform.SetParent(uiCanvas.transform, false);
            panelTransform = panelObject.GetComponent<RectTransform>();

            panelTransform.anchorMin = panelAnchor;
            panelTransform.anchorMax = panelAnchor;
            panelTransform.pivot = panelPivot;
            panelTransform.anchoredPosition = panelPosition;
            float height = elementSize.y * 2f + elementSpacing;
            panelTransform.sizeDelta = new Vector2(elementSize.x, height);
        }

        private void EnsureWidgets()
        {
            if (panelTransform == null)
            {
                return;
            }

            if (clickCounter == null)
            {
                clickCounter = panelTransform.GetComponentInChildren<ClickCounterUI>(true);
            }

            if (statusIndicator == null)
            {
                statusIndicator = panelTransform.GetComponentInChildren<StatusIndicatorUI>(true);
            }

            if (clickCounter == null)
            {
                clickCounter = CreateClickCounter();
            }

            if (statusIndicator == null)
            {
                statusIndicator = CreateStatusIndicator();
            }
        }

        private void BindWidgets()
        {
            if (clickCounter != null)
            {
                clickCounter.Bind(inputManager);
            }

            if (statusIndicator != null)
            {
                statusIndicator.Bind(animationController);
            }
        }

        private ClickCounterUI CreateClickCounter()
        {
            ClickCounterUI instance = clickCounterPrefab != null
                ? Instantiate(clickCounterPrefab, panelTransform)
                : CreateWidget<ClickCounterUI>("ClickCounter");
            ConfigureWidgetTransform(instance.GetComponent<RectTransform>(), 0f);
            return instance;
        }

        private StatusIndicatorUI CreateStatusIndicator()
        {
            StatusIndicatorUI instance = statusIndicatorPrefab != null
                ? Instantiate(statusIndicatorPrefab, panelTransform)
                : CreateWidget<StatusIndicatorUI>("StatusIndicator");
            ConfigureWidgetTransform(instance.GetComponent<RectTransform>(), -(elementSize.y + elementSpacing));
            return instance;
        }

        private T CreateWidget<T>(string name) where T : MonoBehaviour
        {
            GameObject obj = new GameObject(name, typeof(RectTransform));
            obj.transform.SetParent(panelTransform, false);
            return obj.AddComponent<T>();
        }

        private void ConfigureWidgetTransform(RectTransform rectTransform, float offsetY)
        {
            if (rectTransform == null)
            {
                return;
            }

            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(0f, 1f);
            rectTransform.pivot = new Vector2(0f, 1f);
            rectTransform.anchoredPosition = new Vector2(0f, offsetY);
            rectTransform.sizeDelta = elementSize;
        }
    }
}
