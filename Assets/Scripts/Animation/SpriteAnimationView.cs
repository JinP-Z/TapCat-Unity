using UnityEngine;
using TapCat.Animation;

namespace TapCat.Animation
{
    /// <summary>
    /// Sprite animation view that loads and displays frames from Resources.
    /// 
    /// Resources Exception: 由于技术宪法与规格说明书冲突，暂时使用 Resources.Load
    /// 迁移计划：后续版本迁移到 Addressables
    /// </summary>
    public class SpriteAnimationView : MonoBehaviour
    {
        [Header("Resource Loading")]
        [SerializeField] private string resourceBasePath = "CatAnimation/cat_anim_";
        [SerializeField] private int frameCount = 10;

        [Header("References")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Sprite[] frames;
        private AnimationModule module;

        /// <summary>
        /// Bind this view to an AnimationModule.
        /// </summary>
        public void Bind(AnimationModule animationModule)
        {
            if (module != null)
            {
                module.OnFrameChanged -= OnFrameChanged;
            }

            module = animationModule;
            if (module != null)
            {
                module.OnFrameChanged += OnFrameChanged;
            }
        }

        private void Awake()
        {
            EnsureSpriteRenderer();
            DisableAnimator();
            LoadFramesFromResources();
            ApplyFirstFrame();
        }

        private void Start()
        {
            // If no module is bound yet, display the first frame
            if (module == null)
            {
                ApplyFrame(0);
            }
        }

        private void OnDestroy()
        {
            if (module != null)
            {
                module.OnFrameChanged -= OnFrameChanged;
            }
        }

        private void OnFrameChanged(int frameIndex)
        {
            ApplyFrame(frameIndex);
        }

        private void ApplyFrame(int frameIndex)
        {
            if (spriteRenderer == null)
            {
                return;
            }

            if (!HasValidFrames())
            {
                spriteRenderer.sprite = null;
                return;
            }

            int clampedIndex = Mathf.Clamp(frameIndex, 0, frames.Length - 1);
            Sprite frame = frames[clampedIndex];
            spriteRenderer.sprite = frame != null ? frame : frames[0];
        }

        private bool HasValidFrames()
        {
            return frames != null && frames.Length > 0 && frames[0] != null;
        }

        private void SyncFrameCountToModule()
        {
            if (module != null)
            {
                frameCount = Mathf.Max(1, module.TotalFrames);
            }
            else
            {
                frameCount = Mathf.Max(1, frameCount);
            }
        }

        private void LoadFramesFromResources()
        {
            SyncFrameCountToModule();
            frames = new Sprite[frameCount];
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
                Debug.LogError($"SpriteAnimationView: Failed to load frames. Expected Resources/{normalizedBasePath}00 ~ {normalizedBasePath}{frameCount - 1:00}.");
            }
            else if (loaded < frameCount)
            {
                Debug.LogWarning($"SpriteAnimationView: Loaded {loaded}/{frameCount} frames from Resources/{normalizedBasePath}**. Missing frames will show the first valid frame.");
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

        private void ApplyFirstFrame()
        {
            if (HasValidFrames() && spriteRenderer != null)
            {
                spriteRenderer.sprite = frames[0];
            }
        }
    }
}