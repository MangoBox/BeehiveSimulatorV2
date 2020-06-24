using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.XR.WSA;
using System.Linq;
using System.Runtime.InteropServices;

namespace Tests
{
    public class TestGeneration
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestGenerationSimplePasses()
        {
            // Use the Assert class to test conditions

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestGenerationWithEnumeratorPasses()
        {
            SceneManager.LoadScene("MainScene");
            yield return null;
            BeehiveManager bm = MonoBehaviour.FindObjectOfType<BeehiveManager>();
            Assert.IsNotNull(bm);

            Assert.AreEqual(bm.baseHoneyPerSecond, 3f);
            
        }
    }
}
