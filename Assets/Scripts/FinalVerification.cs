using UnityEngine;
using System.IO;

/// <summary>
/// Final verification that all path issues have been fixed.
/// </summary>
public class FinalVerification : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== FINAL VERIFICATION CHECK ===");
        
        bool allChecksPassed = true;
        
        // Check 1: Resources/CatAnimation has actual files
        allChecksPassed &= CheckResourcesAnimation();
        
        // Check 2: PlaceholderCat exists in Resources
        allChecksPassed &= CheckPlaceholderCat();
        
        // Check 3: Key directories exist
        allChecksPassed &= CheckKeyDirectories();
        
        // Check 4: Main scene exists
        allChecksPassed &= CheckMainScene();
        
        // Summary
        Debug.Log($"\n=== VERIFICATION SUMMARY ===");
        if (allChecksPassed)
        {
            Debug.Log("✅ ALL CHECKS PASSED - Project path issues are fixed!");
            Debug.Log("The project should now compile and run correctly.");
            Debug.Log("Please test in Unity Play mode to confirm.");
        }
        else
        {
            Debug.LogError("❌ SOME CHECKS FAILED - Review the issues above.");
            Debug.Log("Run the ResourceFixer and PathFixer scripts to fix remaining issues.");
        }
    }
    
    private bool CheckResourcesAnimation()
    {
        string path = Path.Combine(Application.dataPath, "Resources", "CatAnimation");
        
        if (!Directory.Exists(path))
        {
            Debug.LogError("✗ Resources/CatAnimation directory not found");
            return false;
        }
        
        string[] files = Directory.GetFiles(path, "cat_anim_*.png");
        if (files.Length < 10)
        {
            Debug.LogError($"✗ Incomplete animation frames: {files.Length}/10");
            return false;
        }
        
        // Check file sizes (should be > 1000 bytes, not placeholder size)
        int smallFileCount = 0;
        foreach (string file in files)
        {
            FileInfo info = new FileInfo(file);
            if (info.Length < 1000)
            {
                smallFileCount++;
            }
        }
        
        if (smallFileCount > 0)
        {
            Debug.LogError($"✗ Found {smallFileCount} placeholder files (too small)");
            return false;
        }
        
        Debug.Log($"✓ Resources/CatAnimation: {files.Length} proper animation files");
        return true;
    }
    
    private bool CheckPlaceholderCat()
    {
        string path = Path.Combine(Application.dataPath, "Resources", "PlaceholderCat.png");
        
        if (!File.Exists(path))
        {
            Debug.LogError("✗ Resources/PlaceholderCat.png not found");
            Debug.Log("  Scripts may fail to load Resources.Load(\"PlaceholderCat\")");
            return false;
        }
        
        Debug.Log("✓ Resources/PlaceholderCat.png exists");
        return true;
    }
    
    private bool CheckKeyDirectories()
    {
        string[] dirs = new string[]
        {
            "Assets/Resources",
            "Assets/Sprites",
            "Assets/Scripts",
            "Assets/Scenes",
            "Assets/Animators"
        };
        
        bool allExist = true;
        foreach (string dir in dirs)
        {
            string fullPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), dir);
            if (!Directory.Exists(fullPath))
            {
                Debug.LogError($"✗ {dir} not found");
                allExist = false;
            }
        }
        
        if (allExist)
        {
            Debug.Log("✓ All key directories exist");
        }
        
        return allExist;
    }
    
    private bool CheckMainScene()
    {
        string path = Path.Combine(Application.dataPath, "TapCat.unity");
        
        if (!File.Exists(path))
        {
            Debug.LogError("✗ Main scene TapCat.unity not found");
            return false;
        }
        
        Debug.Log("✓ Main scene TapCat.unity exists");
        return true;
    }
    
    [ContextMenu("Run Final Verification")]
    private void RunVerification()
    {
        Start();
    }
}