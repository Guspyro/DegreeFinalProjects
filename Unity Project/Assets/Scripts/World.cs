using System.Collections.Generic;
using UnityEngine;

abstract public class World
{
    public string worldName = "";
    public int seed = 0;
    public WorldController wc;

    public int type = Constants.GenerationTypes.NoType;
    public int width = 1000;
    public int height = 1000;
    public Tile[,] map;
    public Vector2 spawnPoint;

    protected World(string worldName, int seed, int[] size)
    {
        this.worldName = worldName;
        this.seed = seed;
        width = size[0];
        height = size[1];
        map = new Tile[width, height];
        spawnPoint = new Vector2(height / 2, width / 2);
        wc = WorldController.instance;

        Random.InitState(seed);
    }

    public abstract void WorldGen();

    public void EndWorldCreation()
    {
        SetNewSpawnPoint();
        WorldIO.SaveWorld(this);
    }

    public void SetNewSpawnPoint()
    {
        int currentRow = (int)(height * (type == Constants.GenerationTypes.Final ? 0.85 : 0.99));
        int col = width / 2;
        while (currentRow > 0)
        {
            if (map[col, currentRow].id != Constants.BlockTypes.Air)
            {
                spawnPoint = new Vector2(col, currentRow + 1);
                return;
            }
            currentRow--;
        }

    }

    protected World(string newName, int newSeed, int newType, int newWidth, int newHeight, Tile[,] newMap, Vector2 newSpawnPoint)
    {
        worldName = newName;
        seed = newSeed;
        type = newType;
        width = newWidth;
        height = newHeight;
        map = newMap;
        spawnPoint = newSpawnPoint;
        wc = WorldController.instance;
    }


    #region Utils
    public bool CheckProbability(int probability)
    {
        return Random.Range(0, 100) < probability;
    }

    public void SaveMapsAsImage(List<Tile[,]> maps, string name)
    {
        Texture2D texture = new Texture2D(width * maps.Count + maps.Count, height);
        int startCol = 0;
        foreach (Tile[,] map in maps)
        {
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col <= width; col++)
                {
                    if (col == width) texture.SetPixel(col + startCol, row, Color.black);
                    else texture.SetPixel(col + startCol, row, map[col, row].color);
                }
            }
            startCol += width + 1;
        }

        texture.Apply();
        texture.filterMode = FilterMode.Point;
        Texture2D newTexture = texture.height < 500 ? Resize(texture, 5, 5) : texture;
        byte[] bytes = newTexture.EncodeToPNG();
        System.IO.File.WriteAllBytes("B://" + name + ".png", bytes);
    }

    Texture2D Resize(Texture2D texture, int widthMultiplier, int heightMultiplier)
    {
        Texture2D result = new Texture2D(texture.width * widthMultiplier, texture.height * heightMultiplier);
        for (int row = 0; row < texture.height; row++)
        {
            for (int col = 0; col <= texture.width; col++)
            {
                Color color = texture.GetPixel(col, row);
                for (int pixelRow = 0; pixelRow < widthMultiplier; pixelRow++)
                {
                    for (int pixelCol = 0; pixelCol <= heightMultiplier; pixelCol++)
                    {
                        result.SetPixel(col * widthMultiplier + pixelCol, row * heightMultiplier + pixelRow, color);
                    }
                }
            }
        }
        result.Apply();
        return result;
    }
    #endregion

}
