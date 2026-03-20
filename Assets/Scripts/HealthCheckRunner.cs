using UnityEngine;
using System.IO;
using System.Linq;

/// <summary>
/// Runner script to perform comprehensive health check
/// </summary>
public class HealthCheckRunner : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== TAPCAT PROJECT COMPREHENSIVE HEALTH CHECK ===");
        
        // Run all checks
        bool structureOk = CheckProjectStructure();
        bool scriptsOk = CheckScripts();
        bool resourcesOk = CheckResources();
        bool scenesOk = CheckScenes();
        bool dependenciesOk = CheckDependencies();
        
        // Summary
        Debug.Log("\n=== HEALTH CHECK SUMMARY ===");
        Debug.Log($"Project Structure: {(structureOk ? "PASS" : "FAIL")}");
        Debug.Log($"Scripts: {(scriptsOk ? "PASS" : "FAIL")}");
        Debug.Log($"Resources: {(resourcesOk ? "PASS" : "FAIL")}");
        Debug.Log($"Scenes: {(scenesOk ? "PASS" : "FAIL")}");
        Debug.Log($"Dependencies: {(dependenciesOk ? "PASS" : "FAIL")}");
        
        bool allPassed = structureOk && scriptsOk && resourcesOk && scenesOk && dependenciesOk;
        Debug.Log($"\nOVERALL STATUS: {(allPassed ? "✅ ALL CHECKS PASSED" : "❌ SOME CHECKS FAILED")}");
        
        if (!allPassed)
        {
            Debug.LogWarning("\n=== RECOMMENDED ACTIONS ===");
            Debug.LogWarning("1. Check for missing resources in Resources/ and Sprites/ folders");
            Debug.LogWarning("2. Verify all scripts compile without errors");
            Debug.LogWarning("3. Ensure main scene (TapCat.unity) exists");
            Debug.LogWarning("4. Check Package Manager for missing packages");
        }
    }
    
    private bool CheckProjectStructure()
    {
        Debug.Log("\n--- Project Structure Check ---");
        bool allOk = true;
        
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
            Debug.Log($"{dir.PadRight(30)} {(exists ? "✅" : "❌")}");
            if (!exists) allOk = false;
        }
        
        return allOk;
    }
    
    private bool CheckScripts()
    {
        Debug.Log("\n--- Scripts Check ---");
        bool allOk = true;
        
        string scriptsPath = Path.Combine(Application.dataPath, "Scripts");
        if (Directory.Exists(scriptsPath))
        {
            string[] csFiles = Directory.GetFiles(scriptsPath, "*.cs", SearchOption.AllDirectories);
            Debug.Log($"Total C# scripts: {csFiles.Length} ✅");
            
            // Check for key scripts
            string[] keyScripts = new string[]
            {
                "FinalTapCat_Animated.cs",
                "AutoTapCatSetup.cs",
                "SceneAutoStarter.cs",
                "AnimationManager.cs",
                "InputHandler.cs",
                "CounterUI.cs",
                "ProjectHealthCheck.cs",
                "CompilationTest.cs"
            };
            
            foreach (string script in keyScripts)
            {
                string scriptPath = Path.Combine(scriptsPath, script);
                bool exists = File.Exists(scriptPath);
                Debug.Log($"{script.PadRight(30)} {(exists ? "✅" : "❌")}");
                if (!exists) allOk = false;
            }
        }
        else
        {
            Debug.LogError("Scripts directory not found! ❌");
            allOk = false;
        }
        
        return allOk;
    }
    
    private bool CheckResources()
    {
        Debug.Log("\n--- Resources Check ---");
        bool allOk = true;
        
        // Check Resources/CatAnimation
        string resourcesAnimPath = Path.Combine(Application.dataPath, "Resources", "CatAnimation");
        if (Directory.Exists(resourcesAnimPath))
        {
            string[] pngFiles = Directory.GetFiles(resourcesAnimPath, "cat_anim_*.png");
            bool hasAllFrames = pngFiles.Length >= 10;
            Debug.Log($"Resources animation frames: {pngFiles.Length}/10 {(hasAllFrames ? "✅" : "❌")}");
            if (!hasAllFrames) allOk = false;
            
            if (pngFiles.Length > 0)
            {
                FileInfo sampleFile = new FileInfo(pngFiles[0]);
                bool isPlaceholder = sampleFile.Length < 1000;
                Debug.Log($"  Sample frame size: {sampleFile.Length} bytes {(isPlaceholder ? "⚠️ (placeholder)" : "✅")}");
            }
        }
        else
        {
            Debug.LogError("Resources/CatAnimation directory not found! ❌");
            allOk = false;
        }
        
        // Check Sprites/CatAnimation
        string spritesAnimPath = Path.Combine(Application.dataPath, "Sprites", "CatAnimation");
        if (Directory.Exists(spritesAnimPath))
        {
            string[] pngFiles = Directory.GetFiles(spritesAnimPath, "cat_anim_*.png");
            bool hasAllFrames = pngFiles.Length >= 10;
            Debug.Log($"Sprites animation frames: {pngFiles.Length}/10 {(hasAllFrames ? "✅" : "❌")}");
            if (!hasAllFrames) allOk = false;
        }
        else
        {
            Debug.LogError("Sprites/CatAnimation directory not found! ❌");
            allOk = false;
        }
        
        // Check PlaceholderCat
        string placeholderPath = Path.Combine(Application.dataPath, "Sprites", "PlaceholderCat.png");
        if (File.Exists(placeholderPath))
        {
            Debug.Log("PlaceholderCat.png: ✅");
        }
        else
        {
            Debug.LogWarning("PlaceholderCat.png: ❌ (some scripts may fail)");
        }
        
        return allOk;
    }
    
    private bool CheckScenes()
    {
        Debug.Log("\n--- Scenes Check ---");
        bool allOk = true;
        
        string scenesPath = Path.Combine(Application.dataPath, "Scenes");
        if (Directory.Exists(scenesPath))
        {
            string[] sceneFiles = Directory.GetFiles(scenesPath, "*.unity");
            Debug.Log($"Scene files in Scenes/: {sceneFiles.Length}");
        }
        
        // Check main scene
        string mainScenePath = Path.Combine(Application.dataPath, "TapCat.unity");
        if (File.Exists(mainScenePath))
        {
            Debug.Log("Main scene (TapCat.unity): ✅");
        }
        else
        {
            Debug.LogError("Main scene (TapCat.unity): ❌ - CRITICAL!");
            allOk = false;
        }
        
        return allOk;
    }
    
    private bool CheckDependencies()
    {
        Debug.Log("\n--- Dependencies Check ---");
        bool allOk = true;
        
        // Check for TextMesh Pro
        string tmproPath = Path.Combine(Application.dataPath, "TextMesh Pro");
        if (Directory.Exists(tmproPath))
        {
            Debug.Log("TextMesh Pro: ✅ (package installed)");
        }
        else
        {
            Debug.LogWarning("TextMesh Pro: ❌ (check Package Manager)");
        }
        
        // Check packages manifest
        string manifestPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), "Packages", "manifest.json");
        if (File.Exists(manifestPath))
        {
            Debug.Log("Packages manifest: ✅");
        }
        else
        {
            Debug.LogError("Packages manifest: ❌");
            allOk = false;
        }
        
        return allOk;
    }
    
    [ContextMenu("Run Full Health Check")]
    private void RunFullCheck()
    {
        Start();
    }
}