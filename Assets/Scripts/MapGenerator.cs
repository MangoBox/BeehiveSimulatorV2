
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class MapGenerator : MonoBehaviour
{

    public Gradient colourMapGradient;

    [Header("Gameobjects")]
    public Renderer renderTexture;
    public List<Sprite> flowers;
    public int numFlowers;
    public float flowerScale;

    [Header("Map Settings")]
    public int mapWidth;
    public int mapHeight;
    public float mapScale;

    public void Start()
    {
        var noiseMap = GenerateNoisemap(mapWidth, mapHeight, mapScale);
        DrawMap(noiseMap);
        GenerateFlowers();

    }
    public float[,] GenerateNoisemap(int width, int height, float scale)
    {
        float[,] noisemap = new float[width, height];
        for(int y = 0; y < height; y++ )
        {
            for (int x = 0; x < height; x++)
            {
                noisemap[x, y] = Mathf.PerlinNoise(x / scale, y / scale);
            }
        }
        return noisemap;
    }
     
    public void DrawMap(float[,] heightData)
    {
        int width = heightData.GetLength(0);
        int height = heightData.GetLength(1);

        Texture2D tex = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                colourMap[y * width + x] = colourMapGradient.Evaluate(heightData[x, y]);
            }
        }

        tex.SetPixels(colourMap);
        tex.Apply();
        tex.filterMode = FilterMode.Point;

        renderTexture.sharedMaterial.mainTexture = tex;
        renderTexture.transform.localScale = new Vector3(mapScale, 1, mapScale);
    }

    public void GenerateFlowers()
    {
        for(int i = 0; i < numFlowers; i++)
        {
            Sprite randomFlower = flowers[Random.Range(0, flowers.Count)];
            GameObject flower = new GameObject();
            SpriteRenderer sr = flower.AddComponent<SpriteRenderer>();
            sr.sprite = randomFlower;
            flower.transform.position = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), 0);
            flower.transform.localScale = Vector3.one * flowerScale;
        }
    }

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
}
