using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class BeehiveManager : MonoBehaviour
{
    //Static self-reference for external classes.
    public static BeehiveManager bm;

    [Header("Game Configuration")]
    public GameState gameState;


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

    [Header("Game Value Configuration")]
    public float baseHoneyPerSecond;
    public float basePopulationPerSecond;
    public int honeyGeneratorCost;
    public int startingHoney;

    [Header("External GameObjects")]
    public Tilemap frameTilemap;
    public Tilemap overlayTilemap;

    [Header("Prefabs")]
    public Tile emptyHoneycomb;
    public Tile honeydrop;
    public Tile breeder;
    public Tile pollenTile;

    // Start is called before the first frame update
    void Start()
    {
        bm = this;
        gameState = GameState.HIVE_VIEW;

        //Construct new beehive.
        beehive = new Beehive(numberOfFrames, startingPopulation);
        beehive.setHoney(startingHoney);
        UpdateScreenFrame(beehive.beehiveFrames[0]);

    }

    // Update is called once per frame
    void Update()
    {
        beehive.addHoney(calculateHoneyGrowthRate()*Time.deltaTime);
        beehive.addPopulation(calculatePopulationGrowthRate() * Time.deltaTime);
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

    float calculateHoneyGrowthRate()
    {
        //Calculates honey growth rate per second. Should be as a
        //function of bees pollen?
        return baseHoneyPerSecond * (float)GetTileAmount(honeydrop);
    }

    float calculatePopulationGrowthRate()
    {
        //Calculates honey growth rate per second. Should be as a
        //function of bees pollen?
        return basePopulationPerSecond * (float)GetTileAmount(breeder);
    }

    public void ClickOnCell(Vector3Int pos)
    {
        TileBase t = overlayTilemap.GetTile(pos);
        CellInfo ci = cellInfos[selectedCell];

        if (beehive.currentHoney >= ci.buildCost && overlayTilemap.GetTile<Tile>(pos) == null)
        {
            beehive.addHoney(-ci.buildCost);
            PlaceTile(ci.type, pos);
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
