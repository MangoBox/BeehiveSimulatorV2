using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeehiveFrame : MonoBehaviour
{
    public Cell[,] cells;

    public BeehiveFrame()
    {
        cells = new Cell[BeehiveManager.bm.frameGridSize.y,BeehiveManager.bm.frameGridSize.x];

        //Iterate over all grid indexes and perform any required setup.
        for (int y = 0; y < BeehiveManager.bm.frameGridSize.y; y++)
        {
            for (int x = 0; x < BeehiveManager.bm.frameGridSize.x; x++)
            {
                //stub
            }
        }
    }
}
