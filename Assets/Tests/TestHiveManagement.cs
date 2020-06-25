using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class TestHiveManagement
    {
        [UnityTest]
        public IEnumerator TestRateGeneration()
        {
            SceneManager.LoadScene("MainScene");
            yield return null;
            BeehiveManager bm = MonoBehaviour.FindObjectOfType<BeehiveManager>();
            Assert.IsNotNull(bm);

            bm.selectedCell = 0;
            bm.ClickOnCell(Vector3Int.zero);
            bm.ClickOnCell(Vector3Int.right);
            bm.ClickOnCell(Vector3Int.left);

            Assert.AreEqual(3, bm.GetTileAmount(bm.honeydrop));
        }

        [UnityTest]
        public IEnumerator TestRoyalJellyGeneration()
        {
            SceneManager.LoadScene("MainScene");
            yield return null;
            BeehiveManager bm = MonoBehaviour.FindObjectOfType<BeehiveManager>();
            Assert.IsNotNull(bm);

            bm.selectedCell = 0;
            bm.overlayTilemap.FloodFill(Vector3Int.zero, bm.jelly);
            bm.ClickOnCell(Vector3Int.zero);
            Debug.Log("Jelly Tiles" + bm.GetTileAmount(bm.honeydrop));
            bm.beehive.setHoney(100000);
            bm.beehive.setPollen(300000);
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Current Jelly after 0.5 seconds: " + bm.beehive.currentJelly.ToString());
            Assert.Greater(bm.beehive.currentJelly,0,"Not enough royal jelly.");
        }

        [UnityTest]
        public IEnumerator TestRoyalJellyUsage()
        {
            SceneManager.LoadScene("MainScene");
            yield return null;
            BeehiveManager bm = MonoBehaviour.FindObjectOfType<BeehiveManager>();
            Assert.IsNotNull(bm);

            int originalJelly = 100;
            Debug.Log("Current Jelly after 0 seconds: " + originalJelly.ToString());
            bm.selectedCell = 0;
            bm.beehive.setJelly(originalJelly);
            bm.queenHealthRate = 100f;
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Current Jelly after 0.5 seconds: " + bm.beehive.currentJelly.ToString());
            Assert.AreNotEqual(bm.beehive.currentJelly, originalJelly, "Jelly has not been used here.");
        }
    }
}
