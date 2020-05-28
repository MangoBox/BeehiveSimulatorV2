using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

[System.Serializable]
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

    //EVENTS
    public event IntUIUpdateCallback HoneyUpdateEvent;
    public event IntUIUpdateCallback PopulationUpdateEvent;
    public delegate void IntUIUpdateCallback(int value);

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

        //Initialize Events
    }

    public void setHoney(int honey)
    {
        currentHoney = honey;
        HoneyUpdateEvent?.Invoke(honey);
    }

    public void addHoney(int amount)
    {
        setHoney(currentHoney + amount);
    }

    public void setPopulation(int pop)
    {
        population = pop;
        PopulationUpdateEvent?.Invoke(pop);
    }

}
