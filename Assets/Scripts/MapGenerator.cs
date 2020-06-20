
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class MapGenerator : MonoBehaviour
{
    public Gradient colourMapGradient;

    [Header("Gameobjects")]
    public Renderer renderTexture;
    public List<Sprite> flowers;
    public int numFlowers;
    public float flowerScale;

    public static Color[] drawData;

    public List<GameObject> instantiatedFlowers;

    [Header("Map Settings")]
    public int mapWidth;
    public int mapHeight;
    public float mapScale;

    public FlowerMission[] flowerMissions;


    //Initialises important game functions
    public void Start()
    {
        //Generates noisemap with parameters
        var noiseMap = GenerateNoisemap(mapWidth, mapHeight, mapScale);
        //Places map into scene
        DrawMap(noiseMap);
        //Generates flowers over the top
        GenerateFlowers();
    }
    public float[,] GenerateNoisemap(int width, int height, float scale)
    {
        //Generates a two-dimesional noisemap with width and height parameters.
        float[,] noisemap = new float[width, height];
        //For every point on this map
        for(int y = 0; y < height; y++ )
        {
            for (int x = 0; x < height; x++)
            {
                //Generate a perlin noise sample of the position with scaling.
                noisemap[x, y] = Mathf.PerlinNoise(x / scale, y / scale);
            }
        }
        return noisemap;
    }
     
    public void DrawMap(float[,] heightData)
    {
        //Get width and height dimensional data
        int width = heightData.GetLength(0);
        int height = heightData.GetLength(1);
        //Initialise texture and colourmap
        Texture2D tex = new Texture2D(width, height);
        Color[] colourMap = new Color[width * height];
        //If we haven't already generated the data.
        if (drawData == null)
        {
            //For every pixel on the map
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    //Assigns a gradient-based evaluation to a one-dimensional UV texture
                    colourMap[y * width + x] = colourMapGradient.Evaluate(heightData[x, y]);
                }
            }
            //Assingns the appropriate drawing data.
            drawData = colourMap;
        } else
        {
            //If we've already generated the data, assign it to the current data.
            colourMap = drawData;
        }

        //Apply texture pixels and set up appropriate filtering modes.
        tex.SetPixels(colourMap);
        tex.Apply();
        tex.filterMode = FilterMode.Point;

        //Apply rendertexture texture to materials
        renderTexture.sharedMaterial.mainTexture = tex;
        //Set up localScale to requested parameters.
        renderTexture.transform.localScale = new Vector3(mapScale, 1, mapScale);
    }

    public void ClearCurrentFlowers()
    {
        //Kill all flowers.
        foreach(GameObject flower in instantiatedFlowers)
        {
            Destroy(flower);
        }
        
    }

    public void GenerateFlowers()
    {
        //Clear any current flowers generated.
        ClearCurrentFlowers();
        //For each of the flowers that we have requested.
        for(int i = 0; i < numFlowers; i++)
        {
            //Choose a random flower sprite
            Sprite randomFlower = flowers[Random.Range(0, flowers.Count)];
            //Instantiate new flower object
            GameObject flower = new GameObject();
            //Initialize sprite renderer.
            SpriteRenderer sr = flower.AddComponent<SpriteRenderer>();
            sr.sprite = randomFlower;

            //Add circle collider
            flower.AddComponent<CircleCollider2D>();
            //Set up transform positions, including randomly distributing it around the map.
            flower.transform.position = new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0);
            flower.transform.localScale = Vector3.one * flowerScale;
            flower.transform.SetParent(renderTexture.transform, true);
            flower.transform.name = "Flower" + flower.GetHashCode().ToString();
            //Add a class to the object for easy identification
            Flower flowerInst = flower.AddComponent<Flower>();
            //Add a mission to the flower.
            ApplyFlowerSettings(ref flowerInst, flowerMissions[Random.Range(0, flowerMissions.Length)]);
            //Set rendering layer
            flower.layer = 9;
            //Add the flower to the currently tracked flowers so we can delete it later.
            instantiatedFlowers.Add(flower);
        }
    }

    public void HiveBtn_Click()
    {
        SceneManager.LoadScene("MainScene");
    }

    //CLASS STUB (not implemented)
    [System.Serializable]
    public class ColourHeightmap
    {
        [System.Serializable]
        public class ColourThreshold {
            public Color color;
            public float threshold;
        }

        public ColourThreshold[] colourThresholds;

        public Color GetColourFromValue(float val)
        {
            val = Mathf.Clamp01(val);
            for (int i = 0; i < colourThresholds.Length; i++)
            {
                if (colourThresholds[i].threshold >= val)
                {
                    return colourThresholds[i].color;
                }
            }
            return Color.black;
        }


    }


    //Apply flower settings to a referential parameter.
    public void ApplyFlowerSettings(ref Flower flower, FlowerMission mission)
    {
        flower.beesRequired = Random.Range(mission.minBees, mission.maxBees);
        flower.successChance = Random.Range(mission.minSuccessRate, mission.maxSuccessRate);
        flower.reward = Random.Range(mission.minReward, mission.maxReward);
        flower.description = mission.description;
    }


}
