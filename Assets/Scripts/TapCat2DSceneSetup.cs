using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace TapCat
{
    /// <summary>
    /// Scene setup helper for TapCat2D.
    /// </summary>
    [ExecuteInEditMode]
    public class TapCat2DSceneSetup : MonoBehaviour
    {
        [Header("Scene Setup")]
        [SerializeField] private bool autoSetupOnPlay = true;
        [SerializeField] private bool createPlaceholderSprites = true;

        [Header("Placeholder Textures")]
        [SerializeField] private Texture2D[] placeholderTextures;

        private void Start()
        {
            if (Application.isPlaying && autoSetupOnPlay)
            {
                SetupSceneForPlay();
            }
        }

        private void SetupSceneForPlay()
        {
            Debug.Log("Setting up TapCat2D scene...");

            TapCat2DSetup existingSetup = FindObjectOfType<TapCat2DSetup>();
            if (existingSetup == null)
            {
                GameObject setupObj = new GameObject("TapCat2D_Setup");
                setupObj.AddComponent<TapCat2DSetup>();
                Debug.Log("TapCat2D setup object created.");
            }

            CheckForCatAnimationFrames();

            Debug.Log("TapCat2D scene setup complete.");
            Debug.Log("Controls: Space/Left Mouse = Play, R = Reset");
        }

        private void CheckForCatAnimationFrames()
        {
            SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
            bool hasCatSprites = false;

            foreach (var renderer in spriteRenderers)
            {
                if (renderer.sprite != null && (renderer.sprite.name.Contains("cat_anim") || renderer.sprite.name.Contains("CatAnimation")))
                {
                    hasCatSprites = true;
                    break;
                }
            }

            if (!hasCatSprites && createPlaceholderSprites)
            {
                Debug.LogWarning("No cat animation frames found. Creating placeholders.");
                CreatePlaceholderSprites();
            }
            else if (hasCatSprites)
            {
                Debug.Log("Cat animation frames detected.");
            }
        }

        private void CreatePlaceholderSprites()
        {
            GameObject placeholderObj = new GameObject("CatAnimation_Placeholder");
            SpriteRenderer renderer = placeholderObj.AddComponent<SpriteRenderer>();

            Texture2D tex = new Texture2D(64, 64);
            Color[] colors = new Color[64 * 64];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.cyan;
            }
            tex.SetPixels(colors);
            tex.Apply();

            Sprite placeholderSprite = Sprite.Create(tex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
            renderer.sprite = placeholderSprite;

            TapCat2D tapCat2D = placeholderObj.AddComponent<TapCat2D>();

            Sprite[] placeholderSprites = new Sprite[10];
            for (int i = 0; i < 10; i++)
            {
                Texture2D frameTex = new Texture2D(64, 64);
                Color frameColor = Color.HSVToRGB(i / 10f, 0.8f, 1f);
                Color[] frameColors = new Color[64 * 64];
                for (int j = 0; j < frameColors.Length; j++)
                {
                    frameColors[j] = frameColor;
                }
                frameTex.SetPixels(frameColors);
                frameTex.Apply();

                placeholderSprites[i] = Sprite.Create(frameTex, new Rect(0, 0, 64, 64), new Vector2(0.5f, 0.5f));
                placeholderSprites[i].name = $"cat_anim_{i:00}_placeholder";
            }

            tapCat2D.SetAnimationFrames(placeholderSprites);

            Debug.Log("Placeholder frames created. Replace with cat_anim_00.png to cat_anim_09.png.");
        }

        [ContextMenu("Validate Scene Setup")]
        private void ValidateSceneSetup()
        {
            Debug.Log("Validating TapCat2D scene setup...");

            bool hasTapCat2D = FindObjectOfType<TapCat2D>() != null;
            bool hasTapCat2DSetup = FindObjectOfType<TapCat2DSetup>() != null;
            bool hasCamera = Camera.main != null;

            Debug.Log("Scene check:");
            Debug.Log($"- TapCat2D: {(hasTapCat2D ? "OK" : "Missing")}");
            Debug.Log($"- TapCat2DSetup: {(hasTapCat2DSetup ? "OK" : "Missing")}");
            Debug.Log($"- Main Camera: {(hasCamera ? "OK" : "Missing")}");

            if (!hasTapCat2D || !hasTapCat2DSetup)
            {
                Debug.LogWarning("Scene is missing required components.");
            }
            else
            {
                Debug.Log("Scene setup looks good.");
            }
        }

        [ContextMenu("Run Auto Setup")]
        private void RunAutoSetup()
        {
            if (Application.isPlaying)
            {
                SetupSceneForPlay();
            }
            else
            {
                Debug.Log("Run auto setup in Play Mode.");
                #if UNITY_EDITOR
                EditorUtility.DisplayDialog(
                    "TapCat2D Setup",
                    "Auto setup will run when you enter Play Mode. Make sure cat_anim_00.png to cat_anim_09.png are under Assets/Sprites/CatAnimation/.",
                    "OK");
                #endif
            }
        }

        [ContextMenu("Create Example Scene")]
        private void CreateExampleScene()
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (EditorSceneManager.GetActiveScene().isDirty)
                {
                    EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                }

                EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

                GameObject sceneSetupObj = new GameObject("SceneSetup");
                sceneSetupObj.AddComponent<TapCat2DSceneSetup>();

                GameObject gameSetupObj = new GameObject("GameSetup");
                gameSetupObj.AddComponent<TapCat2DSetup>();

                Debug.Log("Example scene created. Add cat_anim_00.png to cat_anim_09.png under Assets/Sprites/CatAnimation/.");
            }
            else
            {
                Debug.Log("Create example scene in Edit Mode.");
            }
            #endif
        }

        [ContextMenu("Show Controls")]
        private void ShowControls()
        {
            string controls = "Controls:\n- Space / Left Mouse: Play animation\n- R: Reset";
            Debug.Log(controls);
        }
    }
}
