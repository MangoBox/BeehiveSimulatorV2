using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BeehiveManager : MonoBehaviour
{
    //Static self-reference for external classes.
    public static BeehiveManager bm;

    [Header("Beehive Configuration")]
    [HideInInspector]
    public Beehive beehive;
    //Number of frames the game starts with.
    public int numberOfFrames;
    //Number of bees the game starts with.
    public int startingPopulation;
    //Default grid dimensions
    public Vector2Int frameGridSize;

    [Header("External GameObjects")]
    public Tilemap frameTilemap;

    [Header("Prefabs")]
    public Tile emptyHoneycomb;

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
    }

    //Update the currently displayed frame on the screen.
    void UpdateScreenFrame(BeehiveFrame target)
    {
        //Remove old frame
        
        //Generate new frame
        frameTilemap.BoxFill(Vector3Int.zero, emptyHoneycomb, 0, frameGridSize.x, 0, frameGridSize.y);

    }
}
