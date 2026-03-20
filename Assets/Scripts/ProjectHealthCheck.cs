using UnityEngine;
using System.IO;
using System.Linq;

/// <summary>
/// Comprehensive health check for the TapCat Unity project.
/// </summary>
public class ProjectHealthCheck : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== TAPCAT PROJECT HEALTH CHECK ===");
        
        CheckProjectStructure();
        CheckScripts();
        CheckResources();
        CheckScenes();
        CheckDependencies();
        
        Debug.Log("=== HEALTH CHECK COMPLETE ===");
    }
    
    private void CheckProjectStructure()
    {
        Debug.Log("\n--- Project Structure ---");
        
        string[] requiredDirs = new string[]
        {
            "Assets",
            "Assets/Resources",
            "Assets/Sprites",
            "Assets/Scripts",
            "Assets/Scenes",
            "Assets/Animators",
            "Assets/TextMesh Pro"
        };
        
        foreach (string dir in requiredDirs)
        {
            string fullPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), dir);
            bool exists = Directory.Exists(fullPath);
            Debug.Log($"{dir.PadRight(30)} {(exists ? "✓" : "✗")}");
        }
    }
    
    private void CheckScripts()
    {
        Debug.Log("\n--- Scripts Check ---");
        
        string scriptsPath = Path.Combine(Application.dataPath, "Scripts");
        if (Directory.Exists(scriptsPath))
        {
            string[] csFiles = Directory.GetFiles(scriptsPath, "*.cs", SearchOption.AllDirectories);
            Debug.Log($"Total C# scripts: {csFiles.Length}");
            
            // Check for key scripts
            string[] keyScripts = new string[]
            {
                "TapCat2D.cs",
                "FinalTapCat_Animated.cs",
                "AutoTapCatSetup.cs",
                "SceneAutoStarter.cs",
                "AnimationManager.cs",
                "InputHandler.cs",
                "CounterUI.cs"
            };
            
            foreach (string script in keyScripts)
            {
                string scriptPath = Path.Combine(scriptsPath, script);
                bool exists = File.Exists(scriptPath);
                Debug.Log($"{script.PadRight(30)} {(exists ? "✓" : "✗")}");
            }
        }
        else
        {
            Debug.LogError("Scripts directory not found!");
        }
    }
    
    private void CheckResources()
    {
        Debug.Log("\n--- Resources Check ---");
        
        // Check Resources/CatAnimation
        string resourcesAnimPath = Path.Combine(Application.dataPath, "Resources", "CatAnimation");
        if (Directory.Exists(resourcesAnimPath))
        {
            string[] pngFiles = Directory.GetFiles(resourcesAnimPath, "cat_anim_*.png");
            Debug.Log($"Resources animation frames: {pngFiles.Length}/10");
            
            if (pngFiles.Length > 0)
            {
                FileInfo sampleFile = new FileInfo(pngFiles[0]);
                Debug.Log($"  Sample frame size: {sampleFile.Length} bytes");
                
                if (sampleFile.Length < 1000)
                {
                    Debug.LogWarning("  WARNING: Files appear to be placeholders (very small)");
                }
            }
        }
        else
        {
            Debug.LogWarning("Resources/CatAnimation directory not found");
        }
        
        // Check Sprites/CatAnimation
        string spritesAnimPath = Path.Combine(Application.dataPath, "Sprites", "CatAnimation");
        if (Directory.Exists(spritesAnimPath))
        {
            string[] pngFiles = Directory.GetFiles(spritesAnimPath, "cat_anim_*.png");
            Debug.Log($"Sprites animation frames: {pngFiles.Length}/10");
            
            if (pngFiles.Length >= 10)
            {
                Debug.Log("  ✓ Animation frames complete");
            }
            else if (pngFiles.Length > 0)
            {
                Debug.LogWarning($"  WARNING: Incomplete animation frames ({pngFiles.Length}/10)");
            }
        }
        
        // Check PlaceholderCat
        string placeholderPath = Path.Combine(Application.dataPath, "Sprites", "PlaceholderCat.png");
        if (File.Exists(placeholderPath))
        {
            Debug.Log("PlaceholderCat.png: ✓");
        }
        else
        {
            Debug.LogWarning("PlaceholderCat.png: ✗ (some scripts may fail)");
        }
    }
    
    private void CheckScenes()
    {
        Debug.Log("\n--- Scenes Check ---");
        
        string scenesPath = Path.Combine(Application.dataPath, "Scenes");
        if (Directory.Exists(scenesPath))
        {
            string[] sceneFiles = Directory.GetFiles(scenesPath, "*.unity");
            Debug.Log($"Scene files: {sceneFiles.Length}");
        }
        
        // Check main scene
        string mainScenePath = Path.Combine(Application.dataPath, "TapCat.unity");
        if (File.Exists(mainScenePath))
        {
            Debug.Log("Main scene (TapCat.unity): ✓");
        }
        else
        {
            Debug.LogError("Main scene (TapCat.unity): ✗ - CRITICAL!");
        }
    }
    
    private void CheckDependencies()
    {
        Debug.Log("\n--- Dependencies Check ---");
        
        // Check for TextMesh Pro
        string tmproPath = Path.Combine(Application.dataPath, "TextMesh Pro");
        if (Directory.Exists(tmproPath))
        {
            Debug.Log("TextMesh Pro: ✓ (package installed)");
        }
        else
        {
            Debug.LogWarning("TextMesh Pro: ✗ (check Package Manager)");
        }
        
        // Check packages manifest
        string manifestPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), "Packages", "manifest.json");
        if (File.Exists(manifestPath))
        {
            Debug.Log("Packages manifest: ✓");
        }
    }
    
    [ContextMenu("Run Health Check")]
    private void RunHealthCheck()
    {
        Start();
    }
    
    [ContextMenu("Generate Fix Report")]
    private void GenerateFixReport()
    {
        Debug.Log("=== FIX RECOMMENDATIONS ===");
        Debug.Log("1. Ensure Resources/CatAnimation has actual animation frames (not placeholders)");
        Debug.Log("2. Copy files from Sprites/CatAnimation to Resources/CatAnimation if needed");
        Debug.Log("3. Verify all scripts compile without errors");
        Debug.Log("4. Test the main scene (TapCat.unity) in Play Mode");
        Debug.Log("5. Commit all changes to git repository");
    }
}