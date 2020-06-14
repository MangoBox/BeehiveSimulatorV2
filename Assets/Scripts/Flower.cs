using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public float successChance;
    public float reward;
    public float beesRequired;
    public string description;
    public Texture2D flowerImage
    {
        get
        {
            return GetComponent<Sprite>().texture;
        }
    }

    void OnMouseDown()
    {
        BeehiveManager.bm.GetComponent<UIManager>().ClickFlower(this);
    }
}
