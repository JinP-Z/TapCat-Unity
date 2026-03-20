using UnityEngine;

/// <summary>
/// Auto setup for TapCat. Creates camera and controller when the scene starts.
/// </summary>
public class AutoTapCatSetup : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Starting TapCat auto setup...");

        SetupCamera();
        CreateTapCatController();
        CheckAnimationResources();

        Debug.Log("TapCat auto setup complete.");
        Debug.Log("Press Play to test the game.");

        Destroy(gameObject, 2f);
    }

    private void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            GameObject cameraObj = new GameObject("Main Camera");
            cameraObj.tag = "MainCamera";
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.transform.position = new Vector3(0, 0, -10);
            Debug.Log("Main camera created.");
        }
        else
        {
            mainCamera.transform.position = new Vector3(0, 0, -10);
            Debug.Log("Main camera position updated.");
        }
    }

    private void CreateTapCatController()
    {
        FinalTapCat_Animated existingController = FindObjectOfType<FinalTapCat_Animated>();
        if (existingController != null)
        {
            Debug.Log("TapCat controller already exists.");
            return;
        }

        GameObject controllerObj = new GameObject("TapCatController");
        controllerObj.AddComponent<FinalTapCat_Animated>();

        Debug.Log("TapCat controller created.");
    }

    private void CheckAnimationResources()
    {
        Debug.Log("Checking animation resources...");

        string spritePath = Application.dataPath + "/Sprites/CatAnimation/";
        if (System.IO.Directory.Exists(spritePath))
        {
            int pngCount = System.IO.Directory.GetFiles(spritePath, "*.png").Length;

            if (pngCount >= 10)
            {
                Debug.Log($"Found {pngCount} PNG files in Assets/Sprites/CatAnimation/.");
                Debug.Log("Make sure import settings are Sprite (2D and UI).");
            }
            else if (pngCount > 0)
            {
                Debug.LogWarning($"Animation frames incomplete: {pngCount}/10.");
            }
            else
            {
                Debug.LogWarning("Animation folder exists but is empty.");
            }
        }
        else
        {
            Debug.LogWarning("Animation folder not found: Assets/Sprites/CatAnimation/");
        }
    }

    [ContextMenu("Run Auto Setup")]
    private void RunAutoSetup()
    {
        Start();
    }

    [ContextMenu("Check Current Scene")]
    private void CheckCurrentScene()
    {
        Debug.Log("=== Scene Check ===");

        Camera mainCamera = Camera.main;
        Debug.Log(mainCamera != null ? "Main camera: OK" : "Main camera: Missing");

        FinalTapCat_Animated controller = FindObjectOfType<FinalTapCat_Animated>();
        if (controller != null)
        {
            Debug.Log("TapCat controller: OK");

            GameObject cat = GameObject.Find("TapCat");
            Debug.Log(cat != null ? "Cat object: OK" : "Cat object: Missing");
        }
        else
        {
            Debug.Log("TapCat controller: Missing");
        }

        string spritePath = Application.dataPath + "/Sprites/CatAnimation/";
        if (System.IO.Directory.Exists(spritePath))
        {
            int pngCount = System.IO.Directory.GetFiles(spritePath, "*.png").Length;
            Debug.Log($"Animation frames: {pngCount}/10");
        }
        else
        {
            Debug.Log("Animation folder missing.");
        }

        Debug.Log("=== Check Complete ===");
    }
}
