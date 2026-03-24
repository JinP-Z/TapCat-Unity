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
            string normalizedBasePath = NormalizeResourceBasePath(resourceBasePath);
            int loaded = LoadFramesByDirectPath(normalizedBasePath);

            if (loaded == 0)
            {
                loaded = LoadFramesByFolderLookup(normalizedBasePath);
            }

            if (loaded == 0)
            {
                loaded = LoadFramesFromTextures(normalizedBasePath);
            }

            if (loaded == 0)
            {
                Debug.LogError($"TapCatSpriteSequenceAnimator: Failed to load frames. Expected Resources/{normalizedBasePath}00 ~ {normalizedBasePath}{frameCount - 1:00}.");
            }
            else if (loaded < frameCount)
            {
                Debug.LogWarning($"TapCatSpriteSequenceAnimator: Loaded {loaded}/{frameCount} frames from Resources/{normalizedBasePath}**. Missing frames will end playback.");
            }
        }

        private int LoadFramesByDirectPath(string normalizedBasePath)
        {
            int loaded = 0;

            for (int i = 0; i < frameCount; i++)
            {
                string path = $"{normalizedBasePath}{i:00}";
                Sprite frame = Resources.Load<Sprite>(path);
                if (frame != null)
                {
                    frames[i] = frame;
                    loaded++;
                }
            }

            return loaded;
        }

        private int LoadFramesByFolderLookup(string normalizedBasePath)
        {
            GetFolderAndPrefix(normalizedBasePath, out string folderPath, out string namePrefix);
            Sprite[] sprites = Resources.LoadAll<Sprite>(folderPath);
            if (sprites == null || sprites.Length == 0)
            {
                return 0;
            }

            int loaded = 0;
            for (int i = 0; i < frameCount; i++)
            {
                string spriteName = $"{namePrefix}{i:00}";
                for (int s = 0; s < sprites.Length; s++)
                {
                    Sprite sprite = sprites[s];
                    if (sprite != null && sprite.name == spriteName)
                    {
                        frames[i] = sprite;
                        loaded++;
                        break;
                    }
                }
            }

            return loaded;
        }

        private int LoadFramesFromTextures(string normalizedBasePath)
        {
            GetFolderAndPrefix(normalizedBasePath, out string folderPath, out string namePrefix);
            Texture2D[] textures = Resources.LoadAll<Texture2D>(folderPath);
            if (textures == null || textures.Length == 0)
            {
                return 0;
            }

            int loaded = 0;
            for (int i = 0; i < frameCount; i++)
            {
                string textureName = $"{namePrefix}{i:00}";
                for (int t = 0; t < textures.Length; t++)
                {
                    Texture2D texture = textures[t];
                    if (texture != null && texture.name == textureName)
                    {
                        frames[i] = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f);
                        loaded++;
                        break;
                    }
                }
            }

            return loaded;
        }

        private static string NormalizeResourceBasePath(string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                return "CatAnimation/cat_anim_";
            }

            string normalized = basePath.Replace("\\", "/").Trim();
            if (normalized.StartsWith("Assets/Resources/"))
            {
                normalized = normalized.Substring("Assets/Resources/".Length);
            }
            else if (normalized.StartsWith("Resources/"))
            {
                normalized = normalized.Substring("Resources/".Length);
            }

            return normalized.TrimStart('/');
        }

        private static void GetFolderAndPrefix(string normalizedBasePath, out string folderPath, out string namePrefix)
        {
            string cleaned = normalizedBasePath.Trim('/');
            int slashIndex = cleaned.LastIndexOf('/');
            if (slashIndex >= 0)
            {
                folderPath = cleaned.Substring(0, slashIndex);
                namePrefix = cleaned.Substring(slashIndex + 1);
            }
            else
            {
                folderPath = string.Empty;
                namePrefix = cleaned;
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
