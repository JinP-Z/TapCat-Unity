using UnityEngine;
using UnityEngine.UI;
using TapCat.Input;

namespace TapCat.UI
{
    /// <summary>
    /// Displays total input count in the UI.
    /// Performance & lifecycle: event-driven updates, no Update loop.
    /// </summary>
    [DisallowMultipleComponent]
    public class ClickCounterUI : MonoBehaviour
    {
        private const string DefaultFormat = "点击: {0}";

        [Header("UI")]
        [SerializeField] private Text countText;
        [SerializeField] private string textFormat = DefaultFormat;
        [SerializeField] private Color textColor = Color.white;
        [SerializeField, Range(10, 48)] private int fontSize = 18;

        private InputManager inputManager;

        /// <summary>
        /// Bind to the input manager for event-driven updates.
        /// </summary>
        public void Bind(InputManager manager)
        {
            if (inputManager != null)
            {
                inputManager.OnInputCountChanged -= HandleCountChanged;
            }

            inputManager = manager;
            EnsureText();

            if (inputManager != null)
            {
                inputManager.OnInputCountChanged += HandleCountChanged;
                HandleCountChanged(inputManager.TotalInputCount);
            }
            else
            {
                UpdateCount(0);
            }
        }

        /// <summary>
        /// Update the displayed count explicitly.
        /// </summary>
        public void UpdateCount(int totalCount)
        {
            EnsureText();
            if (countText == null)
            {
                return;
            }

            string format = string.IsNullOrWhiteSpace(textFormat) ? DefaultFormat : textFormat;
            countText.text = string.Format(format, totalCount);
        }

        private void Awake()
        {
            EnsureText();
        }

        private void OnDestroy()
        {
            if (inputManager != null)
            {
                inputManager.OnInputCountChanged -= HandleCountChanged;
            }
        }

        private void HandleCountChanged(int totalCount)
        {
            UpdateCount(totalCount);
        }

        private void EnsureText()
        {
            if (countText == null)
            {
                countText = GetComponent<Text>();
                if (countText == null)
                {
                    countText = gameObject.AddComponent<Text>();
                }
            }

            countText.raycastTarget = false;
            countText.color = textColor;
            countText.fontSize = fontSize;
            if (countText.font == null)
            {
                countText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            countText.alignment = TextAnchor.MiddleLeft;
        }
    }
}
