using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TapCat
{
    /// <summary>
    /// TapCat 2D animation controller using sprite frame sequences.
    /// </summary>
    public class TapCat2D : MonoBehaviour
    {
        [Header("Sprite Animation Settings")]
        [SerializeField] private Sprite[] catAnimationFrames;
        [SerializeField] private float frameRate = 0.1f;
        [SerializeField] private SpriteRenderer catSpriteRenderer;

        [Header("UI Display")]
        [SerializeField] private Text tapCountText;
        [SerializeField] private Text statusText;
        [SerializeField] private GameObject tapHintUI;

        [Header("Debug Settings")]
        [SerializeField] private bool showDebugLogs = true;

        [Header("Game State")]
        [SerializeField] private int tapCount = 0;
        [SerializeField] private bool isAnimating = false;
        [SerializeField] private bool isInitialized = false;

        private Coroutine animationCoroutine;
        private int currentFrameIndex = 0;

        private KeyCode tapKey = KeyCode.Space;
        private KeyCode resetKey = KeyCode.R;

        private void Start()
        {
            InitializeComponents();
            SetupUI();
            UpdateStatusDisplay();
        }

        private void InitializeComponents()
        {
            if (catSpriteRenderer == null)
            {
                catSpriteRenderer = GetComponent<SpriteRenderer>();
                if (catSpriteRenderer == null)
                {
                    catSpriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                    Debug.Log("SpriteRenderer added.");
                }
            }

            if (catAnimationFrames == null || catAnimationFrames.Length == 0)
            {
                Debug.LogWarning("Cat animation frames are empty. Assign cat_anim_00.png to cat_anim_09.png.");
            }
            else if (catAnimationFrames.Length < 10)
            {
                Debug.LogWarning($"Cat animation frames are fewer than 10. Current: {catAnimationFrames.Length}.");
            }
            else
            {
                Debug.Log($"Loaded {catAnimationFrames.Length} cat animation frames.");
            }

            isInitialized = true;
        }

        private void SetupUI()
        {
            if (tapCountText == null)
            {
                GameObject tapCountObj = GameObject.Find("TapCountText");
                if (tapCountObj != null)
                {
                    tapCountText = tapCountObj.GetComponent<Text>();
                }
            }

            if (statusText == null)
            {
                GameObject statusObj = GameObject.Find("StatusText");
                if (statusObj != null)
                {
                    statusText = statusObj.GetComponent<Text>();
                }
            }

            if (tapHintUI == null)
            {
                tapHintUI = GameObject.Find("TapHintUI");
            }

            UpdateTapCountDisplay();
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            if (Input.GetKeyDown(tapKey) && !isAnimating)
            {
                StartCatAnimation();
            }

            if (Input.GetMouseButtonDown(0) && !isAnimating)
            {
                StartCatAnimation();
            }

            if (Input.GetKeyDown(resetKey))
            {
                ResetGame();
            }
        }

        public void StartCatAnimation()
        {
            if (!isInitialized || isAnimating)
            {
                return;
            }

            tapCount++;
            UpdateTapCountDisplay();

            isAnimating = true;

            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            animationCoroutine = StartCoroutine(PlayCatAnimation());

            UpdateStatusDisplay();

            if (showDebugLogs)
            {
                Debug.Log($"Playing cat animation. Tap count: {tapCount}");
            }
        }

        private IEnumerator PlayCatAnimation()
        {
            if (catAnimationFrames == null || catAnimationFrames.Length == 0)
            {
                Debug.LogError("Cannot play animation: frames are missing.");
                isAnimating = false;
                yield break;
            }

            for (int i = 0; i < catAnimationFrames.Length; i++)
            {
                currentFrameIndex = i;
                catSpriteRenderer.sprite = catAnimationFrames[i];
                yield return new WaitForSeconds(frameRate);
            }

            isAnimating = false;
            UpdateStatusDisplay();

            if (showDebugLogs)
            {
                Debug.Log("Cat animation completed.");
            }
        }

        public void ResetGame()
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
                animationCoroutine = null;
            }

            isAnimating = false;
            tapCount = 0;
            currentFrameIndex = 0;

            if (catAnimationFrames != null && catAnimationFrames.Length > 0)
            {
                catSpriteRenderer.sprite = catAnimationFrames[0];
            }

            UpdateTapCountDisplay();
            UpdateStatusDisplay();

            if (showDebugLogs)
            {
                Debug.Log("Game reset.");
            }
        }

        private void UpdateTapCountDisplay()
        {
            if (tapCountText != null)
            {
                tapCountText.text = $"Taps: {tapCount}";
            }
        }

        private void UpdateStatusDisplay()
        {
            if (statusText != null)
            {
                string status = isAnimating ? "Playing..." : "Idle";
                statusText.text = $"Status: {status}\nPress Space or Left Mouse to play\nPress R to reset";
            }

            if (tapHintUI != null)
            {
                tapHintUI.SetActive(!isAnimating);
            }
        }

        public int GetTapCount()
        {
            return tapCount;
        }

        public bool IsAnimating()
        {
            return isAnimating;
        }

        public void SetFrameRate(float newFrameRate)
        {
            frameRate = Mathf.Clamp(newFrameRate, 0.01f, 1.0f);
            Debug.Log($"Frame rate set to {frameRate} seconds per frame.");
        }

        public void SetAnimationFrames(Sprite[] frames)
        {
            catAnimationFrames = frames;
            if (frames != null && frames.Length > 0 && catSpriteRenderer != null && !isAnimating)
            {
                catSpriteRenderer.sprite = frames[0];
            }
        }

        public void SetUIRefs(Text tapCountTextRef, Text statusTextRef, GameObject tapHintUIRef)
        {
            tapCountText = tapCountTextRef;
            statusText = statusTextRef;
            tapHintUI = tapHintUIRef;

            UpdateTapCountDisplay();
            UpdateStatusDisplay();
        }

        [ContextMenu("Test Animation")]
        private void TestAnimation()
        {
            if (Application.isPlaying)
            {
                StartCatAnimation();
            }
            else
            {
                Debug.Log("Please test animation in Play Mode.");
            }
        }

        [ContextMenu("Test Reset")]
        private void TestReset()
        {
            if (Application.isPlaying)
            {
                ResetGame();
            }
            else
            {
                Debug.Log("Please test reset in Play Mode.");
            }
        }
    }
}
