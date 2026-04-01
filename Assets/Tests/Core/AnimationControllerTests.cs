using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using TapCat.Core;

namespace TapCat.Tests.Core
{
    public class AnimationControllerTests
    {
        [Test]
        public void PlayNextFrame_AdvancesAndLoops()
        {
            GameObject go = new GameObject("TestAnimationController");
            AnimationController controller = go.AddComponent<AnimationController>();
            controller.SetFramesForTesting(CreateTestFrames(3));

            controller.PlayNextFrame();
            Assert.AreEqual(1, controller.CurrentFrameIndex);

            controller.PlayNextFrame();
            Assert.AreEqual(2, controller.CurrentFrameIndex);

            controller.PlayNextFrame();
            Assert.AreEqual(0, controller.CurrentFrameIndex);
            Assert.AreEqual(1, controller.CompletedLoops);
        }

        [Test]
        public void StatusInfo_UsesExpectedFormat()
        {
            GameObject go = new GameObject("TestAnimationControllerStatus");
            AnimationController controller = go.AddComponent<AnimationController>();
            controller.SetFramesForTesting(CreateTestFrames(10));

            string status = controller.StatusInfo;
            Assert.IsTrue(status.Contains("循环"));
            Assert.IsTrue(status.Contains("帧"));
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