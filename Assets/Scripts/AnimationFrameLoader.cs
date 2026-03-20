using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Utility script to load and assign animation frames to FinalTapCat_Animated
/// </summary>
public class AnimationFrameLoader : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("Path to animation frames in Resources folder")]
    public string animationPath = "CatAnimation/cat_anim_";
    
    [Tooltip("Number of animation frames (0-9)")]
    public int frameCount = 10;
    
    [Tooltip("Frames per second for animation")]
    public float frameRate = 10f;
    
    [Header("Target Script")]
    [Tooltip("The FinalTapCat_Animated script to configure")]
    public FinalTapCat_Animated targetScript;
    
    [Header("Debug")]
    [Tooltip("Auto-load on start")]
    public bool autoLoadOnStart = true;
    
    [Tooltip("Log detailed information")]
    public bool verboseLogging = true;
    
    private void Start()
    {
        if (autoLoadOnStart)
        {
            LoadAndAssignAnimationFrames();
        }
    }
    
    [ContextMenu("Load Animation Frames")]
    public void LoadAndAssignAnimationFrames()
    {
        if (targetScript == null)
        {
            targetScript = FindObjectOfType<FinalTapCat_Animated>();
            
            if (targetScript == null)
            {
                Debug.LogError("No FinalTapCat_Animated script found in scene!");
                return;
            }
        }
        
        // Load animation frames
        List<Sprite> frames = new List<Sprite>();
        
        for (int i = 0; i < frameCount; i++)
        {
            string frameName = $"{animationPath}{i:00}";
            Sprite frame = Resources.Load<Sprite>(frameName);
            
            if (frame != null)
            {
                frames.Add(frame);
                if (verboseLogging)
                {
                    Debug.Log($"Loaded frame {i}: {frameName}");
                }
            }
            else
            {
                Debug.LogWarning($"Failed to load frame {i}: {frameName}");
            }
        }
        
        if (frames.Count > 0)
        {
            // Assign frames to target script
            targetScript.SetAnimationFrames(frames.ToArray());
            targetScript.SetFrameRate(frameRate);
            
            Debug.Log($"Successfully loaded {frames.Count}/{frameCount} animation frames.");
            Debug.Log($"Animation frame rate set to {frameRate} FPS.");
            
            // Run the target script's check
            targetScript.CheckAnimationSystem();
        }
        else
        {
            Debug.LogError("No animation frames could be loaded!");
            Debug.Log($"Checked path: Resources/{animationPath}00 to Resources/{animationPath}{frameCount-1:00}");
            Debug.Log("Make sure the animation frames are in the Resources folder.");
        }
    }
    
    [ContextMenu("Test Animation System")]
    public void TestAnimationSystem()
    {
        if (targetScript == null)
        {
            targetScript = FindObjectOfType<FinalTapCat_Animated>();
        }
        
        if (targetScript != null)
        {
            targetScript.CheckAnimationSystem();
            targetScript.TestClick();
            
            Debug.Log("Animation system test triggered.");
            Debug.Log("Press Space or Left Mouse to see animation.");
        }
        else
        {
            Debug.LogError("No FinalTapCat_Animated script found!");
        }
    }
    
    [ContextMenu("Check Resources")]
    public void CheckResources()
    {
        Debug.Log("=== Resource Check ===");
        Debug.Log($"Checking for frames in: Resources/{animationPath}");
        
        int foundCount = 0;
        for (int i = 0; i < frameCount; i++)
        {
            string frameName = $"{animationPath}{i:00}";
            Sprite frame = Resources.Load<Sprite>(frameName);
            
            if (frame != null)
            {
                foundCount++;
                Debug.Log($"✓ Found: {frameName} ({frame.texture.width}x{frame.texture.height})");
            }
            else
            {
                Debug.Log($"✗ Missing: {frameName}");
            }
        }
        
        Debug.Log($"=== Summary: {foundCount}/{frameCount} frames found ===");
        
        if (foundCount == frameCount)
        {
            Debug.Log("✅ All animation frames are available!");
        }
        else
        {
            Debug.LogWarning($"⚠️ Missing {frameCount - foundCount} frames.");
        }
    }
    
    [ContextMenu("Create Test Cat Object")]
    public void CreateTestCatObject()
    {
        GameObject cat = GameObject.Find("TapCat");
        
        if (cat == null)
        {
            cat = new GameObject("TapCat");
            cat.transform.position = Vector3.zero;
            
            // Add SpriteRenderer
            SpriteRenderer spriteRenderer = cat.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 1;
            
            // Add FinalTapCat_Animated script
            FinalTapCat_Animated animatedScript = cat.AddComponent<FinalTapCat_Animated>();
            
            Debug.Log("Created test cat object with FinalTapCat_Animated script.");
            
            // Set this as target
            targetScript = animatedScript;
            
            // Load animation frames
            LoadAndAssignAnimationFrames();
        }
        else
        {
            Debug.Log("Cat object already exists.");
        }
    }
}