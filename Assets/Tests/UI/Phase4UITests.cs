using System.Reflection;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using UnityEngine.UI;
using TapCat.Core;
using TapCat.Input;
using TapCat.UI;

namespace TapCat.Tests.UI
{
    public class Phase4UITests
    {
        [Test]
        public void ClickCounterUI_UpdatesWhenInputCountChanges()
        {
            GameObject inputObj = new GameObject("InputManager");
            InputManager inputManager = inputObj.AddComponent<InputManager>();

            GameObject uiObj = new GameObject("ClickCounterUI", typeof(RectTransform));
            ClickCounterUI ui = uiObj.AddComponent<ClickCounterUI>();
            ui.Bind(inputManager);

            InvokePrivate(inputManager, "TriggerInput");

            Text text = uiObj.GetComponent<Text>();
            Assert.IsNotNull(text);
            Assert.AreEqual("点击: 1", text.text);
        }

        [Test]
        public void StatusIndicatorUI_UpdatesWhenAnimationStatusChanges()
        {
            GameObject animObj = new GameObject("AnimationController");
            AnimationController controller = animObj.AddComponent<AnimationController>();
            controller.SetFramesForTesting(CreateTestFrames(3));

            GameObject uiObj = new GameObject("StatusIndicatorUI", typeof(RectTransform));
            StatusIndicatorUI ui = uiObj.AddComponent<StatusIndicatorUI>();
            ui.Bind(controller);

            controller.PlayNextFrame();

            Text text = uiObj.GetComponent<Text>();
            Assert.IsNotNull(text);
            StringAssert.Contains("循环", text.text);
            StringAssert.Contains("帧", text.text);
            StringAssert.Contains("2/3", text.text);
        }

        [Test]
        public void UIManager_CreatesWidgetsOnInitialize()
        {
            GameObject inputObj = new GameObject("InputManager");
            InputManager inputManager = inputObj.AddComponent<InputManager>();

            GameObject animObj = new GameObject("AnimationController");
            AnimationController controller = animObj.AddComponent<AnimationController>();
            controller.SetFramesForTesting(CreateTestFrames(2));

            GameObject uiManagerObj = new GameObject("UIManager");
            UIManager uiManager = uiManagerObj.AddComponent<UIManager>();
            uiManager.Initialize(inputManager, controller);

            ClickCounterUI clickCounter = uiManagerObj.GetComponentInChildren<ClickCounterUI>();
            StatusIndicatorUI statusIndicator = uiManagerObj.GetComponentInChildren<StatusIndicatorUI>();

            Assert.IsNotNull(clickCounter);
            Assert.IsNotNull(statusIndicator);
            Assert.IsTrue(uiManager.IsInitialized);
        }

        private static void InvokePrivate(object target, string methodName)
        {
            MethodInfo method = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(method, $"Missing method {methodName}");
            method.Invoke(target, null);
        }

        private static Sprite[] CreateTestFrames(int count)
        {
            Sprite[] frames = new Sprite[count];
            for (int i = 0; i < count; i++)
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.SetPixel(0, 0, Color.white);
                tex.SetPixel(1, 0, Color.white);
                tex.SetPixel(0, 1, Color.white);
                tex.SetPixel(1, 1, Color.white);
                tex.Apply();
                frames[i] = Sprite.Create(tex, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 100f);
            }
            return frames;
        }
    }
}