using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace TapCat
{
    /// <summary>
    /// Automated smoke tests for TapCat2D.
    /// </summary>
    public class TapCat2DTest : MonoBehaviour
    {
        [Header("Test Settings")]
        [SerializeField] private bool runAutomatedTests = true;
        [SerializeField] private float testDelay = 1.0f;

        private TapCat2D tapCat2D;
        private TapCat2DSetup setup;
        private TapCat2DSceneSetup sceneSetup;

        private void Start()
        {
            if (runAutomatedTests)
            {
                StartCoroutine(RunAllTests());
            }
        }

        private IEnumerator RunAllTests()
        {
            Debug.Log("=== TapCat2D Tests: Start ===");

            yield return new WaitForSeconds(testDelay);
            yield return StartCoroutine(TestEssentialComponents());

            yield return new WaitForSeconds(testDelay);
            yield return StartCoroutine(TestInputSystem());

            yield return new WaitForSeconds(testDelay);
            yield return StartCoroutine(TestAnimationSystem());

            yield return new WaitForSeconds(testDelay);
            yield return StartCoroutine(TestUISystem());

            yield return new WaitForSeconds(testDelay);
            yield return StartCoroutine(TestResetFunction());

            Debug.Log("=== TapCat2D Tests: Complete ===");
            Debug.Log("Controls: Space/Left Mouse = Play, R = Reset");
        }

        private IEnumerator TestEssentialComponents()
        {
            Debug.Log("Test 1: Essential components...");

            tapCat2D = FindObjectOfType<TapCat2D>();
            if (tapCat2D == null)
            {
                Debug.LogError("TapCat2D component not found.");
                yield break;
            }
            Debug.Log("TapCat2D found.");

            setup = FindObjectOfType<TapCat2DSetup>();
            if (setup == null)
            {
                Debug.LogWarning("TapCat2DSetup not found (optional).");
            }
            else
            {
                Debug.Log("TapCat2DSetup found.");
            }

            sceneSetup = FindObjectOfType<TapCat2DSceneSetup>();
            if (sceneSetup == null)
            {
                Debug.LogWarning("TapCat2DSceneSetup not found (optional).");
            }
            else
            {
                Debug.Log("TapCat2DSceneSetup found.");
            }

            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found.");
                yield break;
            }

            if (!mainCamera.orthographic)
            {
                Debug.LogWarning("Main camera is not orthographic.");
            }
            else
            {
                Debug.Log("Main camera is orthographic.");
            }

            Debug.Log("Test 1 complete.");
            yield return null;
        }

        private IEnumerator TestInputSystem()
        {
            Debug.Log("Test 2: Input system...");

            if (tapCat2D == null)
            {
                Debug.LogError("TapCat2D is missing.");
                yield break;
            }

            if (tapCat2D.IsAnimating())
            {
                Debug.Log("Animation running. Waiting to finish...");
                while (tapCat2D.IsAnimating())
                {
                    yield return null;
                }
            }

            int initialCount = tapCat2D.GetTapCount();
            tapCat2D.StartCatAnimation();
            yield return new WaitForSeconds(0.1f);

            if (tapCat2D.IsAnimating())
            {
                Debug.Log("Input triggered animation.");
            }
            else
            {
                Debug.LogError("Input did not trigger animation.");
            }

            while (tapCat2D.IsAnimating())
            {
                yield return null;
            }

            int newCount = tapCat2D.GetTapCount();
            if (newCount > initialCount)
            {
                Debug.Log($"Tap count increased: {initialCount} -> {newCount}");
            }
            else
            {
                Debug.LogError($"Tap count did not increase: {initialCount} -> {newCount}");
            }

            Debug.Log("Test 2 complete.");
        }

        private IEnumerator TestAnimationSystem()
        {
            Debug.Log("Test 3: Animation system...");

            if (tapCat2D == null)
            {
                Debug.LogError("TapCat2D is missing.");
                yield break;
            }

            SpriteRenderer spriteRenderer = tapCat2D.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("SpriteRenderer not found.");
                yield break;
            }

            tapCat2D.StartCatAnimation();
            yield return new WaitForSeconds(0.05f);

            if (tapCat2D.IsAnimating())
            {
                Debug.Log("Animation started.");

                float animationTime = 0f;
                while (tapCat2D.IsAnimating() && animationTime < 2.0f)
                {
                    animationTime += Time.deltaTime;
                    yield return null;
                }

                if (!tapCat2D.IsAnimating())
                {
                    Debug.Log("Animation completed.");
                }
                else
                {
                    Debug.LogWarning("Animation taking longer than expected.");
                }
            }
            else
            {
                Debug.LogError("Animation did not start.");
            }

            Debug.Log("Test 3 complete.");
        }

        private IEnumerator TestUISystem()
        {
            Debug.Log("Test 4: UI system...");

            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogWarning("Canvas not found.");
            }
            else
            {
                int uiElementCount = canvas.transform.childCount;
                Debug.Log($"Canvas child count: {uiElementCount}");
            }

            Text[] textComponents = FindObjectsOfType<Text>();
            if (textComponents.Length > 0)
            {
                Debug.Log($"Text components found: {textComponents.Length}");
            }
            else
            {
                Debug.LogWarning("No Text components found.");
            }

            Debug.Log("Test 4 complete.");
            yield return null;
        }

        private IEnumerator TestResetFunction()
        {
            Debug.Log("Test 5: Reset function...");

            if (tapCat2D == null)
            {
                Debug.LogError("TapCat2D is missing.");
                yield break;
            }

            int beforeResetCount = tapCat2D.GetTapCount();
            tapCat2D.ResetGame();
            yield return new WaitForSeconds(0.5f);

            int afterResetCount = tapCat2D.GetTapCount();
            bool isAnimating = tapCat2D.IsAnimating();

            if (afterResetCount == 0 && !isAnimating)
            {
                Debug.Log("Reset OK.");
            }
            else
            {
                Debug.LogError($"Reset failed. Count: {afterResetCount}, Animating: {isAnimating}");
            }

            Debug.Log("Test 5 complete.");
        }

        [ContextMenu("Run Manual Tests")]
        private void RunManualTest()
        {
            if (Application.isPlaying)
            {
                StartCoroutine(RunAllTests());
            }
            else
            {
                Debug.Log("Run tests in Play Mode.");
            }
        }

        [ContextMenu("Quick Validation")]
        private void QuickValidation()
        {
            Debug.Log("=== TapCat2D Quick Validation ===");

            bool hasTapCat2D = FindObjectOfType<TapCat2D>() != null;
            bool hasCamera = Camera.main != null;
            bool hasCanvas = FindObjectOfType<Canvas>() != null;

            Debug.Log("Core components:");
            Debug.Log($"- TapCat2D: {(hasTapCat2D ? "OK" : "Missing")}");
            Debug.Log($"- Main Camera: {(hasCamera ? "OK" : "Missing")}");
            Debug.Log($"- Canvas: {(hasCanvas ? "OK" : "Missing")}");

            if (hasTapCat2D && hasCamera)
            {
                Debug.Log("Basic validation passed.");
                Debug.Log("Press Play and use Space to test animation.");
            }
            else
            {
                Debug.LogError("Validation failed. Missing required components.");
            }
        }
    }
}
