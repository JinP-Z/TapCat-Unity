using UnityEngine;
using UnityEngine.SceneManagement;
using TapCat.Input;
using TapCat.UI;

namespace TapCat.Core
{
    /// <summary>
    /// Bootstraps Phase 3 systems without manual scene edits.
    /// Ensures input + animation wiring and disables legacy handlers.
    /// </summary>
    public class TapCatPhase3Bootstrap : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitializeAfterSceneLoad()
        {
            if (FindObjectOfType<TapCatPhase3Bootstrap>() != null)
            {
                return;
            }

            GameObject bootstrapObj = new GameObject("TapCatPhase3Bootstrap");
            DontDestroyOnLoad(bootstrapObj);
            bootstrapObj.AddComponent<TapCatPhase3Bootstrap>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Awake()
        {
            SetupScene();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetupScene();
        }

        private void SetupScene()
        {
            DisableLegacyComponents();

            InputManager inputManager = FindOrCreateInputManager();
            TapCat.TapCatController tapCatController = FindObjectOfType<TapCat.TapCatController>();
            GameObject catRoot = ResolveCatRoot(tapCatController);
            AnimationController animationController = EnsureAnimationController(catRoot);
            UIManager uiManager = EnsureUIManager();

            EnsurePhase3Manager(inputManager, animationController, tapCatController, uiManager);
        }

        private static void DisableLegacyComponents()
        {
            TapCat.InputHandler[] inputHandlers = FindObjectsOfType<TapCat.InputHandler>(true);
            for (int i = 0; i < inputHandlers.Length; i++)
            {
                inputHandlers[i].enabled = false;
            }

            TapCat.TapCatSpriteSequenceAnimator[] legacyAnimators = FindObjectsOfType<TapCat.TapCatSpriteSequenceAnimator>(true);
            for (int i = 0; i < legacyAnimators.Length; i++)
            {
                legacyAnimators[i].enabled = false;
            }
        }

        private static InputManager FindOrCreateInputManager()
        {
            InputManager inputManager = FindObjectOfType<InputManager>();
            if (inputManager != null)
            {
                return inputManager;
            }

            GameObject inputObj = new GameObject("InputManager");
            inputManager = inputObj.AddComponent<InputManager>();
            return inputManager;
        }

        private static GameObject ResolveCatRoot(TapCat.TapCatController tapCatController)
        {
            if (tapCatController != null)
            {
                return tapCatController.gameObject;
            }

            GameObject named = GameObject.Find("TapCat");
            if (named != null)
            {
                return named;
            }

            SpriteRenderer existingRenderer = FindObjectOfType<SpriteRenderer>();
            if (existingRenderer != null)
            {
                return existingRenderer.gameObject;
            }

            GameObject catRoot = new GameObject("TapCat");
            return catRoot;
        }

        private static AnimationController EnsureAnimationController(GameObject catRoot)
        {
            AnimationController controller = catRoot.GetComponent<AnimationController>();
            if (controller == null)
            {
                controller = catRoot.AddComponent<AnimationController>();
            }

            return controller;
        }

        private static UIManager EnsureUIManager()
        {
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                return uiManager;
            }

            GameObject uiObject = new GameObject("UIManager");
            DontDestroyOnLoad(uiObject);
            uiManager = uiObject.AddComponent<UIManager>();
            return uiManager;
        }

        private static void EnsurePhase3Manager(InputManager inputManager, AnimationController animationController, TapCat.TapCatController tapCatController, UIManager uiManager)
        {
            TapCatPhase3Manager manager = FindObjectOfType<TapCatPhase3Manager>();
            if (manager == null)
            {
                GameObject managerObj = new GameObject("TapCatPhase3Manager");
                manager = managerObj.AddComponent<TapCatPhase3Manager>();
            }

            manager.Inject(inputManager, animationController, tapCatController, uiManager);
        }
    }
}
