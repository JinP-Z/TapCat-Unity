using UnityEngine;
using UnityEngine.UI;

namespace TapCat
{
    /// <summary>
    /// Final one-stop setup for TapCat 2D. Add to a scene to auto-create everything.
    /// </summary>
    public class TapCat2DFinalSetup : MonoBehaviour
    {
        [Header("Auto Setup Options")]
        [SerializeField] private bool autoCreateCat = true;
        [SerializeField] private bool autoCreateUI = true;
        [SerializeField] private bool autoSetupCamera = true;
        [SerializeField] private bool createPlaceholdersIfNeeded = true;

        [Header("Cat Settings")]
        [SerializeField] private Vector3 catPosition = Vector3.zero;
        [SerializeField] private float catScale = 1.0f;

        [Header("UI Settings")]
        [SerializeField] private Color uiTextColor = Color.yellow;
        [SerializeField] private int titleFontSize = 32;
        [SerializeField] private int countFontSize = 24;

        private GameObject catObject;
        private GameObject uiCanvas;

        private void Start()
        {
            Debug.Log("TapCat2D auto setup starting...");
            AutoSetupCompleteGame();
            Debug.Log("TapCat2D auto setup complete.");
        }

        private void AutoSetupCompleteGame()
        {
            if (autoSetupCamera)
            {
                Setup2DCamera();
            }

            if (autoCreateCat)
            {
                CreateCatObject();
            }

            if (autoCreateUI)
            {
                CreateUInterface();
            }

            ConnectAllComponents();

            if (createPlaceholdersIfNeeded)
            {
                CheckResources();
            }
        }

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

            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5.0f;
            mainCamera.transform.position = new Vector3(0, 0, -10);
            mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.2f);
        }

        private void CreateCatObject()
        {
            catObject = new GameObject("TapCat2D");
            catObject.transform.position = catPosition;
            catObject.transform.localScale = Vector3.one * catScale;

            SpriteRenderer spriteRenderer = catObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingLayerName = "Default";
            spriteRenderer.sortingOrder = 0;

            TapCat2D tapCat2D = catObject.AddComponent<TapCat2D>();
            tapCat2D.SetFrameRate(0.1f);
        }

        private void CreateUInterface()
        {
            uiCanvas = new GameObject("TapCat2D_Canvas");
            Canvas canvas = uiCanvas.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiCanvas.AddComponent<CanvasScaler>();
            uiCanvas.AddComponent<GraphicRaycaster>();

            CreateTextElement("TitleText", "TapCat 2D", new Vector2(0.5f, 0.9f), new Vector2(400, 50), titleFontSize, TextAnchor.UpperCenter, Color.white);
            CreateTextElement("TapCountText", "Taps: 0", new Vector2(0.05f, 0.85f), new Vector2(300, 40), countFontSize, TextAnchor.UpperLeft, uiTextColor);
            CreateTextElement("StatusText", "Status: Idle\nPress Space or Left Mouse to play\nPress R to reset", new Vector2(0.5f, 0.1f), new Vector2(600, 100), 20, TextAnchor.UpperCenter, Color.green);

            CreateTapHint();
        }

        private GameObject CreateTextElement(string name, string text, Vector2 anchor, Vector2 size, int fontSize, TextAnchor alignment, Color color)
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

        private void CreateTapHint()
        {
            GameObject hintObj = new GameObject("TapHintUI");
            hintObj.transform.SetParent(uiCanvas.transform);

            Image hintImage = hintObj.AddComponent<Image>();
            hintImage.color = new Color(1, 1, 1, 0.3f);

            GameObject hintTextObj = new GameObject("HintText");
            hintTextObj.transform.SetParent(hintObj.transform);

            Text hintText = hintTextObj.AddComponent<Text>();
            hintText.text = "Tap here";
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
            hintTextRect.anchorMin = Vector2.zero;
            hintTextRect.anchorMax = Vector2.one;
            hintTextRect.sizeDelta = Vector2.zero;
            hintTextRect.anchoredPosition = Vector2.zero;
        }

        private void ConnectAllComponents()
        {
            if (catObject == null || uiCanvas == null)
            {
                return;
            }

            TapCat2D tapCat2D = catObject.GetComponent<TapCat2D>();
            if (tapCat2D == null)
            {
                return;
            }

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

            tapCat2D.SetUIRefs(tapCountText, statusText, tapHintUI);
        }

        private void CheckResources()
        {
            CreatePlaceholderSprites placeholderCreator = FindObjectOfType<CreatePlaceholderSprites>();
            if (placeholderCreator == null)
            {
                GameObject placeholderObj = new GameObject("PlaceholderCreator");
                placeholderCreator = placeholderObj.AddComponent<CreatePlaceholderSprites>();
            }

            placeholderCreator.CheckAndCreatePlaceholders();
        }

        [ContextMenu("Run Auto Setup")]
        private void RunAutoSetupManual()
        {
            if (Application.isPlaying)
            {
                AutoSetupCompleteGame();
            }
            else
            {
                Debug.Log("Run this in Play Mode.");
            }
        }

        [ContextMenu("Test Game")]
        private void TestGameFunction()
        {
            if (!Application.isPlaying)
            {
                Debug.Log("Run this in Play Mode.");
                return;
            }

            if (catObject == null)
            {
                Debug.LogError("Cat object not created.");
                return;
            }

            TapCat2D tapCat2D = catObject.GetComponent<TapCat2D>();
            if (tapCat2D != null)
            {
                tapCat2D.StartCatAnimation();
                Invoke("TestResetFunction", 1.5f);
            }
        }

        private void TestResetFunction()
        {
            if (catObject == null)
            {
                return;
            }

            TapCat2D tapCat2D = catObject.GetComponent<TapCat2D>();
            if (tapCat2D != null)
            {
                tapCat2D.ResetGame();
            }
        }

        [ContextMenu("Show Controls")]
        private void ShowControlInstructions()
        {
            string instructions = "TapCat 2D Controls:\n- Space / Left Mouse: Play animation\n- R: Reset";
            Debug.Log(instructions);
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (catObject != null)
            {
                Destroy(catObject);
            }

            if (uiCanvas != null)
            {
                Destroy(uiCanvas);
            }
        }
    }
}
