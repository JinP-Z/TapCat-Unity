using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using TapCat.Utils;

namespace TapCat.Tests.Utils
{
    public class ResourceLoaderTests
    {
        [Test]
        public void LoadSpritesSync_DefaultPath_LoadsFrames()
        {
            int loaded;
            string error;
            Sprite[] frames = ResourceLoader.LoadSpritesSync(ResourceLoader.DefaultBasePath, 10, out loaded, out error);

            Assert.IsNotNull(frames);
            Assert.AreEqual(10, frames.Length);
            Assert.Greater(loaded, 0);
        }

        [Test]
        public void LoadSpritesSync_InvalidPath_UsesPlaceholder()
        {
            int loaded;
            string error;
            Sprite[] frames = ResourceLoader.LoadSpritesSync("Missing/Path_", 3, out loaded, out error);

            Assert.IsNotNull(frames);
            Assert.AreEqual(3, frames.Length);
            Assert.IsNotNull(frames[0]);
        }
    }
}