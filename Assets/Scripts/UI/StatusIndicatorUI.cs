using UnityEngine;
using UnityEngine.UI;
using TapCat.Core;

namespace TapCat.UI
{
    /// <summary>
    /// Displays animation status (loop count + frame progress).
    /// Performance & lifecycle: event-driven updates, no Update loop.
    /// </summary>
    [DisallowMultipleComponent]
    public class StatusIndicatorUI : MonoBehaviour
    {
        private const string DefaultFormat = "{0}";

        [Header("UI")]
        [SerializeField] private Text statusText;
        [SerializeField] private string statusFormat = DefaultFormat;
        [SerializeField] private Color textColor = Color.white;
        [SerializeField, Range(10, 48)] private int fontSize = 18;

        private AnimationController animationController;

        /// <summary>
        /// Bind to the animation controller for event-driven updates.
        /// </summary>
        public void Bind(AnimationController controller)
        {
            if (animationController != null)
            {
                animationController.OnStatusChanged -= HandleStatusChanged;
            }

            animationController = controller;
            EnsureText();

            if (animationController != null)
            {
                animationController.OnStatusChanged += HandleStatusChanged;
                UpdateStatus(animationController.StatusInfo);
            }
            else
            {
                UpdateStatus(string.Empty);
            }
        }

        /// <summary>
        /// Update the displayed status explicitly.
        /// </summary>
        public void UpdateStatus(string status)
        {
            EnsureText();
            if (statusText == null)
            {
                return;
            }

            string format = string.IsNullOrWhiteSpace(statusFormat) ? DefaultFormat : statusFormat;
            statusText.text = string.Format(format, status ?? string.Empty);
        }

        private void Awake()
        {
            EnsureText();
        }

        private void OnDestroy()
        {
            if (animationController != null)
            {
                animationController.OnStatusChanged -= HandleStatusChanged;
            }
        }

        private void HandleStatusChanged(string status)
        {
            UpdateStatus(status);
        }

        private void EnsureText()
        {
            if (statusText == null)
            {
                statusText = GetComponent<Text>();
                if (statusText == null)
                {
                    statusText = gameObject.AddComponent<Text>();
                }
            }

            statusText.raycastTarget = false;
            statusText.color = textColor;
            statusText.fontSize = fontSize;
            if (statusText.font == null)
            {
                statusText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }
            statusText.alignment = TextAnchor.MiddleLeft;
        }
    }
}
