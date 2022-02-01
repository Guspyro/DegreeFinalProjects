using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CloudWorldStorage
{
    public static Stream cloudWorld;
    public static List<string> allWorlds;

    public static void SetWorld(Stream newWorld)
    {
        cloudWorld = newWorld;
    }

    public static void SetWorldList(List<string> newWorldList)
    {
        allWorlds = newWorldList;
    }

    public static void AddWorldToList(string worldName)
    {
        if (!allWorlds.Contains(worldName))
        {
            allWorlds.Add(worldName);
        }
    }

    public static void RemoveWorldFromList(string worldName)
    {
        allWorlds.Remove(worldName);
    }

    public static void ResetWorldList()
    {
        allWorlds = null;
    }

    public static World GetCloudWorldAsWorld()
    {
        using (BinaryReader reader = new BinaryReader(cloudWorld))
        {
            string name = reader.ReadString();
            int seed = reader.ReadInt32();
            int type = reader.ReadInt32();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            byte[] mapIDs = reader.ReadBytes(width * height);
            float spawnPointX = reader.ReadSingle();
            float spawnPointY = reader.ReadSingle();

            reader.Close();

            WorldController wc = WorldController.instance;
            Tile[,] map = new Tile[width, height];
            int counter = 0;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    map[col, row] = wc.allTiles[mapIDs[counter]];
                    counter++;
                }
            }

            Vector2 spawnPoint = new Vector2(spawnPointX, spawnPointY);

            switch (type)
            {
                case Constants.GenerationTypes.Templates: return new TemplatesWorld(name, seed, type, width, height, map, spawnPoint);
                case Constants.GenerationTypes.CelularAutomata: return new CelularAutomataWorld(name, seed, type, width, height, map, spawnPoint);
                default: return new RandomWorld(name, seed, type, width, height, map, spawnPoint);
            }
        }
    }

    public static void RemoveWorld()
    {
        cloudWorld = null;
    }
}
