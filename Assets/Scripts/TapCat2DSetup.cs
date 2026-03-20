using UnityEngine;
using UnityEngine.UI;

namespace TapCat
{
    /// <summary>
    /// Auto setup script for TapCat 2D. Creates the game on Play with no manual setup.
    /// </summary>
    public class TapCat2DSetup : MonoBehaviour
    {
        [Header("Prefabs (Optional)")]
        [SerializeField] private GameObject tapCat2DPrefab;
        [SerializeField] private GameObject uiCanvasPrefab;

        [Header("Cat Animation Frames")]
        [SerializeField] private Sprite[] catAnimationSprites;

        [Header("Spawn Position")]
        [SerializeField] private Vector3 catPosition = new Vector3(0, 0, 0);

        private GameObject currentCat;
        private GameObject currentCanvas;

        private void Start()
        {
            SetupGame();
        }

        private void SetupGame()
        {
            CreateUICanvas();
            CreateCatObject();
            ConnectUIAndCat();
            SetupCamera();

            Debug.Log("TapCat 2D setup complete. Press Space or Left Mouse to play. Press R to reset.");
        }

        private void CreateUICanvas()
        {
            if (uiCanvasPrefab != null)
            {
                currentCanvas = Instantiate(uiCanvasPrefab);
                currentCanvas.name = "TapCat2D_UI";
            }
            else
            {
                currentCanvas = new GameObject("TapCat2D_UI");
                Canvas canvas = currentCanvas.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                currentCanvas.AddComponent<CanvasScaler>();
                currentCanvas.AddComponent<GraphicRaycaster>();

                CreateUIElements();
            }
        }

        private void CreateUIElements()
        {
            GameObject titleObj = new GameObject("TitleText");
            titleObj.transform.SetParent(currentCanvas.transform);
            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = "TapCat 2D";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 32;
            titleText.alignment = TextAnchor.UpperCenter;
            titleText.color = Color.white;

            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.9f);
            titleRect.anchorMax = new Vector2(0.5f, 0.9f);
            titleRect.pivot = new Vector2(0.5f, 0.5f);
            titleRect.sizeDelta = new Vector2(400, 50);
            titleRect.anchoredPosition = Vector2.zero;

            GameObject countObj = new GameObject("TapCountText");
            countObj.transform.SetParent(currentCanvas.transform);
            Text countText = countObj.AddComponent<Text>();
            countText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            countText.fontSize = 24;
            countText.alignment = TextAnchor.UpperLeft;
            countText.color = Color.yellow;

            RectTransform countRect = countObj.GetComponent<RectTransform>();
            countRect.anchorMin = new Vector2(0.05f, 0.85f);
            countRect.anchorMax = new Vector2(0.05f, 0.85f);
            countRect.pivot = new Vector2(0, 0.5f);
            countRect.sizeDelta = new Vector2(300, 40);
            countRect.anchoredPosition = Vector2.zero;

            GameObject statusObj = new GameObject("StatusText");
            statusObj.transform.SetParent(currentCanvas.transform);
            Text statusText = statusObj.AddComponent<Text>();
            statusText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            statusText.fontSize = 20;
            statusText.alignment = TextAnchor.UpperCenter;
            statusText.color = Color.green;

            RectTransform statusRect = statusObj.GetComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0.5f, 0.1f);
            statusRect.anchorMax = new Vector2(0.5f, 0.1f);
            statusRect.pivot = new Vector2(0.5f, 0.5f);
            statusRect.sizeDelta = new Vector2(600, 100);
            statusRect.anchoredPosition = Vector2.zero;

            GameObject hintObj = new GameObject("TapHintUI");
            hintObj.transform.SetParent(currentCanvas.transform);
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
            hintTextRect.anchorMin = new Vector2(0, 0);
            hintTextRect.anchorMax = new Vector2(1, 1);
            hintTextRect.sizeDelta = Vector2.zero;
            hintTextRect.anchoredPosition = Vector2.zero;
        }

        private void CreateCatObject()
        {
            if (tapCat2DPrefab != null)
            {
                currentCat = Instantiate(tapCat2DPrefab, catPosition, Quaternion.identity);
                currentCat.name = "TapCat2D";
            }
            else
            {
                currentCat = new GameObject("TapCat2D");
                currentCat.transform.position = catPosition;

                SpriteRenderer spriteRenderer = currentCat.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingLayerName = "Default";
                spriteRenderer.sortingOrder = 0;

                TapCat2D tapCat2D = currentCat.AddComponent<TapCat2D>();

                if (catAnimationSprites != null && catAnimationSprites.Length > 0)
                {
                    tapCat2D.SetAnimationFrames(catAnimationSprites);
                }
            }
        }

        private void ConnectUIAndCat()
        {
            if (currentCat == null || currentCanvas == null)
            {
                return;
            }

            TapCat2D tapCat2D = currentCat.GetComponent<TapCat2D>();
            if (tapCat2D == null)
            {
                return;
            }

            Text tapCountText = null;
            Text statusText = null;
            GameObject tapHintUI = null;

            foreach (Transform child in currentCanvas.transform)
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

        private void SetupCamera()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                GameObject cameraObj = new GameObject("MainCamera");
                mainCamera = cameraObj.AddComponent<Camera>();
                cameraObj.AddComponent<AudioListener>();
                cameraObj.tag = "MainCamera";
            }

            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5;
            mainCamera.transform.position = new Vector3(0, 0, -10);
            mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.2f);
        }

        private void OnDestroy()
        {
            if (currentCat != null)
            {
                Destroy(currentCat);
            }

            if (currentCanvas != null)
            {
                Destroy(currentCanvas);
            }
        }

        [ContextMenu("Test Setup")]
        private void TestSetup()
        {
            if (Application.isPlaying)
            {
                SetupGame();
            }
            else
            {
                Debug.Log("Please test setup in Play Mode.");
            }
        }
    }
}
