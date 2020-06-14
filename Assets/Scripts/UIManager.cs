using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    [Header("UI Objects")]
    public Text honeyText;
    public Text populationText;
    public Text royalJellyText;
    public Text pollenText;
    public GameObject buildMenu;

    public Image placingCellSprite;

    public GameObject[] hiveViewGameObjects;
    public GameObject[] mapViewGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        print("Attaching");

        BeehiveManager.bm.beehive.HoneyUpdateEvent += UpdateHoneyCount;
        BeehiveManager.bm.beehive.PopulationUpdateEvent += UpdatePopulationCount;
        BeehiveManager.bm.beehive.PollenUpdateEvent += UpdatePollenCount;
        BeehiveManager.bm.beehive.JellyUpdateEvent += UpdateJellyCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHoneyCount(float honey)
    {
        honeyText.text = "Honey: " + ((int)honey).ToString();
    }

    public void UpdatePopulationCount(float population)
    {
        populationText.text = "Bees: " + ((int)population).ToString();
    }

    public void UpdatePollenCount(float pollen)
    {
        pollenText.text = "Pollen: " + ((int)pollen).ToString();
    }

    public void UpdateJellyCount(float jelly)
    {
        royalJellyText.text = "Royal Jelly: " + ((int)jelly).ToString();
    }

    public void OpenBuildMenu()
    {
        buildMenu.SetActive(true);
    }

    public void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
    }

    public void ClickHoneyGenerator()
    {
        BeehiveManager.bm.selectedCell = (int)CellType.HONEY_MAKER;
        UpdateBuildSprite();
    }

    public void ClickBreeder()
    {
        BeehiveManager.bm.selectedCell = (int)CellType.BREEDER;
        UpdateBuildSprite();
    }

    public void ClickRoyalJellyGenerator()
    {
        BeehiveManager.bm.selectedCell = (int)CellType.ROYAL_JELLY_MAKER;
        UpdateBuildSprite();
    }

    public void ClickMapView()
    {
        BeehiveManager.bm.EnterMapView();
        hiveViewGameObjects.Where(x => x != null).ToList().ForEach(x => x.SetActive(false));
        mapViewGameObjects.Where(x => x != null).ToList().ForEach(x => x.SetActive(true));
    }

    public void ClickHiveView()
    {
        BeehiveManager.bm.EnterHiveView();
        hiveViewGameObjects.Where(x => x != null).ToList().ForEach(x => x.SetActive(true));
        mapViewGameObjects.Where(x => x != null).ToList().ForEach(x => x.SetActive(false));
    }

    public void UpdateBuildSprite()
    {
        placingCellSprite.sprite = BeehiveManager.bm.cellInfos[BeehiveManager.bm.selectedCell].cellSprite;
        Debug.Log(BeehiveManager.bm.cellInfos[BeehiveManager.bm.selectedCell].buildCost);
    }
}
