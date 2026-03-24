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
    [SerializeField] private bool handleOwnInput = true;

    private void Start()
    {
        Debug.Log("TapCat animated controller started.");

        InitializeCat();
        LoadAnimationResources();

        if (GetComponent<TapCat.InputHandler>() != null)
        {
            handleOwnInput = false;
            Debug.Log("FinalTapCat_Animated_Fix: InputHandler detected, using external input.");
        }

        Debug.Log("Controls: Space/Left Mouse = Play animation, R = Reset");
    }

    private void Update()
    {
        if (handleOwnInput)
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
        }

        if (cat != null && !isPlayingAnimation)
        {
            cat.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        UpdateAnimation();
    }

    private void InitializeCat()
    {
        cat = gameObject;
        cat.name = string.IsNullOrWhiteSpace(cat.name) ? "TapCat" : cat.name;

        catSprite = cat.GetComponent<SpriteRenderer>();
        if (catSprite == null)
        {
            catSprite = cat.AddComponent<SpriteRenderer>();
        }

        EnsureClickCollider();

        Debug.Log("Cat object initialized (SpriteRenderer).");
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

        UpdateClickColliderSize();
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

        if (catSprite != null)
        {
            float r = Random.Range(0.5f, 1f);
            float g = Random.Range(0.5f, 1f);
            float b = Random.Range(0.5f, 1f);
            catSprite.color = new Color(r, g, b);

            cat.transform.Rotate(0, 0, 360);
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

    public void ResetGame()
    {
        clicks = 0;
        Debug.Log("Game reset.");

        if (catSprite != null)
        {
            catSprite.color = Color.white;
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

        if (catSprite != null && frames != null && frames.Length > 0 && frames[0] != null)
        {
            catSprite.sprite = frames[0];
            UpdateClickColliderSize();
        }
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

    public void PlayTapAnimation()
    {
        HandleClick();
    }

    [ContextMenu("Reset Game")]
    private void TestResetGame()
    {
        ResetGame();
    }

    public void SetExternalInputMode(bool useExternalInput)
    {
        handleOwnInput = !useExternalInput;
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

    private void EnsureClickCollider()
    {
        if (GetComponent<Collider>() != null || GetComponent<Collider2D>() != null)
        {
            return;
        }

        BoxCollider2D collider2D = gameObject.AddComponent<BoxCollider2D>();
        collider2D.offset = Vector2.zero;
        UpdateClickColliderSize();
    }

    private void UpdateClickColliderSize()
    {
        BoxCollider2D collider2D = GetComponent<BoxCollider2D>();
        if (collider2D == null || catSprite == null || catSprite.sprite == null)
        {
            return;
        }

        Vector2 size = catSprite.sprite.bounds.size;
        collider2D.size = size;
    }
}
