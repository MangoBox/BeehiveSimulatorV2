using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BeehiveManager : MonoBehaviour
{
    //Static self-reference for external classes.
    public static BeehiveManager bm;

    [Header("Beehive Configuration")]
    public Beehive beehive;
    //Number of frames the game starts with.
    public int numberOfFrames;
    //Number of bees the game starts with.
    public int startingPopulation;
    //Default grid dimensions
    public Vector2Int frameGridSize;
    public Vector2Int centerOfGrid;

    [Header("External GameObjects")]
    public Tilemap frameTilemap;
    public Tilemap overlayTilemap;

    [Header("Prefabs")]
    public Tile emptyHoneycomb;
    public Tile honeydrop;

    // Start is called before the first frame update
    void Start()
    {
        bm = this;


        //Construct new beehive.
        beehive = new Beehive(numberOfFrames, startingPopulation);

        UpdateScreenFrame(beehive.beehiveFrames[0]);

    }

    // Update is called once per frame
    void Update()
    {
        beehive.addHoney((int)Math.Ceiling(calculateHoneyGrowthRate()*Time.deltaTime));
        beehive.setPopulation(15000);
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
        overlayTilemap.BoxFill(Vector3Int.zero, honeydrop, -2, -2, 2, 2);

    }

    float calculateHoneyGrowthRate()
    {
        //Calculates honey growth rate per second. Should be as a
        //function of bees pollen?
        return 1f;
    }

    public void ClickOnCell(Vector3Int pos)
    {
        TileBase t = overlayTilemap.GetTile(pos);
        print(pos);
        overlayTilemap.SetTile(pos, honeydrop);
    }
}
