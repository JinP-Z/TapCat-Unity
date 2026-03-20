using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Fixes resource path issues by ensuring Resources folder has proper animation frames.
/// </summary>
public class ResourceFixer : MonoBehaviour
{
    [Header("Source and Target Paths")]
    [SerializeField] private string sourceSpritesPath = "Assets/Sprites/CatAnimation/";
    [SerializeField] private string targetResourcesPath = "Assets/Resources/CatAnimation/";
    
    [Header("Fix Options")]
    [SerializeField] private bool copySpritesToResources = true;
    [SerializeField] private bool createPlaceholderIfMissing = true;
    [SerializeField] private bool fixMetaFiles = true;
    
    void Start()
    {
        Debug.Log("Starting resource fixer...");
        
        if (copySpritesToResources)
        {
            CopySpritesToResources();
        }
        
        if (createPlaceholderIfMissing)
        {
            CreatePlaceholderIfMissing();
        }
        
        Debug.Log("Resource fixer completed.");
    }
    
    private void CopySpritesToResources()
    {
        Debug.Log($"Copying sprites from {sourceSpritesPath} to {targetResourcesPath}");
        
        // Ensure target directory exists
        if (!Directory.Exists(targetResourcesPath))
        {
            Directory.CreateDirectory(targetResourcesPath);
            Debug.Log($"Created directory: {targetResourcesPath}");
        }
        
        // Check source directory
        if (!Directory.Exists(sourceSpritesPath))
        {
            Debug.LogError($"Source directory not found: {sourceSpritesPath}");
            return;
        }
        
        // Copy PNG files
        string[] sourceFiles = Directory.GetFiles(sourceSpritesPath, "cat_anim_*.png");
        Debug.Log($"Found {sourceFiles.Length} animation frames in source");
        
        int copiedCount = 0;
        foreach (string sourceFile in sourceFiles)
        {
            string fileName = Path.GetFileName(sourceFile);
            string targetFile = Path.Combine(targetResourcesPath, fileName);
            
            // Only copy if source is larger (avoid copying placeholders to placeholders)
            FileInfo sourceInfo = new FileInfo(sourceFile);
            FileInfo targetInfo = new FileInfo(targetFile);
            
            if (!targetInfo.Exists || sourceInfo.Length > targetInfo.Length)
            {
                File.Copy(sourceFile, targetFile, true);
                copiedCount++;
                Debug.Log($"Copied: {fileName} ({sourceInfo.Length} bytes)");
            }
            else
            {
                Debug.Log($"Skipped: {fileName} (target already exists)");
            }
        }
        
        Debug.Log($"Copied {copiedCount} files to Resources folder.");
    }
    
    private void CreatePlaceholderIfMissing()
    {
        Debug.Log("Checking for placeholder cat sprite...");
        
        string placeholderPath = "Assets/Sprites/PlaceholderCat.png";
        if (!File.Exists(placeholderPath))
        {
            Debug.LogWarning($"Placeholder cat sprite not found: {placeholderPath}");
            Debug.Log("The game may fail to load if scripts reference this file.");
        }
        else
        {
            Debug.Log("Placeholder cat sprite found.");
        }
    }
    
    [ContextMenu("Run Resource Fixer")]
    private void RunResourceFixer()
    {
        Start();
    }
    
    [ContextMenu("Check Resource Status")]
    private void CheckResourceStatus()
    {
        Debug.Log("=== Resource Status Check ===");
        
        // Check Resources folder
        if (Directory.Exists(targetResourcesPath))
        {
            string[] resourceFiles = Directory.GetFiles(targetResourcesPath, "*.png");
            Debug.Log($"Resources/CatAnimation: {resourceFiles.Length} PNG files");
            
            foreach (string file in resourceFiles)
            {
                FileInfo info = new FileInfo(file);
                Debug.Log($"  {Path.GetFileName(file)}: {info.Length} bytes");
            }
        }
        else
        {
            Debug.LogWarning($"Resources folder not found: {targetResourcesPath}");
        }
        
        // Check Sprites folder
        if (Directory.Exists(sourceSpritesPath))
        {
            string[] spriteFiles = Directory.GetFiles(sourceSpritesPath, "*.png");
            Debug.Log($"Sprites/CatAnimation: {spriteFiles.Length} PNG files");
            
            if (spriteFiles.Length > 0)
            {
                FileInfo sampleFile = new FileInfo(spriteFiles[0]);
                Debug.Log($"  Sample file size: {sampleFile.Length} bytes");
            }
        }
        else
        {
            Debug.LogWarning($"Sprites folder not found: {sourceSpritesPath}");
        }
    }
}