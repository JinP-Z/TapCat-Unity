using UnityEngine;

/// <summary>
/// Scene setup script that ensures animation system is properly configured
/// </summary>
public class SceneAnimationSetup : MonoBehaviour
{
    [Header("Setup Options")]
    public bool runOnStart = true;
    public bool createMissingObjects = true;
    public bool configureAnimation = true;
    
    [Header("Animation Configuration")]
    public string animationPath = "CatAnimation/cat_anim_";
    public int totalFrames = 10;
    public float animationFPS = 10f;
    
    private FinalTapCat_Animated catScript;
    private AnimationFixer fixer;
    
    private void Start()
    {
        if (runOnStart)
        {
            SetupScene();
        }
    }
    
    [ContextMenu("Setup Scene")]
    public void SetupScene()
    {
        Debug.Log("=== Scene Animation Setup ===");
        
        // Step 1: Ensure camera exists
        SetupCamera();
        
        // Step 2: Ensure cat object exists
        SetupCatObject();
        
        // Step 3: Configure animation
        if (configureAnimation)
        {
            ConfigureAnimation();
        }
        
        // Step 4: Add AnimationFixer for runtime fixes
        AddAnimationFixer();
        
        Debug.Log("=== Scene Setup Complete ===");
        Debug.Log("Press Play, then Space or Left Mouse to test animation.");
    }
    
    private void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null && createMissingObjects)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5f;
            mainCamera.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
            
            cameraObj.AddComponent<AudioListener>();
            
            Debug.Log("Created main camera.");
        }
        else if (mainCamera != null)
        {
            // Ensure camera is orthographic
            if (!mainCamera.orthographic)
            {
                mainCamera.orthographic = true;
                mainCamera.orthographicSize = 5f;
                Debug.Log("Set camera to orthographic mode.");
            }
        }
    }
    
    private void SetupCatObject()
    {
        GameObject cat = GameObject.Find("TapCat");
        
        if (cat == null && createMissingObjects)
        {
            cat = new GameObject("TapCat");
            cat.transform.position = Vector3.zero;
            cat.transform.localScale = Vector3.one;
            
            Debug.Log("Created cat object.");
        }
        
        if (cat != null)
        {
            // Add or get SpriteRenderer
            SpriteRenderer spriteRenderer = cat.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                spriteRenderer = cat.AddComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 1;
                Debug.Log("Added SpriteRenderer to cat.");
            }
            
            // Add or get FinalTapCat_Animated script
            catScript = cat.GetComponent<FinalTapCat_Animated>();
            if (catScript == null)
            {
                catScript = cat.AddComponent<FinalTapCat_Animated>();
                Debug.Log("Added FinalTapCat_Animated script to cat.");
            }
        }
    }
    
    private void ConfigureAnimation()
    {
        if (catScript == null)
        {
            GameObject cat = GameObject.Find("TapCat");
            if (cat != null)
            {
                catScript = cat.GetComponent<FinalTapCat_Animated>();
            }
        }
        
        if (catScript != null)
        {
            // Load animation frames
            Sprite[] frames = LoadAnimationFrames();
            
            if (frames != null && frames.Length > 0)
            {
                catScript.SetAnimationFrames(frames);
                catScript.SetFrameRate(animationFPS);
                
                Debug.Log($"Configured animation with {frames.Length} frames at {animationFPS} FPS.");
                
                // Test the configuration
                catScript.CheckAnimationSystem();
            }
            else
            {
                Debug.LogWarning("Could not load animation frames. Animation may not work.");
            }
        }
        else
        {
            Debug.LogWarning("No FinalTapCat_Animated script found. Animation not configured.");
        }
    }
    
    private Sprite[] LoadAnimationFrames()
    {
        Sprite[] frames = new Sprite[totalFrames];
        int loadedCount = 0;
        
        for (int i = 0; i < totalFrames; i++)
        {
            string frameName = $"{animationPath}{i:00}";
            Sprite frame = Resources.Load<Sprite>(frameName);
            
            if (frame != null)
            {
                frames[i] = frame;
                loadedCount++;
            }
        }
        
        if (loadedCount == totalFrames)
        {
            Debug.Log($"Successfully loaded all {totalFrames} animation frames.");
            return frames;
        }
        else if (loadedCount > 0)
        {
            Debug.Log($"Loaded {loadedCount}/{totalFrames} animation frames.");
            // Return only loaded frames
            Sprite[] loadedFrames = new Sprite[loadedCount];
            int index = 0;
            for (int i = 0; i < totalFrames; i++)
            {
                if (frames[i] != null)
                {
                    loadedFrames[index++] = frames[i];
                }
            }
            return loadedFrames;
        }
        else
        {
            Debug.LogError("Failed to load any animation frames!");
            return null;
        }
    }
    
    private void AddAnimationFixer()
    {
        // Add AnimationFixer to this GameObject
        fixer = GetComponent<AnimationFixer>();
        if (fixer == null)
        {
            fixer = gameObject.AddComponent<AnimationFixer>();
            
            // Configure the fixer
            fixer.animationBasePath = animationPath;
            fixer.frameCount = totalFrames;
            fixer.frameRate = animationFPS;
            
            if (catScript != null)
            {
                fixer.targetScript = catScript;
            }
            
            Debug.Log("Added AnimationFixer component for runtime fixes.");
        }
    }
    
    [ContextMenu("Test Resource Loading")]
    public void TestResourceLoading()
    {
        Debug.Log("=== Testing Resource Loading ===");
        
        for (int i = 0; i < totalFrames; i++)
        {
            string path = $"{animationPath}{i:00}";
            Sprite sprite = Resources.Load<Sprite>(path);
            
            if (sprite != null)
            {
                Debug.Log($"✅ Frame {i}: {path} - Loaded successfully");
                Debug.Log($"   Size: {sprite.texture.width}x{sprite.texture.height}");
            }
            else
            {
                Debug.Log($"❌ Frame {i}: {path} - FAILED to load");
            }
        }
        
        // Test placeholder
        Sprite placeholder = Resources.Load<Sprite>("PlaceholderCat");
        if (placeholder != null)
        {
            Debug.Log($"✅ PlaceholderCat - Loaded successfully");
        }
        else
        {
            Debug.Log($"❌ PlaceholderCat - NOT found");
        }
        
        Debug.Log("=== End Test ===");
    }
    
    [ContextMenu("Quick Test Animation")]
    public void QuickTestAnimation()
    {
        if (catScript != null)
        {
            catScript.TestClick();
            Debug.Log("Test click triggered. Check console for animation status.");
        }
        else
        {
            Debug.LogWarning("No cat script found. Run Setup Scene first.");
        }
    }
    
    [ContextMenu("Create Minimal Test Scene")]
    public void CreateMinimalTestScene()
    {
        // Clean up existing objects (except this one)
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj != gameObject && obj.name != "EventSystem")
            {
                DestroyImmediate(obj);
            }
        }
        
        // Create minimal setup
        SetupCamera();
        SetupCatObject();
        ConfigureAnimation();
        
        Debug.Log("Minimal test scene created. Ready for testing.");
    }
}