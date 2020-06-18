using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal.VersionControl;
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
        float honeyGeneratingRate = calculateHoneyGrowthRate() * Time.deltaTime;

        float jellyGeneratingRate = calculateJellyGrowthRate() * Time.deltaTime;

        float populationGrowthRate = calculatePopulationGrowthRate() * Time.deltaTime;

        //Net honey growth rate.
        float netHoneyGrowth = honeyGeneratingRate - (populationGrowthRate + (jellyGeneratingRate * honeyToJellyFactor));
        beehive.addHoney(netHoneyGrowth);
        if (beehive.currentPollen > 0)
        {
            beehive.addPollen(-honeyGeneratingRate);
        } else
        {
            beehive.setPollen(0);
        }

        if(beehive.currentHoney <= 0)
        {
            beehive.setHoney(0);
        } else
        {
            beehive.addJelly(jellyGeneratingRate);
            beehive.addPopulation(populationGrowthRate);
        }

        beehive.queenBeeHealth -= queenHealthRate * Time.deltaTime;

        weekProgress += weekProgressRate * Time.deltaTime;
        if(weekProgress >= 1)
        {
            NewWeek();
        }
  
    }

    public void NewWeek()
    {
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
        bool success = Random.value <= flower.successChance;
        uiManager.OpenMissionResultDialog(success, (int)flower.reward);
        if(success)
        {
            beehive.addPollen(flower.reward);
        } else
        {
            beehive.addPopulation(-flower.beesRequired);
        }
        
    }

    float calculateHoneyGrowthRate()
    {
        //Calculates honey growth rate per second. Should be as a
        //function of bees pollen?
        return beehive.currentPollen > 0 ? baseHoneyPerSecond * (float)GetTileAmount(honeydrop) : 0;
    }

    float calculateJellyGrowthRate()
    {
        //Calculates honey growth rate per second. Should be as a
        //function of bees pollen?
        return baseJellyPerSecond * (float)GetTileAmount(jelly);
    }

    float calculatePopulationGrowthRate()
    {
        //Calculates honey growth rate per second. Should be as a
        //function of bees pollen?
        return basePopulationPerSecond * (float)GetTileAmount(breeder);
    }

    public void ClickOnCell(Vector3Int pos)
    {
        CellInfo ci = cellInfos[selectedCell];

        if(ci.type != CellType.BLANK_CELL)
        {
            if (BeehiveManager.bm.frameTilemap.GetTile(pos) != null && beehive.currentHoney >= ci.buildCost && overlayTilemap.GetTile<Tile>(pos) == null)
            { 
                beehive.addHoney(-ci.buildCost);
                PlaceTile(ci.type, pos);
            }
        } else
        {
            print("place new tile");
            if (beehive.currentHoney >= ci.buildCost && frameTilemap.GetTile<Tile>(pos) == null)
            {
                beehive.addHoney(-ci.buildCost);
                PlaceTile(ci.type, pos);
            }
        }
    }

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

    public int GetTileAmount(Tile tile)
    {
        int amount = 0;
        BoundsInt tilemapBounds = overlayTilemap.cellBounds;
        foreach (Vector3Int pos in tilemapBounds.allPositionsWithin)
        {
            Tile comparison_tile = overlayTilemap.GetTile<Tile>(pos);
            if(comparison_tile != null && comparison_tile == tile)
            {
                amount++;
            }
        }
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
    CELL_BUILD
}
