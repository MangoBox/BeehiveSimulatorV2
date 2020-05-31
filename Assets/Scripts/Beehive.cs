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

    //Current amount of pollen
    public int currentPollen;

    //Current amount of jelly
    public int currentJelly;

    //EVENTS
    public event IntUIUpdateCallback HoneyUpdateEvent;
    public event IntUIUpdateCallback PopulationUpdateEvent;
    public event IntUIUpdateCallback PollenUpdateEvent;
    public event IntUIUpdateCallback JellyUpdateEvent;
    public delegate void IntUIUpdateCallback(int value);

    //Returns all cells in the beehive for statistics.
    public List<Cell> getAllCells()
    {
        List<Cell> cells = new List<Cell>();
        foreach (BeehiveFrame frame in beehiveFrames)
        {
            for (int i = 0; i < frame.cells.Length; i++)
            {
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


    public void setPollen(int pollen)
    {
        currentPollen = pollen;
        PollenUpdateEvent?.Invoke(pollen);
    }

    public void addPollen(int amount)
    {
        setPollen(currentPollen + amount);
    }


    public void setPopulation(int pop)
    {
        population = pop;
        PopulationUpdateEvent?.Invoke(pop);
    }

    public void addPopulation(int amount)
    {
        setPopulation(population + amount);
    }
}
