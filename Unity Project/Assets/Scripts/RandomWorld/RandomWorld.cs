using UnityEngine;

public class RandomWorld : World
{
    public RandomWorld(string newName, int newSeed, int newType, int newWidth, int newHeight, Tile[,] newMap, Vector2 newSpawnPoint) : base(newName, newSeed, newType, newWidth, newHeight, newMap, newSpawnPoint) { }
    public RandomWorld(string worldName, int seed, int[] size) : base(worldName, seed, size) {
        type = Constants.GenerationTypes.RandomGeneration;
        WorldGen();
        EndWorldCreation();
    }

    public override void WorldGen()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x, y] = wc.allTiles[Random.Range(0, 3)];
            }
        }
    }
}
