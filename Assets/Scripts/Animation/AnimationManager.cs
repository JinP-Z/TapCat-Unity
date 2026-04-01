using System;
using UnityEngine;
using TapCat.Input;

namespace TapCat.Animation
{
    /// <summary>
    /// Sprite sequence animation manager driven by input events.
    /// </summary>
    public class AnimationManager : MonoBehaviour
    {
        public event Action<int> OnFrameChanged;
        public event Action<int> OnLoopCompleted;
        public event Action<string> OnStatusChanged;

        [Header("Resources")]
        [SerializeField] private string spriteBaseName = "cat_anim_";
        [SerializeField] private int totalFrames = 10;
        [SerializeField] private SpriteRenderer catRenderer;

        [Header("Playback")]
        [SerializeField] private bool playOnInput = true;
        [SerializeField] private bool loopAnimation = true;

        [Header("State")]
        [SerializeField] private int currentFrameIndex = 0;
        [SerializeField] private int completedLoops = 0;
        [SerializeField] private bool isPlaying = false;

        private Sprite[] animationFrames;
        private InputManager inputManager;
        private bool hasValidFrames;

        public int CurrentFrameIndex => currentFrameIndex;
        public int CompletedLoops => completedLoops;
        public bool IsPlaying => isPlaying;
        public int TotalFrames => totalFrames;
        public bool HasValidFrames => hasValidFrames;
        public string StatusInfo => $"Loop {completedLoops}, Frame {currentFrameIndex + 1}/{totalFrames}";

        private void Awake()
        {
            InitializeComponents();
            LoadAnimationFrames();
            SetupInputConnection();
            ApplyFirstFrame();
        }

        private void InitializeComponents()
        {
            if (catRenderer == null)
            {
                catRenderer = GetComponent<SpriteRenderer>();
                if (catRenderer == null)
                {
                    catRenderer = gameObject.AddComponent<SpriteRenderer>();
                    Debug.LogWarning("AnimationManager: Added SpriteRenderer component.");
                }
            }

            inputManager = FindObjectOfType<InputManager>();
            if (inputManager == null)
            {
                Debug.LogError("AnimationManager: InputManager not found in scene.");
            }
        }

        private void LoadAnimationFrames()
        {
            totalFrames = Mathf.Max(1, totalFrames);
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
                    Debug.LogWarning($"AnimationManager: Failed to load frame {frameName}.");
                }
            }

            hasValidFrames = loadedFrames > 0;

            if (hasValidFrames)
            {
                Debug.Log($"AnimationManager: Loaded {loadedFrames}/{totalFrames} frames.");
                OnStatusChanged?.Invoke($"Loaded {loadedFrames}/{totalFrames} frames.");
            }
            else
            {
                Debug.LogError("AnimationManager: No animation frames loaded.");
                OnStatusChanged?.Invoke("Error: No animation frames loaded.");
            }
        }

        private void SetupInputConnection()
        {
            if (inputManager != null && playOnInput)
            {
                inputManager.OnInputTriggered += PlayNextFrame;
                Debug.Log("AnimationManager: Connected to input system.");
            }
        }

        private void ApplyFirstFrame()
        {
            if (hasValidFrames && catRenderer != null)
            {
                catRenderer.sprite = animationFrames[0];
                currentFrameIndex = 0;
                isPlaying = false;
                OnFrameChanged?.Invoke(0);
                OnStatusChanged?.Invoke("Idle");
            }
        }

        public void PlayNextFrame()
        {
            if (!hasValidFrames || animationFrames == null)
            {
                Debug.LogWarning("AnimationManager: No valid frames to play.");
                return;
            }

            currentFrameIndex++;

            if (currentFrameIndex >= totalFrames)
            {
                completedLoops++;
                currentFrameIndex = 0;
                OnLoopCompleted?.Invoke(completedLoops);

                if (!loopAnimation)
                {
                    isPlaying = false;
                    OnStatusChanged?.Invoke($"Loop completed {completedLoops} time(s). Stopped.");
                    return;
                }
            }

            if (catRenderer != null)
            {
                catRenderer.sprite = animationFrames[currentFrameIndex];
            }

            isPlaying = true;
            OnFrameChanged?.Invoke(currentFrameIndex);
            OnStatusChanged?.Invoke(StatusInfo);
            Debug.Log($"AnimationManager: Frame {currentFrameIndex + 1}/{totalFrames}, Loop {completedLoops}.");
        }

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
            OnStatusChanged?.Invoke("Reset");

            Debug.Log("AnimationManager: Animation reset.");
        }

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

        public string GetAnimationInfo()
        {
            string playState = isPlaying ? "Playing" : "Idle";
            return $"Frame {currentFrameIndex + 1}/{totalFrames}, Loops: {completedLoops}, State: {playState}";
        }

        private void OnDestroy()
        {
            if (inputManager != null)
            {
                inputManager.OnInputTriggered -= PlayNextFrame;
            }
        }

        [ContextMenu("Debug Play Next Frame")]
        private void DebugPlayNextFrame()
        {
            PlayNextFrame();
        }

        [ContextMenu("Debug Reset Animation")]
        private void DebugResetAnimation()
        {
            ResetAnimation();
        }
    }
}
