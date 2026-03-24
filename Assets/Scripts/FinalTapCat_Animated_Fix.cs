using UnityEngine;

/// <summary>
/// Fixed version of FinalTapCat_Animated with proper animation loading
/// </summary>
public class FinalTapCat_Animated_Fix : MonoBehaviour
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
        Debug.Log("Loading animation frames from Resources/CatAnimation/...");
        
        // Try to load animation frames
        animationFrames = new Sprite[10];
        int loadedCount = 0;
        
        for (int i = 0; i < 10; i++)
        {
            string frameName = $"CatAnimation/cat_anim_{i:00}";
            Sprite frame = Resources.Load<Sprite>(frameName);
            
            if (frame != null)
            {
                animationFrames[i] = frame;
                loadedCount++;
                Debug.Log($"Loaded frame {i}: {frameName}");
            }
            else
            {
                Debug.LogWarning($"Failed to load frame {i}: {frameName}");
            }
        }
        
        if (loadedCount > 0)
        {
            Debug.Log($"Successfully loaded {loadedCount}/10 animation frames.");
            
            // If we have frames, set the first one as default
            if (catSprite != null && animationFrames[0] != null)
            {
                catSprite.sprite = animationFrames[0];
            }
        }
        else
        {
            Debug.LogWarning("No animation frames could be loaded.");
            Debug.Log("Make sure files are in Assets/Resources/CatAnimation/");
            Debug.Log("File names should be: cat_anim_00.png to cat_anim_09.png");
        }
        
        // Also try to load placeholder as fallback
        Sprite placeholder = Resources.Load<Sprite>("PlaceholderCat");
        if (placeholder != null && catSprite != null && catSprite.sprite == null)
        {
            catSprite.sprite = placeholder;
            Debug.Log("Set placeholder sprite.");
        }
    }

    private void HandleClick()
    {
        clicks++;
        Debug.Log($"Tap! Count: {clicks}");

        if (animationFrames != null && animationFrames.Length > 0 && animationFrames[0] != null)
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
        if (animationFrames == null || animationFrames.Length == 0 || animationFrames[0] == null)
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

            if (currentFrame >= animationFrames.Length || animationFrames[currentFrame] == null)
            {
                isPlayingAnimation = false;
                currentFrame = 0;

                // Keep the first frame displayed
                if (catSprite != null && animationFrames[0] != null)
                {
                    catSprite.sprite = animationFrames[0];
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

        int frameCount = 0;
        if (animationFrames != null)
        {
            foreach (Sprite frame in animationFrames)
            {
                if (frame != null) frameCount++;
            }
        }
        
        string resourceStatus = frameCount > 0
            ? $"Frames ready ({frameCount}/10)"
            : "Frames not loaded";

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

        // Show first frame if available
        if (catSprite != null && animationFrames != null && animationFrames.Length > 0 && animationFrames[0] != null)
        {
            catSprite.sprite = animationFrames[0];
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
    public void TestClick()
    {
        HandleClick();
    }

    [ContextMenu("Reset Game")]
    private void TestResetGame()
    {
        ResetGame();
    }

    [ContextMenu("Check Animation System")]
    public void CheckAnimationSystem()
    {
        if (animationFrames == null || animationFrames.Length == 0 || animationFrames[0] == null)
        {
            Debug.LogWarning("Animation frames not loaded.");
            Debug.Log("Make sure animation frames are in Assets/Resources/CatAnimation/");
        }
        else
        {
            int validFrames = 0;
            foreach (Sprite frame in animationFrames)
            {
                if (frame != null) validFrames++;
            }
            
            Debug.Log($"Animation system OK. Frames loaded: {validFrames}/10");
        }
    }
    
    [ContextMenu("Reload Animation Frames")]
    public void ReloadAnimationFrames()
    {
        LoadAnimationResources();
    }
}
