using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.XR.WSA;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor.VersionControl;

namespace Tests
{
    public class TestGeneration
    {

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestFlowerGeneration()
        {
            SceneManager.LoadScene("MainScene");
            yield return null;
            BeehiveManager bm = Object.FindObjectOfType<BeehiveManager>();

            Assert.IsNotNull(bm);
            MapGenerator mg = bm.GetComponent<MapGenerator>();

            Assert.IsNotNull(mg);
            bm.EnterMapView();
            int flowers_real = mg.renderTexture.GetComponentsInChildren<Flower>().Count();
            Assert.AreEqual(mg.numFlowers, flowers_real);
            
        }

        [UnityTest]
        public IEnumerator TestFlowerRandomisationGeneration()
        {
            SceneManager.LoadScene("MainScene");
            yield return null;
            BeehiveManager bm = Object.FindObjectOfType<BeehiveManager>();

            Assert.IsNotNull(bm);
            MapGenerator mg = bm.GetComponent<MapGenerator>();

            Assert.IsNotNull(mg);
            bm.EnterMapView();
            
            foreach(Flower f in mg.renderTexture.GetComponentsInChildren<Flower>())
            {
                Assert.IsNotNull(f.description);
                Assert.IsNotNull(f.successChance);
                Assert.IsNotNull(f.reward);
                Assert.IsNotNull(f.beesRequired);
                Assert.IsNotNull(f.GetComponent<SpriteRenderer>());
            }

        }
    }
}
