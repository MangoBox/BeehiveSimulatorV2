using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.AI;
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
    public GameObject missionResultMenu;
    public GameObject successMenu;
    public GameObject failedMenu;
    public Text pollenRewardText;
    public Image queenBeeHealthMeter;
    public Image weekMeter;
    public Text weekNumber;
    public GameObject pauseMenu;


    [Header("Flower Menu")]
    public GameObject flowerMenu;
    public Text rewardText;
    public Text riskText;
    public Text successText;
    public Text beesText;
    public Text descriptionText;
    public Image flowerImage;

    public Image placingCellSprite;

    [Header("Build Cell Menu")]
    public Text honeyGeneratorCost;
    public Text honeyGeneratorRate;
    public Text breederCost;
    public Text breederRate;
    public Text royalJellyCost;
    public Text royalJellyRate;
    public Text newCellCost;

    public GameObject[] hiveViewGameObjects;
    public GameObject[] mapViewGameObjects;

    public Color[] riskColors;

    private Flower mostRecentFlower;

    // Start is called before the first frame update
    void Start()
    {
        print("Attaching");

        BeehiveManager.bm.beehive.HoneyUpdateEvent += UpdateHoneyCount;
        BeehiveManager.bm.beehive.PopulationUpdateEvent += UpdatePopulationCount;
        BeehiveManager.bm.beehive.PollenUpdateEvent += UpdatePollenCount;
        BeehiveManager.bm.beehive.JellyUpdateEvent += UpdateJellyCount;
        BeehiveManager.bm.beehive.QueenHealthUpdateEvent += UpdateQueenBeeHealth;
        BeehiveManager.bm.WeekProgressUpdateEvent += UpdateWeekProgress;
        BeehiveManager.bm.WeekNumberUpdateEvent += UpdateWeekNumber;

        //Update all initial values
        honeyGeneratorCost.text = "Cost: " + BeehiveManager.bm.honeyGeneratorCost + "H";
        honeyGeneratorRate.text = "Rate: " + BeehiveManager.bm.baseHoneyPerSecond + "H/s";
        breederCost.text = "Cost: " + BeehiveManager.bm.cellInfos[1].buildCost + "H";
        breederRate.text = "Rate: " + BeehiveManager.bm.basePopulationPerSecond + " Bees/s";
        royalJellyCost.text = "Cost: " + BeehiveManager.bm.cellInfos[2].buildCost + "H";
        royalJellyRate.text = (BeehiveManager.bm.baseJellyPerSecond * 60).ToString() + " Jelly/m";
        newCellCost.text = "Cost: " + BeehiveManager.bm.cellInfos[3].buildCost + "H";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu() {
        pauseMenu.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateHoneyCount(float honey)
    {
        honeyText.text = "Honey: " + ((int)honey).ToString();
    }

    public void UpdateWeekNumber(float week)
    {
        weekNumber.text = "Week " + (int)week;
    }
    
    public void UpdateWeekProgress(float progress)
    {
        weekMeter.fillAmount = progress;
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

    public void UpdateQueenBeeHealth(float health)
    {
        queenBeeHealthMeter.fillAmount = health;
    }

    public void OpenBuildMenu()
    {
        buildMenu.SetActive(true);
        BeehiveManager.bm.gameState = GameState.CELL_BUILD;
    }

    public void CloseBuildMenu()
    {
        buildMenu.SetActive(false);
        BeehiveManager.bm.gameState = GameState.HIVE_VIEW;
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
    
    public void ClickBuildNewCell()
    {
        BeehiveManager.bm.selectedCell = (int)CellType.BLANK_CELL;
        UpdateBuildSprite();
    }

    public void ClickMapView()
    {
        BeehiveManager.bm.EnterMapView();
        GetComponent<ControlManager>().ResetCamera();
        hiveViewGameObjects.Where(x => x != null).ToList().ForEach(x => x.SetActive(false));
        mapViewGameObjects.Where(x => x != null).ToList().ForEach(x => x.SetActive(true));
    }

    public void ClickHiveView()
    {
        BeehiveManager.bm.EnterHiveView();
        GetComponent<ControlManager>().ResetCamera();
        hiveViewGameObjects.Where(x => x != null).ToList().ForEach(x => x.SetActive(true));
        mapViewGameObjects.Where(x => x != null).ToList().ForEach(x => x.SetActive(false));
    }

    public void UpdateBuildSprite()
    {
        placingCellSprite.sprite = BeehiveManager.bm.cellInfos[BeehiveManager.bm.selectedCell].cellSprite;
        Debug.Log(BeehiveManager.bm.cellInfos[BeehiveManager.bm.selectedCell].buildCost);
    }


    public void ClickFlower(Flower flower)
    {
        OpenFlowerMenu(flower);
    }

    public void CloseFlowerMenu()
    {
        flowerMenu.SetActive(false);
    }

    public void ConfirmFlowerCollect()
    {
        flowerMenu.SetActive(false);
        BeehiveManager.bm.ConfirmFlowerMission(mostRecentFlower);
        mostRecentFlower.gameObject.SetActive(false);
    }

    public void OpenMissionResultDialog(bool success, int pollen = 0)
    {
        missionResultMenu.SetActive(true);
        successMenu.SetActive(success);
        failedMenu.SetActive(!success);
        pollenRewardText.text = "Reward: " + pollen.ToString() + " Pollen";
    }

    public void AcceptMissionResult()
    {
        missionResultMenu.SetActive(false);
    }

    public void OpenFlowerMenu(Flower flower)
    {
        flowerMenu.SetActive(true);
        rewardText.text = "Reward: " + ((int)flower.reward).ToString() + " Pollen";
        successText.text = ((int)(flower.successChance * 100f)).ToString() + "% Chance of Success";
        Color riskColor = Color.white;
        riskText.text = "Risk: " + calculateRiskLevel(flower.successChance, ref riskColor);
        riskText.color = riskColor;
        beesText.text = ((int)flower.beesRequired).ToString() + " Bees Required";
        descriptionText.text = flower.description;
        mostRecentFlower = flower;
    }

    public string calculateRiskLevel(float success, ref Color riskColor)
    {
        if (success < 0.3f)
        {
            riskColor = riskColors[4];
            return "EXTREME";
        }
        else if(success < 0.5f)
        {
            riskColor = riskColors[3];
            return "VERY HIGH";
        } else if (success < 0.7f)
        {
            riskColor = riskColors[2];
            return "HIGH";
        }
        else if (success < 1f)
        {
            riskColor = riskColors[1];
            return "MEDIUM";
        }
        else if (success == 1f)
        {
            riskColor = riskColors[0];
            return "NONE";
        }
        return "UNKNOWN";
    }
}
