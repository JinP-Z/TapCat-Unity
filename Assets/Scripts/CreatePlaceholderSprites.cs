using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace TapCat
{
    /// <summary>
    /// Creates placeholder sprites for cat animation frames when real assets are missing.
    /// </summary>
    public class CreatePlaceholderSprites : MonoBehaviour
    {
        [Header("Placeholder Settings")]
        [SerializeField] private int textureSize = 128;
        [SerializeField] private string outputFolder = "Assets/Sprites/CatAnimation/";
        [SerializeField] private bool createOnStart = true;

        [Header("Frame Colors")]
        [SerializeField] private Color[] frameColors = new Color[]
        {
            Color.red,
            new Color(1, 0.5f, 0),
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            new Color(0.5f, 0, 1),
            Color.magenta,
            Color.white,
            Color.gray
        };

        private void Start()
        {
            if (createOnStart)
            {
                CheckAndCreatePlaceholders();
            }
        }

        public void CheckAndCreatePlaceholders()
        {
            Debug.Log("Checking cat animation frames...");

            if (!Directory.Exists(outputFolder))
            {
                Debug.Log($"Creating folder: {outputFolder}");
                Directory.CreateDirectory(outputFolder);
            }

            bool hasCatFrames = false;
            string[] pngFiles = Directory.GetFiles(outputFolder, "*.png");
            foreach (string file in pngFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.StartsWith("cat_anim_"))
                {
                    hasCatFrames = true;
                    break;
                }
            }

            if (!hasCatFrames)
            {
                Debug.Log("No cat animation frames found. Creating placeholders...");
                CreatePlaceholderTextures();
            }
            else
            {
                Debug.Log("Cat animation frames found. No placeholders needed.");
            }
        }

        private void CreatePlaceholderTextures()
        {
            for (int i = 0; i < 10; i++)
            {
                Texture2D texture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);
                Color color = frameColors[i % frameColors.Length];

                Color[] pixels = new Color[textureSize * textureSize];
                for (int p = 0; p < pixels.Length; p++)
                {
                    int x = p % textureSize;
                    int y = p / textureSize;
                    float centerX = textureSize / 2f;
                    float centerY = textureSize / 2f;
                    float distance = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));
                    float radius = textureSize / 3f;

                    if (distance < radius)
                    {
                        pixels[p] = color;
                    }
                    else if (distance < radius + 2)
                    {
                        pixels[p] = Color.black;
                    }
                    else
                    {
                        pixels[p] = new Color(0.2f, 0.2f, 0.2f, 0.5f);
                    }
                }

                texture.SetPixels(pixels);
                texture.Apply();

                string fileName = $"cat_anim_{i:00}_placeholder.png";
                string filePath = Path.Combine(outputFolder, fileName);
                byte[] pngData = texture.EncodeToPNG();
                File.WriteAllBytes(filePath, pngData);

                Debug.Log($"Created placeholder: {fileName}");

                Destroy(texture);
            }

            Debug.Log("Placeholder frames created.");
            Debug.Log($"Location: {outputFolder}");
            Debug.Log("Replace with cat_anim_00.png to cat_anim_09.png when available.");

            #if UNITY_EDITOR
            AssetDatabase.Refresh();
            #endif
        }

        [ContextMenu("Create Placeholders")]
        private void CreatePlaceholdersManual()
        {
            CheckAndCreatePlaceholders();
        }

        [ContextMenu("Cleanup Placeholders")]
        private void CleanupPlaceholders()
        {
            if (Directory.Exists(outputFolder))
            {
                string[] placeholderFiles = Directory.GetFiles(outputFolder, "*_placeholder.png");
                foreach (string file in placeholderFiles)
                {
                    File.Delete(file);
                    Debug.Log($"Deleted: {Path.GetFileName(file)}");
                }

                #if UNITY_EDITOR
                AssetDatabase.Refresh();
                #endif

                Debug.Log("Placeholders cleaned up.");
            }
        }

        [ContextMenu("Check Resource Status")]
        private void CheckResourceStatus()
        {
            if (!Directory.Exists(outputFolder))
            {
                Debug.LogWarning($"Folder not found: {outputFolder}");
                return;
            }

            string[] pngFiles = Directory.GetFiles(outputFolder, "*.png");
            int placeholderCount = 0;
            int actualFrameCount = 0;

            foreach (string file in pngFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (fileName.Contains("placeholder"))
                {
                    placeholderCount++;
                }
                else if (fileName.StartsWith("cat_anim_"))
                {
                    actualFrameCount++;
                }
            }

            Debug.Log("Resource status:");
            Debug.Log($"- Total PNG files: {pngFiles.Length}");
            Debug.Log($"- Placeholder frames: {placeholderCount}");
            Debug.Log($"- Real frames: {actualFrameCount}");

            if (actualFrameCount >= 10)
            {
                Debug.Log("Frames OK.");
            }
            else if (actualFrameCount > 0)
            {
                Debug.LogWarning($"Frames incomplete: {actualFrameCount}/10.");
            }
            else
            {
                Debug.LogError("No real frames found.");
            }
        }

        [ContextMenu("Generate Resource Guide")]
        private void GenerateResourceGuide()
        {
            string guide = "TapCat2D Resources:\n" +
                "- Provide 10 PNGs: cat_anim_00.png to cat_anim_09.png\n" +
                "- Place in Assets/Sprites/CatAnimation/\n" +
                "- Import as Sprite (2D and UI), Point filter, no compression.";

            Debug.Log(guide);
        }
    }
}
