using UnityEngine;

namespace TapCat
{
    /// <summary>
    /// Sprite sequence animator for TapCat using Resources/CatAnimation frames.
    /// </summary>
    public class TapCatSpriteSequenceAnimator : MonoBehaviour
    {
        [Header("Resource Loading")]
        [SerializeField] private string resourceBasePath = "CatAnimation/cat_anim_";
        [SerializeField] private int frameCount = 10;

        [Header("Playback")]
        [SerializeField] private float secondsPerFrame = 0.1f;
        [SerializeField] private bool loop = false;
        [SerializeField] private bool handleOwnInput = false;

        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Sprite[] frames;
        private int currentFrame;
        private float timer;
        private bool isPlaying;

        private void Awake()
        {
            EnsureSpriteRenderer();
            DisableAnimator();
            LoadFramesFromResources();
            ApplyFirstFrame();
            EnsureClickCollider();
        }

        private void Update()
        {
            if (handleOwnInput)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    PlayTapAnimation();
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    ResetAnimation();
                }
            }

            StepAnimation();
        }

        public void PlayTapAnimation()
        {
            if (!HasValidFrames())
            {
                Debug.LogWarning("TapCatSpriteSequenceAnimator: No valid frames to play. Check Resources/CatAnimation.");
                return;
            }

            isPlaying = true;
            currentFrame = 0;
            timer = 0f;
            spriteRenderer.sprite = frames[0];
        }

        public void ResetAnimation()
        {
            isPlaying = false;
            currentFrame = 0;
            timer = 0f;
            ApplyFirstFrame();
        }

        public void SetExternalInputMode(bool useExternalInput)
        {
            handleOwnInput = !useExternalInput;
        }

        public bool IsPlaying()
        {
            return isPlaying;
        }

        private void StepAnimation()
        {
            if (!isPlaying || frames == null || frames.Length == 0)
            {
                return;
            }

            timer += Time.deltaTime;
            if (timer < secondsPerFrame)
            {
                return;
            }

            timer = 0f;
            currentFrame++;

            if (currentFrame >= frames.Length || frames[currentFrame] == null)
            {
                if (loop)
                {
                    currentFrame = 0;
                    spriteRenderer.sprite = frames[0];
                }
                else
                {
                    isPlaying = false;
                    currentFrame = 0;
                    ApplyFirstFrame();
                }

                return;
            }

            spriteRenderer.sprite = frames[currentFrame];
        }

        private void LoadFramesFromResources()
        {
            frames = new Sprite[Mathf.Max(1, frameCount)];
            int loaded = 0;

            for (int i = 0; i < frameCount; i++)
            {
                string path = $"{resourceBasePath}{i:00}";
                Sprite frame = Resources.Load<Sprite>(path);
                if (frame != null)
                {
                    frames[i] = frame;
                    loaded++;
                }
            }

            if (loaded == 0)
            {
                Debug.LogError("TapCatSpriteSequenceAnimator: Failed to load frames. Expected Resources/CatAnimation/cat_anim_00.png ~ cat_anim_09.png.");
            }
            else if (loaded < frameCount)
            {
                Debug.LogWarning($"TapCatSpriteSequenceAnimator: Loaded {loaded}/{frameCount} frames. Missing frames will end playback.");
            }
        }

        private void ApplyFirstFrame()
        {
            if (spriteRenderer == null)
            {
                return;
            }

            if (HasValidFrames())
            {
                spriteRenderer.sprite = frames[0];
            }
            else
            {
                spriteRenderer.sprite = null;
            }
        }

        private bool HasValidFrames()
        {
            return frames != null && frames.Length > 0 && frames[0] != null;
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

        private void EnsureClickCollider()
        {
            if (GetComponent<Collider>() != null || GetComponent<Collider2D>() != null)
            {
                return;
            }

            BoxCollider2D collider2D = gameObject.AddComponent<BoxCollider2D>();
            collider2D.offset = Vector2.zero;
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
