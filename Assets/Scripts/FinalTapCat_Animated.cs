using UnityEngine;

/// <summary>
/// Final TapCat animated controller using sprite frames.
/// </summary>
public class FinalTapCat_Animated : MonoBehaviour
{
    private GameObject cat;
    private int clicks = 0;
    private float rotationSpeed = 30f;

    private SpriteRenderer catSprite;
    private Sprite[] animationFrames;
    private bool isPlayingAnimation = false;
    private float animationTimer = 0f;
    private float frameTime = 0.1f;
    private int currentFrame = 0;

    private void Start()
    {
        Debug.Log("TapCat animated controller started.");

        CreateCat();
        LoadAnimationResources();

        Debug.Log("Controls: Space/Left Mouse = Play animation, R = Reset");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleClick();
        }

        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }

        if (cat != null && !isPlayingAnimation)
        {
            cat.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }

        UpdateAnimation();
    }

    private void CreateCat()
    {
        if (cat != null)
        {
            Destroy(cat);
        }

        cat = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cat.name = "TapCat";
        cat.transform.position = Vector3.zero;
        cat.transform.localScale = new Vector3(3f, 3f, 0.2f);

        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(1f, 0.8f, 0f);
        }

        catSprite = cat.GetComponent<SpriteRenderer>();
        if (catSprite == null)
        {
            catSprite = cat.AddComponent<SpriteRenderer>();
        }

        Debug.Log("Cat object created.");
    }

    private void LoadAnimationResources()
    {
        Debug.Log("Animation folder: Assets/Sprites/CatAnimation/");
        Debug.Log("Frames: cat_anim_00.png to cat_anim_09.png");

        string spritePath = Application.dataPath + "/Sprites/CatAnimation/";
        if (System.IO.Directory.Exists(spritePath))
        {
            int pngCount = System.IO.Directory.GetFiles(spritePath, "*.png").Length;
            Debug.Log($"Found {pngCount} PNG files.");

            if (pngCount >= 10)
            {
                Debug.Log("Animation frames look complete.");
            }
            else
            {
                Debug.LogWarning($"Animation frames incomplete. Need 10, found {pngCount}.");
            }
        }
        else
        {
            Debug.LogWarning("Animation folder not found.");
        }
    }

    private void HandleClick()
    {
        clicks++;
        Debug.Log($"Tap! Count: {clicks}");

        if (animationFrames != null && animationFrames.Length > 0)
        {
            StartAnimation();
        }
        else
        {
            ApplyColorChange();
        }

        ShowClickCount();
    }

    private void StartAnimation()
    {
        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning("No animation frames to play.");
            return;
        }

        isPlayingAnimation = true;
        currentFrame = 0;
        animationTimer = 0f;

        if (catSprite != null)
        {
            catSprite.sprite = animationFrames[0];
        }

        Debug.Log("Animation started.");
    }

    private void UpdateAnimation()
    {
        if (!isPlayingAnimation || animationFrames == null || animationFrames.Length == 0)
        {
            return;
        }

        animationTimer += Time.deltaTime;

        if (animationTimer >= frameTime)
        {
            animationTimer = 0f;
            currentFrame++;

            if (currentFrame >= animationFrames.Length)
            {
                isPlayingAnimation = false;
                currentFrame = 0;

                if (catSprite != null)
                {
                    catSprite.sprite = null;
                }

                Debug.Log("Animation finished.");
            }
            else
            {
                if (catSprite != null)
                {
                    catSprite.sprite = animationFrames[currentFrame];
                }
            }
        }
    }

    private void ApplyColorChange()
    {
        if (cat == null)
        {
            return;
        }

        Renderer renderer = cat.GetComponent<Renderer>();
        if (renderer != null)
        {
            float r = Random.Range(0.5f, 1f);
            float g = Random.Range(0.5f, 1f);
            float b = Random.Range(0.5f, 1f);
            renderer.material.color = new Color(r, g, b);

            cat.transform.Rotate(0, 360, 0);
        }
    }

    private void OnGUI()
    {
        GUI.color = Color.white;
        GUI.backgroundColor = new Color(0, 0, 0, 0.7f);

        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        string statusText = isPlayingAnimation
            ? $"Taps: {clicks}\nAnimation playing..."
            : $"Taps: {clicks}\nAnimation idle";

        GUI.Box(new Rect(10, 10, 250, 70), $"{statusText}\nPress R to reset", style);

        GUIStyle hintStyle = new GUIStyle(GUI.skin.label);
        hintStyle.fontSize = 14;
        hintStyle.normal.textColor = Color.yellow;

        GUI.Label(new Rect(10, 90, 300, 40), "Press Space or Left Mouse to play", hintStyle);

        string resourceStatus = animationFrames != null && animationFrames.Length > 0
            ? $"Frames ready ({animationFrames.Length})"
            : "Frames not set";

        GUI.Label(new Rect(10, 130, 300, 40), resourceStatus, hintStyle);
    }

    private void ShowClickCount()
    {
        // Displayed in OnGUI.
    }

    private void ResetGame()
    {
        clicks = 0;
        Debug.Log("Game reset.");

        if (cat != null)
        {
            Renderer renderer = cat.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(1f, 0.8f, 0f);
            }
        }

        isPlayingAnimation = false;
        currentFrame = 0;

        if (catSprite != null)
        {
            catSprite.sprite = null;
        }
    }

    public void SetAnimationFrames(Sprite[] frames)
    {
        animationFrames = frames;
        Debug.Log($"Animation frames set: {frames.Length}");
    }

    public void SetFrameRate(float framesPerSecond)
    {
        if (framesPerSecond > 0)
        {
            frameTime = 1f / framesPerSecond;
            Debug.Log($"Frame rate set to {framesPerSecond} FPS");
        }
    }

    [ContextMenu("Test Click")]
    private void TestClick()
    {
        HandleClick();
    }

    [ContextMenu("Reset Game")]
    private void TestResetGame()
    {
        ResetGame();
    }

    [ContextMenu("Check Animation System")]
    private void CheckAnimationSystem()
    {
        if (animationFrames == null || animationFrames.Length == 0)
        {
            Debug.LogWarning("Animation frames not set.");
            Debug.Log("Assign cat_anim_00.png to cat_anim_09.png to animationFrames.");
        }
        else
        {
            Debug.Log($"Animation system OK. Frames: {animationFrames.Length}");
        }
    }
}
