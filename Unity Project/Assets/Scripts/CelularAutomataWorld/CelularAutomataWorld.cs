using System.Collections.Generic;
using UnityEngine;

public class CelularAutomataWorld : World
{
    private const int initialPercentage = 53;
    private const int iterations = 4;
    private const int neighborhoodThreshold = 13;
    private const int neighborhoodSize = 2;

    struct phase2
    {
        public const int neighborhoodSize = 1; //Von Neumann
        public const float initialStones = 1 / 200f;
        public const float initialWaters = 1 / 1000f;
        public const float initialGolds = 1 / 2000f;
        public const int stoneExpansionProbability = 50;
        public const int waterExpansionProbability = 30;
        public const int goldExpansionProbability = 15;
        public const int realgarSpawnProbability = 5;

    }

    private readonly int initialPercentageLowerPart = 58;

    List<Tile[,]> allMaps;

    public CelularAutomataWorld(string newName, int newSeed, int newType, int newWidth, int newHeight, Tile[,] newMap, Vector2 newSpawnPoint) : base(newName, newSeed, newType, newWidth, newHeight, newMap, newSpawnPoint) { }
    public CelularAutomataWorld(string worldName, int seed, int[] size) : base(worldName, seed, size)
    {
        type = Constants.GenerationTypes.CelularAutomata;
        WorldGen();
        EndWorldCreation();
    }

    public override void WorldGen()
    {
        allMaps = new List<Tile[,]>();
        SetInitialGrid();
        IterateCA1();
        //SaveMapsAsImage(allMaps, "1"); Uncomment to Save map as 1.png into B://

        allMaps = new List<Tile[,]>();
        SetRandomTiles();
        IterateCA2();
        //SaveMapsAsImage(allMaps, "2");
    }

    private void SetInitialGrid()
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (row >= height - height / 10) map[col, row] = wc.allTiles[Constants.BlockTypes.Air];
                else if (row == 0 || col == 0 || col == width - 1) map[col, row] = wc.allTiles[Constants.BlockTypes.Dirt];
                else if (row < height / 10) map[col, row] = Random.Range(0, 100) < initialPercentageLowerPart ? wc.allTiles[Constants.BlockTypes.Dirt] : wc.allTiles[Constants.BlockTypes.Air];
                else map[col, row] = Random.Range(0, 100) < initialPercentage ? wc.allTiles[Constants.BlockTypes.Dirt] : wc.allTiles[Constants.BlockTypes.Air];

            }
        }
    }

    private void IterateCA1()
    {
        allMaps.Add(map);

        for (int iteration = 0; iteration < iterations; iteration++)
        {

            Tile[,] newMap = new Tile[width, height];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    newMap[col, row] = GetTileFromRulesPhase1(col, row);
                }
            }
            allMaps.Add(newMap);
            map = newMap;
        }
    }

    private Tile GetTileFromRulesPhase1(int currentCol, int currentRow)
    {
        int dirt = 0;
        int air = 0;
        for (int row = currentRow - neighborhoodSize; row <= currentRow + neighborhoodSize; row++)
        {
            for (int col = currentCol - neighborhoodSize; col <= currentCol + neighborhoodSize; col++)
            {
                if (row < 0 || row >= height || col < 0 || col >= width || (row == currentRow && col == currentCol)) continue;
                if (map[col, row].id == Constants.BlockTypes.Dirt) dirt++;
                else if (map[col, row].id == Constants.BlockTypes.Air) air++;

                if (dirt == neighborhoodThreshold) return wc.allTiles[Constants.BlockTypes.Dirt];
                else if (air == neighborhoodThreshold) return wc.allTiles[Constants.BlockTypes.Air];
            }
        }

        return map[currentCol, currentRow];
    }

    private void SetRandomTiles()
    {
        int randomStones = (int)(width * height * phase2.initialStones);
        while (randomStones > 0)
        {
            int row = Random.Range(0, height);
            int col = Random.Range(0, width);
            if (map[col, row].id == Constants.BlockTypes.Dirt)
            {
                map[col, row] = wc.allTiles[Constants.BlockTypes.Stone];
                randomStones--;
            }
        }

        int randomWater = (int)(width * height * phase2.initialWaters);
        while (randomWater > 0)
        {
            int row = Random.Range(0, height);
            int col = Random.Range(0, width);
            if (map[col, row].id == Constants.BlockTypes.Dirt)
            {
                map[col, row] = wc.allTiles[Constants.BlockTypes.Water];
                randomWater--;
            }
        }

        int randomGold = (int)(width * height * phase2.initialGolds);
        while (randomGold > 0)
        {
            int row = Random.Range(0, height);
            int col = Random.Range(0, width);
            if (map[col, row].id == Constants.BlockTypes.Dirt)
            {
                map[col, row] = wc.allTiles[Constants.BlockTypes.Gold];
                randomGold--;
            }
        }
    }

    private void IterateCA2()
    {
        for (int iteration = 0; iteration < iterations; iteration++)
        {
            Tile[,] newMap = new Tile[width, height];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    newMap[col, row] = GetTileFromRulesPhase2(row, col);
                }
            }
            allMaps.Add(newMap);
            map = newMap;
        }
    }

    private Tile GetTileFromRulesPhase2(int currentRow, int currentCol)
    {
        if (map[currentCol, currentRow].id != Constants.BlockTypes.Dirt) return map[currentCol, currentRow];

        bool waterFound = false;
        bool goldFound = false;
        bool stoneFound = false;

        for (int row = currentRow - phase2.neighborhoodSize; row <= currentRow + phase2.neighborhoodSize; row++)
        {
            for (int col = currentCol - phase2.neighborhoodSize; col <= currentCol + phase2.neighborhoodSize; col++)
            {
                if (row != currentRow && col != currentCol) continue;

                if (row < 0 || row >= height || col < 0 || col >= width || (row == currentRow && col == currentCol)) continue;

                if (map[col, row].id == Constants.BlockTypes.Water) waterFound = true;
                if (map[col, row].id == Constants.BlockTypes.Gold) goldFound = true;
                if (map[col, row].id == Constants.BlockTypes.Stone) stoneFound = true;
            }
        }
        if (waterFound && Random.Range(0, 100) < phase2.realgarSpawnProbability) return wc.allTiles[Constants.BlockTypes.Realgar];
        else if (waterFound && Random.Range(0, 100) < phase2.waterExpansionProbability) return wc.allTiles[Constants.BlockTypes.Water];
        else if (goldFound && Random.Range(0, 100) < phase2.goldExpansionProbability) return wc.allTiles[Constants.BlockTypes.Gold];
        else if (stoneFound && Random.Range(0, 100) < phase2.stoneExpansionProbability) return wc.allTiles[Constants.BlockTypes.Stone];
        return map[currentCol, currentRow];
    }
}
