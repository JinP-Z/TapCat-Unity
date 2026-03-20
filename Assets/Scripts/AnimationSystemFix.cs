using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Complete animation system fix for TapCat-Unity project
/// This script fixes all animation-related issues
/// </summary>
public class AnimationSystemFix : MonoBehaviour
{
    [Header("Fix Options")]
    public bool applyFixOnStart = true;
    public bool replaceOriginalScript = true;
    public bool setupCompleteScene = true;
    public bool testAfterFix = true;
    
    [Header("Animation Configuration")]
    public string animationPath = "CatAnimation/cat_anim_";
    public int totalFrames = 10;
    public float frameRate = 10f;
    
    [Header("Scene Objects")]
    public GameObject catObject;
    public Camera mainCamera;
    
    private void Start()
    {
        if (applyFixOnStart)
        {
            ApplyCompleteFix();
        }
    }
    
    [ContextMenu("Apply Complete Fix")]
    public void ApplyCompleteFix()
    {
        Debug.Log("=== APPLYING ANIMATION SYSTEM FIX ===");
        
        // Step 1: Verify resources exist
        if (!VerifyResources())
        {
            Debug.LogError("Fix cannot proceed: Resources missing!");
            return;
        }
        
        // Step 2: Setup scene if needed
        if (setupCompleteScene)
        {
            SetupScene();
        }
        
        // Step 3: Replace original script if needed
        if (replaceOriginalScript)
        {
            ReplaceOriginalScript();
        }
        
        // Step 4: Configure animation
        ConfigureAnimation();
        
        // Step 5: Test if requested
        if (testAfterFix)
        {
            TestAnimation();
        }
        
        Debug.Log("=== FIX COMPLETE ===");
        Debug.Log("Animation system should now work correctly.");
        Debug.Log("Press Space or Left Mouse to test animation.");
    }
    
    private bool VerifyResources()
    {
        Debug.Log("Verifying animation resources...");
        
        int foundFrames = 0;
        for (int i = 0; i < totalFrames; i++)
        {
            string frameName = $"{animationPath}{i:00}";
            Sprite frame = Resources.Load<Sprite>(frameName);
            
            if (frame != null)
            {
                foundFrames++;
            }
        }
        
        if (foundFrames == totalFrames)
        {
            Debug.Log($"✅ All {totalFrames} animation frames found in Resources folder.");
            return true;
        }
        else if (foundFrames > 0)
        {
            Debug.LogWarning($"⚠️ Found {foundFrames}/{totalFrames} animation frames.");
            Debug.Log("Animation may work partially.");
            return true;
        }
        else
        {
            Debug.LogError($"❌ No animation frames found in Resources folder!");
            Debug.Log($"Expected: Resources/{animationPath}00 to {animationPath}{totalFrames-1:00}");
            return false;
        }
    }
    
    private void SetupScene()
    {
        Debug.Log("Setting up scene...");
        
        // Setup camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = 5f;
            mainCamera.backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            cameraObj.AddComponent<AudioListener>();
            Debug.Log("Created main camera.");
        }
        
        // Setup cat object
        catObject = GameObject.Find("TapCat");
        if (catObject == null)
        {
            catObject = new GameObject("TapCat");
            catObject.transform.position = Vector3.zero;
            Debug.Log("Created cat object.");
        }
        
