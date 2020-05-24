using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Beehive
{
    //All beehive frames
    public BeehiveFrame[] beehiveFrames;

    //Number of frames
    public int frameNumber;

    //Current number of bees
    public int population;

    //Total current amount of honey
    public int currentHoney;

    //Returns all cells in the beehive for statistics.
    public List<Cell> getAllCells()
    {
        List<Cell> cells = new List<Cell>();
        foreach (BeehiveFrame frame in beehiveFrames)
        {
            for(int i = 0; i < frame.cells.Length; i++) {
                //Currently broken
                    
            }
            
        }
        return cells;
    }
    
    //Constructor class
    public Beehive(int _frameNumber, int startingPopulation)
    {
        this.frameNumber = _frameNumber;
        this.population = startingPopulation;

        beehiveFrames = new BeehiveFrame[frameNumber];
    }

}
