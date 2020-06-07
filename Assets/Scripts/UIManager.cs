using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    [Header("UI Objects")]
    public Text honeyText;
    public Text populationText;
    public Text royalJellyText;
    public Text pollenText;
    public GameObject buildMenu;

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

    public void UpdateHoneyCount(int honey)
    {
        honeyText.text = "Honey: " + honey.ToString();
    }

    public void UpdatePopulationCount(int population)
    {
        populationText.text = "Bees: " + population.ToString();
    }

    public void UpdatePollenCount(int pollen)
    {
        pollenText.text = "Pollen: " + pollen.ToString();
    }

    public void UpdateJellyCount(int jelly)
    {
        royalJellyText.text = "Royal Jelly: " + jelly.ToString();
    }

    public void OpenBuildMenu()
    {
        buildMenu.SetActive(true);
    }

    public void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
    }
}
