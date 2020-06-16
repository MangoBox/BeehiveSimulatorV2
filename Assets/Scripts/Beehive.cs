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
    public float population;

    //Total current amount of honey
    public float currentHoney;

    //Current amount of pollen
    public float currentPollen;

    //Current amount of jelly
    public float currentJelly;

    public float _queenBeeHealth;
    public float queenBeeHealth
    {
        get
        {
            return _queenBeeHealth;
        } set
        {
            _queenBeeHealth = value;
            QueenHealthUpdateEvent?.Invoke(value);
        }
    }

    //EVENTS
    public event IntUIUpdateCallback HoneyUpdateEvent;
    public event IntUIUpdateCallback PopulationUpdateEvent;
    public event IntUIUpdateCallback PollenUpdateEvent;
    public event IntUIUpdateCallback JellyUpdateEvent;
    public event IntUIUpdateCallback QueenHealthUpdateEvent;
    public delegate void IntUIUpdateCallback(float value);

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
        queenBeeHealth = 1f;

        //Initialize Events
    }

    public void setHoney(float honey)
    {
        currentHoney = honey;
        HoneyUpdateEvent?.Invoke(honey);
    }

    public void addHoney(float amount)
    {
        setHoney(currentHoney + amount);
    }


    public void setPollen(float pollen)
    {
        currentPollen = pollen;
        PollenUpdateEvent?.Invoke(pollen);
    }

    public void addPollen(float amount)
    {
        setPollen(currentPollen + amount);
    }


    public void setPopulation(float pop)
    {
        population = pop;
        PopulationUpdateEvent?.Invoke(pop);
    }

    public void addPopulation(float amount)
    {
        setPopulation(population + amount);
    }

    public void setJelly(float jelly)
    {
        currentJelly = jelly;
        JellyUpdateEvent?.Invoke(jelly);
    }

    public void addJelly(float amount)
    {
        setJelly(currentJelly + amount);
    }
}
