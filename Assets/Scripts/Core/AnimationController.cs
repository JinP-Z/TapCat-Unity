using System;
using System.Collections;
using UnityEngine;
using TapCat.Utils;

namespace TapCat.Core
{
    /// <summary>
    /// Controls sprite sequence animation playback (frame-advance per input).
    /// Performance & lifecycle: loads frames once on startup, no Update loop needed,
    /// only reacts to input events, no per-frame allocations.
    /// </summary>
    [DisallowMultipleComponent]
    public class AnimationController : MonoBehaviour
    {
        public event Action<int> OnFrameChanged;
        public event Action<int> OnLoopCompleted;
        public event Action<string> OnStatusChanged;

        [Header("Resource Loading")]
        [SerializeField] private string resourceBasePath = ResourceLoader.DefaultBasePath;
        [SerializeField, Range(1, 30)] private int frameCount = 10;
        [SerializeField] private bool loadOnAwake = true;

        [Header("Playback")]
        [SerializeField] private bool loopAnimation = true;

        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLog = false;

        private Sprite[] frames;
        private int currentFrameIndex;
        private int completedLoops;
        private bool hasValidFrames;
        private AnimationState currentState = AnimationState.Idle;
        private bool isLoading;

        public int CurrentFrameIndex => currentFrameIndex;
        public int CompletedLoops => completedLoops;
        public int TotalFrames => frameCount;
        public bool HasValidFrames => hasValidFrames;
        public AnimationState CurrentState => currentState;

        public string StatusInfo => string.Format("循环{0}，帧{1}/{2}", completedLoops, currentFrameIndex + 1, frameCount);

        private void Awake()
        {
            EnsureSpriteRenderer();
            DisableAnimator();

            if (loadOnAwake)
            {
                StartCoroutine(LoadFramesAsync());
            }
        }

        /// <summary>
        /// Advance one frame in response to input.
        /// </summary>
        public void PlayNextFrame()
        {
            if (currentState == AnimationState.Cooldown)
            {
                return;
            }

            if (!hasValidFrames || frames == null || frames.Length == 0)
            {
                if (enableDebugLog)
                {
                    Debug.LogWarning("AnimationController: No valid frames to play.");
                }
                return;
            }

            currentState = AnimationState.Playing;

            int nextFrame = currentFrameIndex + 1;
            if (nextFrame >= frameCount)
            {
                completedLoops++;
                currentFrameIndex = 0;
                OnLoopCompleted?.Invoke(completedLoops);

                if (!loopAnimation)
                {
                    ApplyFrame(currentFrameIndex);
                    OnFrameChanged?.Invoke(currentFrameIndex);
                    currentState = AnimationState.Idle;
                    OnStatusChanged?.Invoke(StatusInfo);
                    return;
                }
            }
            else
            {
                currentFrameIndex = nextFrame;
            }

            ApplyFrame(currentFrameIndex);
            OnFrameChanged?.Invoke(currentFrameIndex);
            OnStatusChanged?.Invoke(StatusInfo);
            currentState = AnimationState.Idle;
        }

        /// <summary>
        /// Reset animation state to the first frame.
        /// </summary>
        public void ResetAnimation()
        {
            currentFrameIndex = 0;
            completedLoops = 0;
            currentState = AnimationState.Idle;

            ApplyFrame(currentFrameIndex);
            OnFrameChanged?.Invoke(currentFrameIndex);
            OnStatusChanged?.Invoke(StatusInfo);
        }

        /// <summary>
        /// Set the cooldown state (inputs ignored while in cooldown).
        /// </summary>
        public void SetCooldownState(bool isCooldown)
        {
            if (isCooldown)
            {
                currentState = AnimationState.Cooldown;
                return;
            }

            if (currentState == AnimationState.Cooldown)
            {
                currentState = AnimationState.Idle;
            }
        }

        /// <summary>
        /// Synchronously load frames (useful for tests or manual initialization).
        /// </summary>
        public bool LoadFramesSync()
        {
            string errorMessage;
            int loadedCount;
            frames = ResourceLoader.LoadSpritesSync(resourceBasePath, frameCount, out loadedCount, out errorMessage);
            hasValidFrames = loadedCount > 0 && frames != null && frames.Length > 0 && frames[0] != null;

            if (!string.IsNullOrEmpty(errorMessage) && enableDebugLog)
            {
                Debug.LogWarning(errorMessage);
            }

            ApplyFrame(0);
            currentFrameIndex = 0;
            completedLoops = 0;
            currentState = AnimationState.Idle;
            return hasValidFrames;
        }

        /// <summary>
        /// Test helper: inject frames without touching Resources.
        /// </summary>
        public void SetFramesForTesting(Sprite[] testFrames)
        {
            frames = testFrames;
            frameCount = frames != null ? Mathf.Max(1, frames.Length) : 1;
            hasValidFrames = frames != null && frames.Length > 0 && frames[0] != null;
            currentFrameIndex = 0;
            completedLoops = 0;
            currentState = AnimationState.Idle;
            ApplyFrame(0);
        }

        private IEnumerator LoadFramesAsync()
        {
            if (isLoading)
            {
                yield break;
            }

            isLoading = true;
            yield return ResourceLoader.LoadSpritesAsync(resourceBasePath, frameCount, loadedFrames =>
            {
                frames = loadedFrames;
                hasValidFrames = frames != null && frames.Length > 0 && frames[0] != null;
                ApplyFrame(0);
                currentFrameIndex = 0;
                completedLoops = 0;
                currentState = AnimationState.Idle;
                OnFrameChanged?.Invoke(currentFrameIndex);
                OnStatusChanged?.Invoke(StatusInfo);
            }, error =>
            {
                if (enableDebugLog)
                {
                    Debug.LogWarning(error);
                }
            });

            isLoading = false;
        }

        private void ApplyFrame(int frameIndex)
        {
            if (spriteRenderer == null)
            {
                return;
            }

            if (frames == null || frames.Length == 0)
            {
                spriteRenderer.sprite = null;
                return;
            }

            int clampedIndex = Mathf.Clamp(frameIndex, 0, frames.Length - 1);
            Sprite frame = frames[clampedIndex];
            spriteRenderer.sprite = frame != null ? frame : frames[0];
        }

        private void EnsureSpriteRenderer()
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
                if (spriteRenderer == null)
                {
                    spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
                }
            }
        }

        private void DisableAnimator()
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false;
            }
        }
    }
}

