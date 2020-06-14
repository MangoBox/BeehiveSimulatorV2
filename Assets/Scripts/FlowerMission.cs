using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Beehive Simulator/FlowerMission", order = 1)]
public class FlowerMission : ScriptableObject
{
    [Header("Success Rate")]
    [Range(0, 1)]
    public float minSuccessRate;
    [Range(0, 1)]
    public float maxSuccessRate;

    [Header("Reward")]
    public float minReward;
    public float maxReward;

    [Header("Bees Required")]
    public float minBees;
    public float maxBees;

    [TextArea(3, 5)]
    public string description;
}