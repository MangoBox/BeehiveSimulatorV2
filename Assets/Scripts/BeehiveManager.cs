using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class BeehiveManager : MonoBehaviour
{
    //Static self-reference for external classes.
    public static BeehiveManager bm;

    [Header("Game Configuration")]
    public GameState gameState;

    private float _weekProgress;
    public float weekProgress
    {
        get { return _weekProgress; }
        set
        {
            _weekProgress = value;
            WeekProgressUpdateEvent?.Invoke(value);
        }
    }
    public event Beehive.IntUIUpdateCallback WeekProgressUpdateEvent;

    private int _weekNumber;
    public float weekNumber
    {
        get { return _weekNumber;  }
        set
        {
            _weekNumber = (int)value;
            WeekNumberUpdateEvent?.Invoke(value);
        }
    }

    public event Beehive.IntUIUpdateCallback WeekNumberUpdateEvent;

    [Header("Beehive Configuration")]
    public Beehive beehive;
    //Number of frames the game starts with.
    public int numberOfFrames;
    //Number of bees the game starts with.
    public int startingPopulation;
    //Default grid dimensions
    public Vector2Int frameGridSize;
    public Vector2Int centerOfGrid;
    public CellInfo[] cellInfos;
    public int selectedCell;
    public UIManager uiManager;

    [Header("Game Value Configuration")]
    public float baseHoneyPerSecond;
    public float basePopulationPerSecond;
    public float baseJellyPerSecond;
    public float honeyToJellyFactor;
    public int honeyGeneratorCost;
    public int startingHoney;
    public float queenHealthRate;
    public float weekProgressRate;
    public float queenBeeHealthRateIncrease;

    [Header("External GameObjects")]
    public Tilemap frameTilemap;
    public Tilemap overlayTilemap;

    [Header("Prefabs")]
    public Tile emptyHoneycomb;
    public Tile honeydrop;
    public Tile breeder;
    public Tile jelly;
    public Tile pollenTile;

    // Start is called before the first frame update
    void Start()
    {
        //Initialise singleton patterns, game states and default variable values.
        bm = this;
        gameState = GameState.HIVE_VIEW;
        weekProgress = 0;
        weekNumber = 1;

        //Construct new beehive.
        beehive = new Beehive(numberOfFrames, startingPopulation);
        beehive.setHoney(startingHoney);
        UpdateScreenFrame(beehive.beehiveFrames[0]);

    }

    // Update is called once per frame
    void Update()
    {
        //Update per-frame information if game isn't paused.
        if(gameState != GameState.PAUSED)
        {
            //Calculate the rates of the three resources.
            float honeyGeneratingRate = calculateHoneyGrowthRate() * Time.deltaTime;

            float jellyGeneratingRate = calculateJellyGrowthRate() * Time.deltaTime;

            float populationGrowthRate = calculatePopulationGrowthRate() * Time.deltaTime;

            //Net honey growth rate. Calculated from the growth rates of each.
            float netHoneyGrowth = honeyGeneratingRate - (populationGrowthRate + (jellyGeneratingRate * honeyToJellyFactor));

            //Add or remove honey to the beehive appropriately.
            beehive.addHoney(netHoneyGrowth);
            if (beehive.currentPollen > 0)
            {
                //Compensate for pollen usage.
                beehive.addPollen(-honeyGeneratingRate);
            }
            else
            {
                //Prevent pollen entering negatives.
                beehive.setPollen(0);
            }

            //If honey does not have any honey, reset the current honey amount to zero to avoid negatives.
            if (beehive.currentHoney <= 0)
            {
                beehive.setHoney(0);
            }
            else
            {
                //Add jelly and population if nessecary.
                beehive.addJelly(jellyGeneratingRate);
                beehive.addPopulation(populationGrowthRate);
            }

            //Remove health from the Queen Bee.
            beehive.queenBeeHealth -= queenHealthRate * Time.deltaTime;
            if (beehive.queenBeeHealth <= 0)
            {
                //If the queen bee no longer has health.
                //Attempt to use royal jelly.
                if (beehive.currentJelly >= 1)
                {
                    //Take away royal jelly and create a new queen bee.
                    beehive.currentJelly -= 1f;
                    beehive.queenBeeHealth = 1f;
                    //Make the game harder by making the queen bee die faster.
                    queenHealthRate *= queenBeeHealthRateIncrease;
                }
                else
                {
                    //GameOver
                    uiManager.OpenGameOver((int)weekNumber);
                    gameState = GameState.PAUSED;
                }
            }

            //Progress the week and start new week if nessecary.
            weekProgress += weekProgressRate * Time.deltaTime;
            if (weekProgress >= 1)
            {
                NewWeek();
            }
        }
    }

    public void NewWeek()
    {
        //Start new week by refreshing week.
        weekProgress = 0;
        weekNumber++;
        GetComponent<MapGenerator>().GenerateFlowers();
        uiManager.CloseFlowerMenu();
    }


    //Update the currently displayed frame on the screen.
    void UpdateScreenFrame(BeehiveFrame target)
    {
        //Remove old frame

        //Generate new frame
        frameTilemap.ResizeBounds();

        frameTilemap.BoxFill(new Vector3Int(centerOfGrid.x, centerOfGrid.y, 0), emptyHoneycomb,
            -((frameGridSize.x - 1) / 2),
            -((frameGridSize.y - 1) / 2),
            ((frameGridSize.x - 1) / 2),
            ((frameGridSize.y - 1) / 2)
            );
        frameTilemap.ResizeBounds();
        //overlayTilemap.BoxFill(Vector3Int.zero, honeydrop, -2, -2, 2, 2);

    }

    public void ConfirmFlowerMission(Flower flower)
    {
        //Confirm the current flower mission.
        bool success = Random.value <= flower.successChance;
        uiManager.OpenMissionResultDialog(success, (int)flower.reward);
        if(success)
        {
            //If the mission is a success, the bees return with their pollen.
            beehive.addPollen(flower.reward);
        } else
        {
            //If the mission is a failure, no bees return and thus they are lost.
            beehive.addPopulation(-flower.beesRequired);
        }
        
    }

    public float calculateHoneyGrowthRate()
    {
        //Calculates honey growth rate per second. Should be as a
        //function of bees pollen?
        return beehive.currentPollen > 0 ? baseHoneyPerSecond * (float)GetTileAmount(honeydrop) : 0;
    }

    public float calculateJellyGrowthRate()
    {
        //Calculates jelly growth rate per second.
        return baseJellyPerSecond * (float)GetTileAmount(jelly);
    }

    public float calculatePopulationGrowthRate()
    {
        //Calculates population growth rate based off current breeder amount in hive.
        return basePopulationPerSecond * (float)GetTileAmount(breeder);
    }

    //Click on cell
    public void ClickOnCell(Vector3Int pos)
    {
        //Get the clicked on cell.
        CellInfo ci = cellInfos[selectedCell];

        //If we are adding any cell on the overlay layer.
        if(ci.type != CellType.BLANK_CELL)
        {
            if (BeehiveManager.bm.frameTilemap.GetTile(pos) != null && beehive.currentHoney >= ci.buildCost && overlayTilemap.GetTile<Tile>(pos) == null)
            { 
                beehive.addHoney(-ci.buildCost);
                PlaceTile(ci.type, pos);
            }
        } else
        {
            //If we are adding a new hive cell (a honeycomb hexagon), add to the frame layer.
            if (beehive.currentHoney >= ci.buildCost && frameTilemap.GetTile<Tile>(pos) == null)
            {
                beehive.addHoney(-ci.buildCost);
                PlaceTile(ci.type, pos);
            }
        }
    }

    //Place a tile, respecting tile type given a parameter. Simple switching parameter.
    public void PlaceTile(CellType type, Vector3Int pos)
    {
        switch (type)
        {
            case CellType.HONEY_MAKER:
                overlayTilemap.SetTile(pos, honeydrop);
                return;

            case CellType.BREEDER:
                overlayTilemap.SetTile(pos, breeder);
                return;


            case CellType.ROYAL_JELLY_MAKER:
                overlayTilemap.SetTile(pos, pollenTile);
                return;

            case CellType.BLANK_CELL:
                frameTilemap.SetTile(pos, emptyHoneycomb);
                return;

            default:
                return;
        }
    }
    
    //Gets the number of tiles given a parameter.
    public int GetTileAmount(Tile tile)
    {
        //For each tile in the bounds of our tilemap.
        int amount = 0;
        BoundsInt tilemapBounds = overlayTilemap.cellBounds;
        foreach (Vector3Int pos in tilemapBounds.allPositionsWithin)
        {
            //If the tile is valid AND is not null.
            Tile comparison_tile = overlayTilemap.GetTile<Tile>(pos);
            if(comparison_tile != null && comparison_tile == tile)
            {
                amount++;
            }
        }
        //Return the amount we found!
        return amount;
    }

    public void EnterMapView()
    {
        gameState = GameState.MAP_VIEW;
    }

    public void EnterHiveView()
    {
        gameState = GameState.HIVE_VIEW;
    }

    [System.Serializable]
    public class CellInfo
    {
        public CellType type;
        public int buildCost;
        public Sprite cellSprite;

        public static CellInfo getCellByType(CellInfo[] infos, CellType type)
        {
            foreach(CellInfo ci in infos)
            {
                if(ci.type == type)
                {
                    return ci;
                }
            }
            return null;
        }
    }
   
}

public enum GameState
{
    HIVE_VIEW,
    MAP_VIEW,
    CELL_BUILD,
    PAUSED
}
