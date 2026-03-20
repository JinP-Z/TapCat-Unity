using UnityEngine;

/// <summary>
/// Simple animated TapCat example.
/// </summary>
public class TapCatSimpleAnimated : MonoBehaviour
{
    private int clicks = 0;
    private bool isAnimating = false;

    private void Start()
    {
        Debug.Log("TapCatSimpleAnimated started.");
        Debug.Log("Controls: Space/Left Mouse = Play, R = Reset");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            clicks++;
            isAnimating = true;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            clicks = 0;
            isAnimating = false;
            Debug.Log("Game reset.");
        }

        if (isAnimating)
        {
            // Placeholder for animation logic.
            isAnimating = false;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        GUI.Box(new Rect(10, 10, 250, 70), $"Taps: {clicks}\nPress R to reset", style);
    }
}
