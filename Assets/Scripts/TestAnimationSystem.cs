using UnityEngine;

/// <summary>
/// Simple test to verify animation system works
/// </summary>
public class TestAnimationSystem : MonoBehaviour
{
    [Header("Test Settings")]
    public bool runOnStart = true;
    public bool createTestObjects = true;
    public bool testAnimationLoading = true;
    
    private FinalTapCat_Animated_Fix animatedScript;
    private GameObject testCat;
    
    private void Start()
    {
        if (runOnStart)
        {
            RunTest();
        }
    }
    
    [ContextMenu("Run Animation Test")]
    public void RunTest()
    {
        Debug.Log("=== Animation System Test ===");
        
        // Step 1: Check resources
        if (testAnimationLoading)
        {
            TestResourceLoading();
        }
        
        // Step 2: Create test objects
        if (createTestObjects)
        {
            CreateTestObjects();
        }
        
        // Step 3: Test animation
        TestAnimationPlayback();
        
        Debug.Log("=== Test Complete ===");
        Debug.Log("Press Space or Left Mouse to test animation playback.");
    }
    
    private void TestResourceLoading()
    {
        Debug.Log("Testing resource loading...");
        
        int loadedFrames = 0;
        for (int i = 0; i < 10; i++)
        {
            string frameName = $"CatAnimation/cat_anim_{i:00}";
            Sprite frame = Resources.Load<Sprite>(frameName);
            
            if (frame != null)
            {
                loadedFrames++;
                Debug.Log($"✅ Frame {i}: {frameName} - Loaded ({frame.texture.width}x{frame.texture.height})");
            }
            else
            {
                Debug.Log($"❌ Frame {i}: {frameName} - Failed to load");
            }
        }
        
        if (loadedFrames == 10)
        {
            Debug.Log($"✅ SUCCESS: All 10 animation frames loaded!");
        }
        else if (loadedFrames > 0)
        {
            Debug.Log($"⚠️ PARTIAL: {loadedFrames}/10 frames loaded");
        }
        else
        {
            Debug.LogError("❌ FAILED: No animation frames loaded!");
            Debug.Log("Check that files are in Assets/Resources/CatAnimation/");
            Debug.Log("File names should be: cat_anim_00.png to cat_anim_09.png");
        }
        
        // Test placeholder
        Sprite placeholder = Resources.Load<Sprite>("PlaceholderCat");
        if (placeholder != null)
        {
            Debug.Log($"✅ PlaceholderCat loaded ({placeholder.texture.width}x{placeholder.texture.height})");
        }
        else
        {
            Debug.LogWarning("⚠️ PlaceholderCat not found in Resources folder");
        }
    }
    
    private void CreateTestObjects()
    {
        // Create or find camera
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Test Camera");
            cameraObj.tag = "MainCamera";
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5f;
            mainCamera.backgroundColor = Color.gray;
            cameraObj.AddComponent<AudioListener>();
            Debug.Log("Created test camera.");
        }
        
        // Create test cat
        testCat = GameObject.Find("TestCat");
        if (testCat == null)
        {
            testCat = new GameObject("TestCat");
            testCat.transform.position = Vector3.zero;
            
            // Add SpriteRenderer
            SpriteRenderer spriteRenderer = testCat.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;
            
            // Add the fixed animation script
            animatedScript = testCat.AddComponent<FinalTapCat_Animated_Fix>();
            
            Debug.Log("Created test cat with animation script.");
        }
        else
        {
            animatedScript = testCat.GetComponent<FinalTapCat_Animated_Fix>();
            if (animatedScript == null)
            {
                animatedScript = testCat.AddComponent<FinalTapCat_Animated_Fix>();
            }
            Debug.Log("Using existing test cat.");
        }
        
        // Set a placeholder sprite immediately
        Sprite placeholder = Resources.Load<Sprite>("PlaceholderCat");
        SpriteRenderer sr = testCat.GetComponent<SpriteRenderer>();
        if (sr != null && placeholder != null && sr.sprite == null)
        {
            sr.sprite = placeholder;
            Debug.Log("Set placeholder sprite on test cat.");
        }
    }
    
    private void TestAnimationPlayback()
    {
        if (animatedScript != null)
        {
            // Check animation system
            animatedScript.CheckAnimationSystem();
            
            // Trigger a test click
            animatedScript.TestClick();
            
            Debug.Log("Animation test triggered. Check console for status.");
            Debug.Log("The cat should now show an animation when you press Space or Left Mouse.");
        }
        else
        {
            Debug.LogError("No animation script found for testing!");
        }
    }
    
    [ContextMenu("Manual Test Click")]
    public void ManualTestClick()
    {
        if (animatedScript != null)
        {
            animatedScript.TestClick();
            Debug.Log("Manual test click triggered.");
        }
        else
        {
            Debug.LogWarning("No animation script found. Run test first.");
        }
    }
    
    [ContextMenu("Check Current Setup")]
    public void CheckCurrentSetup()
    {
        Debug.Log("=== Current Setup Check ===");
        
        // Check camera
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Debug.Log($"Camera: {mainCamera.name} ({(mainCamera.orthographic ? "Orthographic" : "Perspective")})");
        }
        else
        {
            Debug.Log("Camera: None (create one with Run Test)");
        }
        
        // Check cat object
        GameObject cat = GameObject.Find("TestCat");
        if (cat == null) cat = GameObject.Find("TapCat");
        
        if (cat != null)
        {
            Debug.Log($"Cat object: {cat.name}");
            
            // Check components
            SpriteRenderer sr = cat.GetComponent<SpriteRenderer>();
            Debug.Log($"SpriteRenderer: {(sr != null ? "Present" : "Missing")}");
            
            if (sr != null)
            {
                Debug.Log($"Current sprite: {(sr.sprite != null ? sr.sprite.name : "None")}");
            }
            
            FinalTapCat_Animated_Fix fixScript = cat.GetComponent<FinalTapCat_Animated_Fix>();
            FinalTapCat_Animated origScript = cat.GetComponent<FinalTapCat_Animated>();
            
            if (fixScript != null)
            {
                Debug.Log("Animation script: FinalTapCat_Animated_Fix (Fixed version)");
                fixScript.CheckAnimationSystem();
            }
            else if (origScript != null)
            {
                Debug.Log("Animation script: FinalTapCat_Animated (Original)");
                origScript.CheckAnimationSystem();
            }
            else
            {
                Debug.Log("Animation script: None");
            }
        }
        else
        {
            Debug.Log("Cat object: None (create one with Run Test)");
        }
        
        Debug.Log("=== End Check ===");
    }
    
    [ContextMenu("Cleanup Test Objects")]
    public void CleanupTestObjects()
    {
        GameObject testCat = GameObject.Find("TestCat");
        if (testCat != null)
        {
            DestroyImmediate(testCat);
            Debug.Log("Removed test cat.");
        }
        
        GameObject testCamera = GameObject.Find("Test Camera");
        if (testCamera != null)
        {
            DestroyImmediate(testCamera);
            Debug.Log("Removed test camera.");
        }
        
        Debug.Log("Cleanup complete.");
    }
}