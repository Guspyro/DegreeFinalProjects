using UnityEngine;

public class PerlinNoiseWorld : World
{
    public PerlinNoiseWorld(string newName, int newSeed, int newType, int newWidth, int newHeight, Tile[,] newMap, Vector2 newSpawnPoint) : base(newName, newSeed, newType, newWidth, newHeight, newMap, newSpawnPoint) { }
    public PerlinNoiseWorld(string worldName, int seed, int[] size) : base(worldName, seed, size)
    {
        type = Constants.GenerationTypes.PerlinNoise;
        WorldGen();
        EndWorldCreation();
    }

    public float[,] perlinValues;
    const float scale = 0.05f;
    const float scalePhase2 = 0.07f;

    const float stoneThreshold = 0.17f;
    const float dirtThreshold = 0.6f;

    const float waterThreshold = 0.15f;
    const float realgarThreshold = 0.16f;
    const int realgarProbability = 30;
    const float goldThreshold = 0.9f;

    public override void WorldGen()
    {
        perlinValues = new float[width, height];
        int newSeed = Random.Range(0, 10000000);

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {

                float perlinRow = (newSeed + row) * scale;
                float perlinCol = (newSeed + col) * scale;
                float noiseValue = Mathf.PerlinNoise(perlinRow, perlinCol);

                if (row >= height - height / 10) noiseValue = 1;

                perlinValues[col, row] = noiseValue;
                map[col, row] = GetTileFromNoiseValue(noiseValue);
            }
        }

        newSeed = Random.Range(0, 10000000);

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                float perlinRow = (newSeed + row) * scalePhase2;
                float perlinCol = (newSeed + col) * scalePhase2;
                float noiseValue = Mathf.PerlinNoise(perlinRow, perlinCol);

                map[col, row] = GetTileFromNoiseValuePhase2(noiseValue, map[col, row]);
            }
        }

    }

    private Tile GetTileFromNoiseValue(float noiseValue)
    {
        if (noiseValue < stoneThreshold) return wc.allTiles[Constants.BlockTypes.Stone];
        else if (noiseValue < dirtThreshold) return wc.allTiles[Constants.BlockTypes.Dirt];

        return wc.allTiles[Constants.BlockTypes.Air];
    }

    private Tile GetTileFromNoiseValuePhase2(float noiseValue, Tile currentTile)
    {
        if (currentTile.id == Constants.BlockTypes.Dirt)
        {
            if (noiseValue < waterThreshold) return wc.allTiles[Constants.BlockTypes.Water];
            else if (noiseValue < realgarThreshold && Random.Range(0, 100) < realgarProbability) return wc.allTiles[Constants.BlockTypes.Realgar];
            else if (noiseValue > goldThreshold) return wc.allTiles[Constants.BlockTypes.Gold];
        }
        return currentTile;
    }
}
