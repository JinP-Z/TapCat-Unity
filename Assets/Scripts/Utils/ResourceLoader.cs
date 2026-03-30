using System;
using System.Collections;
using UnityEngine;

namespace TapCat.Utils
{
    /// <summary>
    /// Loads TapCat animation sprites from Resources.
    /// Performance & lifecycle: load once during initialization, cache sprites in memory,
    /// no per-frame allocations, no Update usage.
    /// Resources Exception: required by current spec; migrate to Addressables in a later phase.
    /// </summary>
    public static class ResourceLoader
    {
        public const string DefaultBasePath = "CatAnimation/cat_anim_";
        public const string PlaceholderResourcePath = "PlaceholderCat";

        /// <summary>
        /// Async load sprites via Resources.LoadAsync. Calls onLoaded with a cached sprite array.
        /// </summary>
        public static IEnumerator LoadSpritesAsync(string basePath, int frameCount, Action<Sprite[]> onLoaded, Action<string> onError = null)
        {
            string normalized = NormalizeResourceBasePath(basePath);
            int safeCount = Mathf.Max(1, frameCount);
            Sprite[] frames = new Sprite[safeCount];

            int loaded = 0;
            for (int i = 0; i < safeCount; i++)
            {
                string path = $"{normalized}{i:00}";
                ResourceRequest request = Resources.LoadAsync<Sprite>(path);
                yield return request;

                Sprite sprite = request.asset as Sprite;
                if (sprite != null)
                {
                    frames[i] = sprite;
                    loaded++;
                }
            }

            FinalizeFrames(normalized, frames, ref loaded, onError);
            onLoaded?.Invoke(frames);
        }

        /// <summary>
        /// Sync load sprites via Resources.Load. Returns cached sprite array.
        /// </summary>
        public static Sprite[] LoadSpritesSync(string basePath, int frameCount, out int loadedCount, out string errorMessage)
        {
            string normalized = NormalizeResourceBasePath(basePath);
            int safeCount = Mathf.Max(1, frameCount);
            Sprite[] frames = new Sprite[safeCount];

            int loaded = 0;
            for (int i = 0; i < safeCount; i++)
            {
                string path = $"{normalized}{i:00}";
                Sprite sprite = Resources.Load<Sprite>(path);
                if (sprite != null)
                {
                    frames[i] = sprite;
                    loaded++;
                }
            }

            errorMessage = null;
            FinalizeFrames(normalized, frames, ref loaded, msg => errorMessage = msg);
            loadedCount = CountNonNull(frames);
            return frames;
        }

        /// <summary>
        /// Loads the placeholder sprite if available.
        /// </summary>
        public static Sprite LoadPlaceholderSprite()
        {
            return Resources.Load<Sprite>(PlaceholderResourcePath);
        }

        private static void FinalizeFrames(string normalizedBasePath, Sprite[] frames, ref int loaded, Action<string> onError)
        {
            if (loaded == 0)
            {
                Sprite placeholder = LoadPlaceholderSprite();
                if (placeholder != null)
                {
                    for (int i = 0; i < frames.Length; i++)
                    {
                        frames[i] = placeholder;
                    }
                    loaded = frames.Length;
                }
                else
                {
                    onError?.Invoke($"ResourceLoader: Failed to load frames from Resources/{normalizedBasePath}** and no placeholder found.");
                }

                return;
            }

            if (loaded < frames.Length)
            {
                Sprite fallback = GetFirstValid(frames);
                if (fallback != null)
                {
                    for (int i = 0; i < frames.Length; i++)
                    {
                        if (frames[i] == null)
                        {
                            frames[i] = fallback;
                        }
                    }
                }
            }
        }

        private static Sprite GetFirstValid(Sprite[] frames)
        {
            if (frames == null)
            {
                return null;
            }

            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null)
                {
                    return frames[i];
                }
            }

            return null;
        }

        private static int CountNonNull(Sprite[] frames)
        {
            if (frames == null)
            {
                return 0;
            }

            int count = 0;
            for (int i = 0; i < frames.Length; i++)
            {
                if (frames[i] != null)
                {
                    count++;
                }
            }

            return count;
        }

        private static string NormalizeResourceBasePath(string basePath)
        {
            if (string.IsNullOrWhiteSpace(basePath))
            {
                return DefaultBasePath;
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
    }
}