        // Ensure SpriteRenderer exists
        SpriteRenderer spriteRenderer = catObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = catObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;
            Debug.Log("Added SpriteRenderer to cat.");
        }
        
        // Set placeholder sprite
        Sprite placeholder = Resources.Load<Sprite>("PlaceholderCat");
        if (placeholder != null && spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = placeholder;
            Debug.Log("Set placeholder sprite.");
        }
    }
    
    private void ReplaceOriginalScript()
    {
        if (catObject == null)
        {
            catObject = GameObject.Find("TapCat");
            if (catObject == null) return;
        }
        
        // Remove original FinalTapCat_Animated script
        FinalTapCat_Animated originalScript = catObject.GetComponent<FinalTapCat_Animated>();
        if (originalScript != null)
        {
            DestroyImmediate(originalScript);
            Debug.Log("Removed original FinalTapCat_Animated script.");
        }
        
        // Add the fixed version
        FinalTapCat_Animated_Fix fixedScript = catObject.GetComponent<FinalTapCat_Animated_Fix>();
        if (fixedScript == null)
        {
            fixedScript = catObject.AddComponent<FinalTapCat_Animated_Fix>();
            Debug.Log("Added FinalTapCat_Animated_Fix script.");
        }
    }
    
    private void ConfigureAnimation()
    {
        if (catObject == null) return;
        
        // Try to get the fixed script
        FinalTapCat_Animated_Fix fixedScript = catObject.GetComponent<FinalTapCat_Animated_Fix>();
        
        if (fixedScript != null)
        {
            // The fixed script loads animation automatically in Start()
            Debug.Log("Fixed animation script configured.");
            
            // Manually trigger reload to ensure frames are loaded
            fixedScript.ReloadAnimationFrames();
        }
        else
        {
            // Fallback: Try to configure original script
            FinalTapCat_Animated originalScript = catObject.GetComponent<FinalTapCat_Animated>();
            if (originalScript != null)
            {
                // Load frames manually
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
                
                if (loadedCount > 0)
                {
                    originalScript.SetAnimationFrames(frames);
                    originalScript.SetFrameRate(frameRate);
                    Debug.Log($"Configured original script with {loadedCount} frames.");
                }
            }
        }
    }
    
    private void TestAnimation()
    {
        if (catObject == null) return;
        
        Debug.Log("Testing animation system...");
        
        // Try fixed script first
        FinalTapCat_Animated_Fix fixedScript = catObject.GetComponent<FinalTapCat_Animated_Fix>();
        if (fixedScript != null)
        {
            fixedScript.CheckAnimationSystem();
            fixedScript.TestClick();
            Debug.Log("Tested fixed animation script.");
        }
        else
        {
            // Try original script
            FinalTapCat_Animated originalScript = catObject.GetComponent<FinalTapCat_Animated>();
            if (originalScript != null)
            {
                originalScript.CheckAnimationSystem();
                originalScript.TestClick();
                Debug.Log("Tested original animation script.");
            }
        }
    }
    
    [ContextMenu("Quick Status Check")]
    public void QuickStatusCheck()
    {
        Debug.Log("=== Quick Status Check ===");
        
        // Check camera
        Camera cam = Camera.main;
        Debug.Log($"Camera: {(cam != null ? cam.name : "None")}");
        
        // Check cat object
        GameObject cat = GameObject.Find("TapCat");
        if (cat != null)
        {
            Debug.Log($"Cat object: {cat.name}");
            
            // Check scripts
            FinalTapCat_Animated_Fix fixScript = cat.GetComponent<FinalTapCat_Animated_Fix>();
            FinalTapCat_Animated origScript = cat.GetComponent<FinalTapCat_Animated>();
            
            if (fixScript != null)
            {
                Debug.Log("Script: FinalTapCat_Animated_Fix (Fixed)");
                fixScript.CheckAnimationSystem();
            }
            else if (origScript != null)
            {
                Debug.Log("Script: FinalTapCat_Animated (Original)");
                origScript.CheckAnimationSystem();
            }
            else
            {
                Debug.Log("Script: None (needs fix)");
            }
            
            // Check sprite
            SpriteRenderer sr = cat.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Debug.Log($"Sprite: {(sr.sprite != null ? sr.sprite.name : "None")}");
            }
        }
        else
        {
            Debug.Log("Cat object: None (needs setup)");
        }
        
        Debug.Log("=== End Check ===");
    }
    
    [ContextMenu("Create Simple Test Scene")]
    public void CreateSimpleTestScene()
    {
        // Clean up existing test objects
        GameObject[] objectsToRemove = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objectsToRemove)
        {
            if (obj != gameObject && 
                obj.name != "EventSystem" && 
                !obj.name.Contains("Canvas") &&
                obj.transform.parent == null)
            {
                DestroyImmediate(obj);
            }
        }
        
        // Create minimal setup
        GameObject cameraObj = new GameObject("Main Camera");
        cameraObj.tag = "MainCamera";
        Camera cam = cameraObj.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.backgroundColor = Color.gray;
        cameraObj.AddComponent<AudioListener>();
        
        GameObject cat = new GameObject("TapCat");
        cat.transform.position = Vector3.zero;
        
        SpriteRenderer sr = cat.AddComponent<SpriteRenderer>();
        sr.sortingOrder = 1;
        
        // Add fixed script
        cat.AddComponent<FinalTapCat_Animated_Fix>();
        
        Debug.Log("Created simple test scene.");
        Debug.Log("Press Play, then Space or Left Mouse to test animation.");
    }
    
    [ContextMenu("Fix Only Original Script")]
    public void FixOnlyOriginalScript()
    {
        // This modifies the original script in memory
        Debug.Log("Attempting to fix original script behavior...");
        
        GameObject cat = GameObject.Find("TapCat");
        if (cat == null)
        {
            Debug.LogError("No TapCat object found!");
            return;
        }
        
        FinalTapCat_Animated script = cat.GetComponent<FinalTapCat_Animated>();
        if (script == null)
        {
            Debug.LogError("No FinalTapCat_Animated script found!");
            return;
        }
        
        // Load frames and assign them
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
        
        if (loadedCount > 0)
        {
            script.SetAnimationFrames(frames);
            script.SetFrameRate(frameRate);
            Debug.Log($"Fixed original script with {loadedCount} frames.");
            script.CheckAnimationSystem();
        }
        else
        {
            Debug.LogError("Could not load any frames for original script!");
        }
    }
}