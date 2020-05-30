﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{

    [Header("UI Objects")]
    public Text honeyText;
    public Text populationText;

    // Start is called before the first frame update
    void Start()
    {
        print("Attaching");

        BeehiveManager.bm.beehive.HoneyUpdateEvent += UpdateHoneyCount;
        BeehiveManager.bm.beehive.PopulationUpdateEvent += UpdatePopulationCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHoneyCount(int honey)
    {
        string honeyStr = ((float)honey/1000f).ToString();
        string resultText = string.Format("Honey: {0:G0}", honeyStr);

        honeyText.text = resultText;
    }

    public void UpdatePopulationCount(int population)
    {
        populationText.text = "Bees: " + population.ToString();
    }
}
