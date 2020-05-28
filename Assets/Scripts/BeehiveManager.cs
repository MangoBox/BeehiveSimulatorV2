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
        beehive.addHoney((int)(Time.deltaTime * calculateHoneyGrowthRate()));
        beehive.setPopulation(15000);
    }

    //Update the currently displayed frame on the screen.
    void UpdateScreenFrame(BeehiveFrame target)
    {
        //Remove old frame
        
        //Generate new frame
        frameTilemap.BoxFill(Vector3Int.zero, emptyHoneycomb, -5,-5, 5,5);
        overlayTilemap.BoxFill(Vector3Int.zero, honeydrop, -2, -2, 2, 2);

    }

    float calculateHoneyGrowthRate()
    {
        //Calculates honey growth rate per second. Should be as a
        //function of bees pollen?
        return 100f;
    }
}
