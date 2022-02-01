using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class WorldIO
{
    public static void SaveWorld(World world)
    {
        Directory.CreateDirectory(Constants.SaveWorldPath);
        using (BinaryWriter writter = new BinaryWriter(File.Open(GetWorldPathFromName(world.worldName), FileMode.Create)))
        {
            writter.Write(world.worldName);
            writter.Write(world.seed);
            writter.Write(world.type);
            writter.Write(world.width);
            writter.Write(world.height);
            byte[] mapIDs = new byte[world.width * world.height];
            int counter = 0;
            for (int row = 0; row < world.height; row++)
            {
                for (int col = 0; col < world.width; col++)
                {
                    mapIDs[counter] = (byte)world.map[col, row].id;
                    counter++;
                }
            }
            writter.Write(mapIDs);
            writter.Write(world.spawnPoint.x);
            writter.Write(world.spawnPoint.y);
        }
    }

    public static void SaveWorld(string worldName, Stream world)
    {
        using (FileStream output = new FileStream(GetWorldPathFromName(worldName), FileMode.Create))
        {
            world.CopyTo(output);
        }
    }

    public static World LoadWorld(string file)
    {
        if (!CheckIfWorldExists(file)) throw new System.Exception();

        using (BinaryReader reader = new BinaryReader(File.Open(GetWorldPathFromName(file), FileMode.Open)))
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
                case Constants.GenerationTypes.Final: return new FinalWorld(name, seed, type, width, height, map, spawnPoint);
                case Constants.GenerationTypes.PerlinNoise: return new PerlinNoiseWorld(name, seed, type, width, height, map, spawnPoint);
                case Constants.GenerationTypes.CelularAutomata: return new CelularAutomataWorld(name, seed, type, width, height, map, spawnPoint);
                case Constants.GenerationTypes.Templates: return new TemplatesWorld(name, seed, type, width, height, map, spawnPoint);
                default: return new RandomWorld(name, seed, type, width, height, map, spawnPoint);
            }
        }
    }

    public static byte[] GetWorld(string file)
    {
        if (!CheckIfWorldExists(file)) throw new System.Exception();
        return File.ReadAllBytes(GetWorldPathFromName(file));
    }

    public static bool CheckIfWorldExists(string file)
    {
        return File.Exists(GetWorldPathFromName(file));
    }

    public static string GetWorldPathFromName(string file)
    {
        return Constants.SaveWorldPath + file + Constants.WorldFileExtension;
    }

    public static List<string> GetAllWorlds()
    {
        var info = new DirectoryInfo(Constants.SaveWorldPath);
        var fileInfo = info.GetFiles("*.world");

        List<string> worldNames = new List<string>();
        for (int i = 0; i < fileInfo.Length; i++)
        {
            worldNames.Add(Path.GetFileNameWithoutExtension(fileInfo[i].FullName));
        }
        return worldNames;
    }

    public static void RemoveWorld(string file)
    {
        File.Delete(GetWorldPathFromName(file));
    }
}
