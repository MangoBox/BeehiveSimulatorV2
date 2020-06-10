using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Gradient colourMapGradient;

    [Header("Gameobjects")]
    public Renderer renderTexture;
    public List<GameObject> flowers;

    [Header("Map Settings")]
    public int mapWidth;
    public int mapHeight;
    public float mapScale;

    public void Start()
    {
        var noiseMap = GenerateNoisemap(mapWidth, mapHeight, mapScale);
        DrawMap(noiseMap);

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

        renderTexture.sharedMaterial.mainTexture = tex;
        renderTexture.transform.localScale = new Vector3(mapScale, 1, mapScale);
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
