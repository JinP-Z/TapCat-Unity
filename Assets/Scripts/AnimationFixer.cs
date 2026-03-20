using UnityEngine;

/// <summary>
/// Simple script to fix animation loading in FinalTapCat_Animated
/// </summary>
public class AnimationFixer : MonoBehaviour
{
    [Header("Animation Settings")]
    public string animationBasePath = "CatAnimation/cat_anim_";
    public int frameCount = 10;
    public float frameRate = 10f;
    
    [Header("Target")]
    public FinalTapCat_Animated targetScript;
    
    private void Start()
    {
        FixAnimation();
    }
    
    [ContextMenu("Fix Animation")]
    public void FixAnimation()
    {
        if (targetScript == null)
        {
            targetScript = FindObjectOfType<FinalTapCat_Animated>();
            
            if (targetScript == null)
            {
                Debug.LogError("[AnimationFixer] No FinalTapCat_Animated script found!");
                
                // Try to find any cat object
                GameObject cat = GameObject.Find("TapCat");
                if (cat == null)
                {
                    cat = new GameObject("TapCat");
                    cat.transform.position = Vector3.zero;
                }
                
                // Add the script if missing
                targetScript = cat.AddComponent<FinalTapCat_Animated>();
                Debug.Log("[AnimationFixer] Added FinalTapCat_Animated script to cat object.");
            }
        }
        
        // Load animation frames
        Sprite[] frames = new Sprite[frameCount];
        int loadedCount = 0;
        
        for (int i = 0; i < frameCount; i++)
        {
            string framePath = $"{animationBasePath}{i:00}";
            Sprite frame = Resources.Load<Sprite>(framePath);
            
            if (frame != null)
            {
                frames[i] = frame;
                loadedCount++;
                Debug.Log($"[AnimationFixer] Loaded frame {i}: {framePath}");
            }
            else
            {
                Debug.LogWarning($"[AnimationFixer] Failed to load frame {i}: {framePath}");
            }
        }
        
        if (loadedCount > 0)
        {
            // Set animation frames
            targetScript.SetAnimationFrames(frames);
            targetScript.SetFrameRate(frameRate);
            
            Debug.Log($"[AnimationFixer] Success! Loaded {loadedCount} frames.");
            Debug.Log($"[AnimationFixer] Frame rate set to {frameRate} FPS.");
            
            // Test the animation
            targetScript.CheckAnimationSystem();
            
            // Create a simple test object if needed
            EnsureSpriteRenderer();
        }
        else
        {
            Debug.LogError("[AnimationFixer] Failed to load any animation frames!");
            Debug.Log($"[AnimationFixer] Checked path: Resources/{animationBasePath}00 to Resources/{animationBasePath}{frameCount-1:00}");
        }
    }
    
    private void EnsureSpriteRenderer()
    {
        if (targetScript == null) return;
        
        GameObject cat = targetScript.gameObject;
        SpriteRenderer spriteRenderer = cat.GetComponent<SpriteRenderer>();
        
        if (spriteRenderer == null)
        {
            spriteRenderer = cat.AddComponent<SpriteRenderer>();
            Debug.Log("[AnimationFixer] Added SpriteRenderer to cat object.");
        }
        
        // Set a default sprite if available
        Sprite placeholder = Resources.Load<Sprite>("PlaceholderCat");
        if (placeholder != null && spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = placeholder;
            Debug.Log("[AnimationFixer] Set placeholder sprite.");
        }
    }
    
    [ContextMenu("Test Animation")]
    public void TestAnimation()
    {
        if (targetScript != null)
        {
            targetScript.TestClick();
            Debug.Log("[AnimationFixer] Test click triggered. Press Space or Left Mouse to see animation.");
        }
    }
    
    [ContextMenu("Check Resource Paths")]
    public void CheckResourcePaths()
    {
        Debug.Log("=== Resource Path Check ===");
        
        // Check for animation frames
        for (int i = 0; i < frameCount; i++)
        {
            string path = $"{animationBasePath}{i:00}";
            Sprite sprite = Resources.Load<Sprite>(path);
            
            if (sprite != null)
            {
                Debug.Log($"✅ Resources/{path} - FOUND ({sprite.texture.width}x{sprite.texture.height})");
            }
            else
            {
                Debug.Log($"❌ Resources/{path} - NOT FOUND");
            }
        }
        
        // Check for placeholder
        Sprite placeholder = Resources.Load<Sprite>("PlaceholderCat");
        if (placeholder != null)
        {
            Debug.Log($"✅ Resources/PlaceholderCat - FOUND");
        }
        else
        {
            Debug.Log($"❌ Resources/PlaceholderCat - NOT FOUND");
        }
        
        Debug.Log("=== End Check ===");
    }
    
    [ContextMenu("Create Complete Setup")]
    public void CreateCompleteSetup()
    {
        // Create or find camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5;
            cameraObj.AddComponent<AudioListener>();
            Debug.Log("[AnimationFixer] Created main camera.");
        }
        
        // Create or find cat
        GameObject cat = GameObject.Find("TapCat");
        if (cat == null)
        {
            cat = new GameObject("TapCat");
            cat.transform.position = Vector3.zero;
            Debug.Log("[AnimationFixer] Created cat object.");
        }
        
        // Add components
        SpriteRenderer spriteRenderer = cat.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = cat.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;
        }
        
        FinalTapCat_Animated animatedScript = cat.GetComponent<FinalTapCat_Animated>();
        if (animatedScript == null)
        {
            animatedScript = cat.AddComponent<FinalTapCat_Animated>();
        }
        
        // Set target
        targetScript = animatedScript;
        
        // Fix animation
        FixAnimation();
        
        Debug.Log("[AnimationFixer] Complete setup created!");
        Debug.Log("[AnimationFixer] Press Play in Unity, then Space or Left Mouse to test animation.");
    }
}