using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Fixes path issues in the TapCat project.
/// </summary>
public class PathFixer : MonoBehaviour
{
    [Header("Fix Options")]
    [SerializeField] private bool fixResourcesPaths = true;
    [SerializeField] private bool fixSpritePaths = true;
    [SerializeField] private bool createMissingFolders = true;
    
    [Header("Paths")]
    [SerializeField] private string resourcesPath = "Assets/Resources/";
    [SerializeField] private string spritesPath = "Assets/Sprites/";
    [SerializeField] private string animationFolderName = "CatAnimation";
    
    void Start()
    {
        Debug.Log("Starting path fixer...");
        
        if (fixResourcesPaths)
        {
            FixResourcesPaths();
        }
        
        if (fixSpritePaths)
        {
            FixSpritePaths();
        }
        
        if (createMissingFolders)
        {
            CreateMissingFolders();
        }
        
        Debug.Log("Path fixer completed.");
    }
    
    private void FixResourcesPaths()
    {
        Debug.Log("Fixing Resources paths...");
        
        string resourcesAnimationPath = Path.Combine(resourcesPath, animationFolderName);
        
        if (Directory.Exists(resourcesAnimationPath))
        {
            Debug.Log($"Resources animation path exists: {resourcesAnimationPath}");
            
            // Check if files are placeholders
            string[] pngFiles = Directory.GetFiles(resourcesAnimationPath, "*.png");
            Debug.Log($"Found {pngFiles.Length} PNG files in Resources/CatAnimation");
            
            foreach (string file in pngFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Length < 1000) // Small files are likely placeholders
                {
                    Debug.Log($"File {fileInfo.Name} is small ({fileInfo.Length} bytes), likely a placeholder");
                }
            }
        }
        else
        {
            Debug.LogWarning($"Resources animation path not found: {resourcesAnimationPath}");
        }
    }
    
    private void FixSpritePaths()
    {
        Debug.Log("Fixing Sprite paths...");
        
        string spritesAnimationPath = Path.Combine(spritesPath, animationFolderName);
        
        if (Directory.Exists(spritesAnimationPath))
        {
            Debug.Log($"Sprites animation path exists: {spritesAnimationPath}");
            
            string[] pngFiles = Directory.GetFiles(spritesAnimationPath, "*.png");
            Debug.Log($"Found {pngFiles.Length} PNG files in Sprites/CatAnimation");
            
            if (pngFiles.Length >= 10)
            {
                Debug.Log("Animation frames are complete in Sprites folder.");
            }
            else
            {
                Debug.LogWarning($"Animation frames incomplete: {pngFiles.Length}/10");
            }
        }
        else
        {
            Debug.LogWarning($"Sprites animation path not found: {spritesAnimationPath}");
        }
    }
    
    private void CreateMissingFolders()
    {
        Debug.Log("Creating missing folders...");
        
        string[] foldersToCheck = new string[]
        {
            resourcesPath,
            Path.Combine(resourcesPath, animationFolderName),
            spritesPath,
            Path.Combine(spritesPath, animationFolderName)
        };
        
        foreach (string folder in foldersToCheck)
        {
            if (!Directory.Exists(folder))
            {
                Debug.Log($"Creating folder: {folder}");
                Directory.CreateDirectory(folder);
            }
        }
    }
    
    [ContextMenu("Run Path Fixer")]
    private void RunPathFixer()
    {
        Start();
    }
    
    [ContextMenu("Check Project Structure")]
    private void CheckProjectStructure()
    {
        Debug.Log("=== Project Structure Check ===");
        Debug.Log($"Project Path: {Application.dataPath}");
        
        // Check key directories
        string[] keyDirs = new string[]
        {
            "Assets",
            "Assets/Resources",
            "Assets/Resources/CatAnimation",
            "Assets/Sprites", 
            "Assets/Sprites/CatAnimation",
            "Assets/Scripts",
            "Assets/Scenes",
            "Assets/Animators"
        };
        
        foreach (string dir in keyDirs)
        {
            string fullPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), dir);
            bool exists = Directory.Exists(fullPath);
            Debug.Log($"{dir}: {(exists ? "OK" : "MISSING")}");
        }
        
        // Check key files
        string[] keyFiles = new string[]
        {
            "Assets/TapCat.unity",
            "Assets/Sprites/PlaceholderCat.png",
            "Assets/Animators/CatAnimator.controller"
        };
        
        foreach (string file in keyFiles)
        {
            string fullPath = Path.Combine(Application.dataPath.Replace("/Assets", ""), file);
            bool exists = File.Exists(fullPath);
            Debug.Log($"{file}: {(exists ? "OK" : "MISSING")}");
        }
    }
}