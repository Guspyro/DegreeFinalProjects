using System.Collections.Generic;
using UnityEngine;

public class FinalWorld : World
{

    private const float undergroundHeight = 0.2f;
    private const float surfaceHeight = 0.8f;

    struct phaseTerrain
    {
        public const int iterations = 4;
        public const int initialPercentage = 53;
        public const int initialPercentageUnderground = 58;
        public const int neighborhoodThreshold = 13;
        public const int neighborhoodSize = 2;
    }

    struct phasePlanetoids
    {
        public const int planetoidMinRadius = 7;
        public const int planetoidMaxRadius = 12;
        public const int planetoidNumber = 2;
        public const float minWidth = 0.2f;
        public const float maxWidth = 0.8f;
        public const float minHeight = 0.9f;
        public const float maxHeight = 0.94f;
    }

    struct phaseLiquids
    {
        public const int iterations = 4;
        public const int neighborhoodSize = 1; //Von Neumann
        public const float initialWaters = 1 / 1000f;
        public const float initialMagmas = 1 / 2000f;
        public const int waterExpansionProbability = 30;
        public const int magmaExpansionProbability = 50;
        public const int realgarSpawnProbability = 5;
        public const float magmaHeight = 0.008f;
        public const int seas = 2;
    }

    struct phaseOres
    {
        public const int iterations = 4;
        public const int neighborhoodSize = 1; //Von Neumann
        public const float initialStones = 1 / 200f;
        public const float initialGolds = 1 / 2000f;
        public const float initialIrons = 1 / 2000f;
        public const int stoneExpansionProbability = 50;
        public const int goldExpansionProbability = 15;
        public const int ironExpansionProbability = 25;
    }

    struct phaseLeftBiome
    {
        public const float leftMin = 0.2f;
        public const float leftMax = 0.24f;
        public const float rightMin = 0.40f;
        public const float rightMax = 0.44f;
    }

    struct phaseRightBiome
    {
        public const float leftMin = 0.6f;
        public const float leftMax = 0.64f;
        public const float rightMin = 0.80f;
        public const float rightMax = 0.84f;
    }

    struct biomeCommon
    {
        public const float heightMin = 0.35f;
        public const float heightMax = 0.45f;
        public const float heightVariation = 0.02f;
    }

    struct phaseUndergroundBiome
    {
        public const float upMin = 0.06f;
        public const float upMax = 0.07f;
        public const float downMin = 0.006f;
        public const float downMax = 0.02f;
        public const int iterations = 4;
        public const int initialPercentage = 50;
        public const int neighborhoodThreshold = 13;
        public const int neighborhoodSize = 2;
        public const int crystalProbability = 30;
    }

    struct phaseMiniBiome
    {
        public const int miniBiomeNumber = 3;
        public const int iterations = 4;
        public const int initialPercentage = 50;
        public const int neighborhoodThreshold = 13;
        public const int neighborhoodSize = 2;

        public const int expandIterations = 4;
        public const int expandNeighborhoodSize = 1;
        public const int expandProbability = 70;

        public const int rubyProbability = 20;
    }

    struct phaseStructures
    {
        public const int minTrees = 40;
        public const int maxTrees = 50;
        public const float minLabHeight = 0.2f;
        public const float maxLabHeight = 0.3f;
        public const float minLabWidth = 0.2f;
        public const float maxLabWidth = 0.3f;
        public const float minDungeonWidth = 0.6f;
        public const float maxDungeonWidth = 0.75f;
    }

    List<Vector2> planetoidCenters;
    private bool biomeLeft = true;
    private float biomeHeight = 0f;
    private int surfaceRow = 0;

    List<Tile[,]> allMaps;

    public FinalWorld(string newName, int newSeed, int newType, int newWidth, int newHeight, Tile[,] newMap, Vector2 newSpawnPoint) : base(newName, newSeed, newType, newWidth, newHeight, newMap, newSpawnPoint) { }
    public FinalWorld(string worldName, int seed, int[] size) : base(worldName, seed, size)
    {
        type = Constants.GenerationTypes.Final;
        WorldGen();
        EndWorldCreation();
    }

    public override void WorldGen()
    {
        allMaps = new List<Tile[,]>();

        surfaceRow = (int)(height * surfaceHeight);
        SetInitialGrid();
        SetPlanetoids();
        Iterate(phaseTerrain.iterations, GetTileFromRulesTerrain);

        SetLiquids();
        Iterate(phaseLiquids.iterations, GetTileFromRulesLiquids);

        SetOres();
        Iterate(phaseOres.iterations, GetTileFromRulesOres);
        
        biomeLeft = CheckProbability(50);
        biomeHeight = Random.Range(biomeCommon.heightMin, biomeCommon.heightMax);
        Iterate(1, GetTileFromRulesTundra);
        biomeLeft = !biomeLeft;
        biomeHeight = Random.Range(biomeCommon.heightMin, biomeCommon.heightMax);
        Iterate(1, GetTileFromRulesDesert);
        
        CreateMiniBiomes();
        Iterate(phaseMiniBiome.expandIterations, GetTileFromRulesExpandMiniBiome);
        Iterate(1, GetTileFromRulesRuby);

        SetLab();
        SetDungeon();
        SetTrees();

        SetUnderground();
        Iterate(phaseUndergroundBiome.iterations, GetTileFromRulesUnderground);
        Iterate(1, GetTileFromRulesCrystals);

        Iterate(1, GetTileFromSettleLiquids);
        Iterate(1, GetTileFromGrowGrass);
    }

    private void Iterate(int iterations, System.Func<int, int, Tile> getTileFromRule)
    {
        allMaps.Add(map);

        for (int iteration = 0; iteration < iterations; iteration++)
        {

            Tile[,] newMap = new Tile[width, height];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    newMap[col, row] = getTileFromRule(col, row);
                }
            }
            allMaps.Add(newMap);
            map = newMap;
        }
    }

    #region Terrain
    private void SetInitialGrid()
    {
        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (row >= surfaceRow) map[col, row] = wc.allTiles[Constants.BlockTypes.Air];
                else if (row == 0 || col == 0 || col == width - 1) map[col, row] = wc.allTiles[Constants.BlockTypes.Dirt];
                else if (row < height * undergroundHeight) map[col, row] = CheckProbability(phaseTerrain.initialPercentageUnderground) ? wc.allTiles[Constants.BlockTypes.Dirt] : wc.allTiles[Constants.BlockTypes.Air];
                else map[col, row] = CheckProbability(phaseTerrain.initialPercentage) ? wc.allTiles[Constants.BlockTypes.Dirt] : wc.allTiles[Constants.BlockTypes.Air];
            }
        }
    }

    private void SetPlanetoids()
    {
        planetoidCenters = new List<Vector2>();
        for (int i = 0; i < phasePlanetoids.planetoidNumber; i++)
        {
            int planetoidCol = (int)(width * Random.Range(phasePlanetoids.minWidth, phasePlanetoids.maxWidth));
            int planetoidRow = (int)(height * Random.Range(phasePlanetoids.minHeight, phasePlanetoids.maxHeight));
            int planetoidRadius = Random.Range(phasePlanetoids.planetoidMinRadius, phasePlanetoids.planetoidMaxRadius);

            planetoidCenters.Add(new Vector2(planetoidCol, planetoidRow));
            int squaredRadius = planetoidRadius * planetoidRadius;
            for (int row = -planetoidRadius; row < planetoidRadius; row++)
            {
                for (int col = -planetoidRadius; col < planetoidRadius; col++)
                {
                    if (row * row + col * col < squaredRadius) map[col + planetoidCol, row + planetoidRow] = wc.allTiles[Constants.BlockTypes.Dirt];
                }
            }
        }
    }

    private Tile GetTileFromRulesTerrain(int currentCol, int currentRow)
    {
        int dirt = 0;
        int air = 0;
        for (int row = currentRow - phaseTerrain.neighborhoodSize; row <= currentRow + phaseTerrain.neighborhoodSize; row++)
        {
            for (int col = currentCol - phaseTerrain.neighborhoodSize; col <= currentCol + phaseTerrain.neighborhoodSize; col++)
            {
                if (row < 0 || row >= height || col < 0 || col >= width || (row == currentRow && col == currentCol)) continue;
                if (map[col, row].id == Constants.BlockTypes.Dirt) dirt++;
                else if (map[col, row].id == Constants.BlockTypes.Air) air++;

                if (dirt == phaseTerrain.neighborhoodThreshold) return wc.allTiles[Constants.BlockTypes.Dirt];
                else if (air == phaseTerrain.neighborhoodThreshold) return wc.allTiles[Constants.BlockTypes.Air];
            }
        }

        return map[currentCol, currentRow];
    }
    #endregion

    #region Liquids
    private void SetLiquids()
    {
        int planetoidCoreRadius = Random.Range(2, 3);
        int squaredRadius = planetoidCoreRadius * planetoidCoreRadius;

        for (int row = -planetoidCoreRadius; row < planetoidCoreRadius; row++)
        {
            for (int col = -planetoidCoreRadius; col < planetoidCoreRadius; col++)
            {
                if (row * row + col * col < squaredRadius) map[(int)planetoidCenters[0].x + col, (int)planetoidCenters[0].y + row] = wc.allTiles[Constants.BlockTypes.Water];
            }
        }

        int randomWater = (int)(width * height * phaseLiquids.initialWaters);
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


        int randomSeas = phaseLiquids.seas;
        while (randomSeas > 0)
        {
            int undergroundSeaRadius = Random.Range(12, 16);
            int squaredSeaRadius = undergroundSeaRadius * undergroundSeaRadius;
            int seaRow = (int)(height * Random.Range(undergroundHeight, surfaceHeight - 0.04f));
            int seaCol = Random.Range(5, width - 5);
            if (map[seaCol, seaRow].id == Constants.BlockTypes.Air) continue;
            for (int row = -undergroundSeaRadius; row < undergroundSeaRadius; row++)
            {
                for (int col = -undergroundSeaRadius; col < undergroundSeaRadius; col++)
                {
                    if (seaCol + col < 0 || seaCol + col >= width) continue;
                    if (row * row + col * col < squaredSeaRadius) map[seaCol + col, seaRow + row] = wc.allTiles[Constants.BlockTypes.Water];
                }
            }
            randomSeas--;
        }

        int randomMagma = (int)(width * height * phaseLiquids.initialMagmas);
        while (randomMagma > 0)
        {
            int row = Random.Range(0, (int)(height * 0.2));
            int col = Random.Range(0, width);
            if (map[col, row].id == Constants.BlockTypes.Dirt)
            {
                map[col, row] = wc.allTiles[Constants.BlockTypes.Magma];
                randomMagma--;
            }
        }

        for (int row = 0; row <= height * phaseLiquids.magmaHeight; row++)
        {
            for (int col = 0; col < width; col++)
            {
                map[col, row] = wc.allTiles[Constants.BlockTypes.Magma];
            }
        }
    }

    private Tile GetTileFromRulesLiquids(int currentCol, int currentRow)
    {
        if (map[currentCol, currentRow].id != Constants.BlockTypes.Dirt) return map[currentCol, currentRow];

        bool waterFound = false;
        bool magmaFound = false;

        for (int row = currentRow - phaseLiquids.neighborhoodSize; row <= currentRow + phaseLiquids.neighborhoodSize; row++)
        {
            for (int col = currentCol - phaseLiquids.neighborhoodSize; col <= currentCol + phaseLiquids.neighborhoodSize; col++)
            {
                if (row != currentRow && col != currentCol) continue;

                if (row < 0 || row >= height || col < 0 || col >= width || (row == currentRow && col == currentCol)) continue;

                if (map[col, row].id == Constants.BlockTypes.Water)
                {
                    waterFound = true;
                    break;
                }
                else if (map[col, row].id == Constants.BlockTypes.Magma)
                {
                    magmaFound = true;
                    break;
                }
            }
        }
        if (waterFound && CheckProbability(phaseLiquids.realgarSpawnProbability)) return wc.allTiles[Constants.BlockTypes.Realgar];
        else if (waterFound && CheckProbability(phaseLiquids.waterExpansionProbability)) return wc.allTiles[Constants.BlockTypes.Water];
        else if (magmaFound && CheckProbability(phaseLiquids.magmaExpansionProbability)) return wc.allTiles[Constants.BlockTypes.Magma];

        return map[currentCol, currentRow];
    }
    #endregion

    #region Ores
    private void SetOres()
    {
        int planetoidCoreRadius = Random.Range(2, 3);
        int squaredRadius = planetoidCoreRadius * planetoidCoreRadius;

        for (int row = -planetoidCoreRadius; row < planetoidCoreRadius; row++)
        {
            for (int col = -planetoidCoreRadius; col < planetoidCoreRadius; col++)
            {
                if (row * row + col * col < squaredRadius) map[(int)planetoidCenters[1].x + col, (int)planetoidCenters[1].y + row] = wc.allTiles[Constants.BlockTypes.GoldOre];
            }
        }

        int randomStones = (int)(width * height * phaseOres.initialStones);
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

        int randomGold = (int)(width * height * phaseOres.initialGolds);
        while (randomGold > 0)
        {
            int row = Random.Range(0, height);
            int col = Random.Range(0, width);
            if (map[col, row].id == Constants.BlockTypes.Dirt)
            {
                map[col, row] = wc.allTiles[Constants.BlockTypes.GoldOre];
                randomGold--;
            }
        }

        int randomIron = (int)(width * height * phaseOres.initialIrons);
        while (randomIron > 0)
        {
            int row = Random.Range(0, height);
            int col = Random.Range(0, width);
            if (map[col, row].id == Constants.BlockTypes.Dirt)
            {
                map[col, row] = wc.allTiles[Constants.BlockTypes.IronOre];
                randomIron--;
            }
        }
    }

    private Tile GetTileFromRulesOres(int currentCol, int currentRow)
    {
        if (map[currentCol, currentRow].id != Constants.BlockTypes.Dirt) return map[currentCol, currentRow];

        bool goldFound = false;
        bool ironFound = false;
        bool stoneFound = false;

        for (int row = currentRow - phaseOres.neighborhoodSize; row <= currentRow + phaseOres.neighborhoodSize; row++)
        {
            for (int col = currentCol - phaseOres.neighborhoodSize; col <= currentCol + phaseOres.neighborhoodSize; col++)
            {
                if (row != currentRow && col != currentCol) continue;

                if (row < 0 || row >= height || col < 0 || col >= width || (row == currentRow && col == currentCol)) continue;

                if (map[col, row].id == Constants.BlockTypes.GoldOre) goldFound = true;
                if (map[col, row].id == Constants.BlockTypes.IronOre) ironFound = true;
                if (map[col, row].id == Constants.BlockTypes.Stone) stoneFound = true;
            }
        }

        if (goldFound && CheckProbability(phaseOres.goldExpansionProbability)) return wc.allTiles[Constants.BlockTypes.GoldOre];
        else if (ironFound && CheckProbability(phaseOres.ironExpansionProbability)) return wc.allTiles[Constants.BlockTypes.IronOre];
        else if (stoneFound && CheckProbability(phaseOres.stoneExpansionProbability)) return wc.allTiles[Constants.BlockTypes.Stone];
        return map[currentCol, currentRow];
    }
    #endregion

    #region Biomes
    private int getBiomeLimit(bool left, int pos)
    {
        switch (pos)
        {
            case 0: return left ? (int)(width * Random.Range(phaseLeftBiome.leftMin, phaseLeftBiome.leftMax)) : (int)(width * Random.Range(phaseRightBiome.leftMin, phaseRightBiome.leftMax));
            case 1: return left ? (int)(width * Random.Range(phaseLeftBiome.rightMin, phaseLeftBiome.rightMax)) : (int)(width * Random.Range(phaseRightBiome.rightMin, phaseRightBiome.rightMax));
            default: return (int)(height * Random.Range(biomeHeight - biomeCommon.heightVariation, biomeHeight + biomeCommon.heightVariation));
        }

    }

    private Tile GetTileFromRulesTundra(int currentCol, int currentRow)
    {
        int leftLimit = getBiomeLimit(biomeLeft, 0);
        int rightLimit = getBiomeLimit(biomeLeft, 1);
        int downLimit = getBiomeLimit(biomeLeft, 2);
        if (currentCol < leftLimit || currentCol > rightLimit || currentRow < downLimit) return map[currentCol, currentRow];

        if (map[currentCol, currentRow].id == Constants.BlockTypes.Dirt) return wc.allTiles[Constants.BlockTypes.Snow];
        else if (map[currentCol, currentRow].id == Constants.BlockTypes.Stone) return wc.allTiles[Constants.BlockTypes.Ice];
        else if (map[currentCol, currentRow].id == Constants.BlockTypes.Water) return wc.allTiles[Constants.BlockTypes.BlueIce];

        return map[currentCol, currentRow];
    }

    private Tile GetTileFromRulesDesert(int currentCol, int currentRow)
    {
        int leftLimit = getBiomeLimit(biomeLeft, 0);
        int rightLimit = getBiomeLimit(biomeLeft, 1);
        int downLimit = getBiomeLimit(biomeLeft, 2);
        if (currentCol < leftLimit || currentCol > rightLimit || currentRow < downLimit) return map[currentCol, currentRow];

        if (map[currentCol, currentRow].id == Constants.BlockTypes.Dirt) return wc.allTiles[Constants.BlockTypes.Sand];
        else if (map[currentCol, currentRow].id == Constants.BlockTypes.Stone) return wc.allTiles[Constants.BlockTypes.SandStone];

        return map[currentCol, currentRow];
    }
    #endregion

    #region MiniBiome
    private void CreateMiniBiomes()
    {
        for (int i = 0; i < phaseMiniBiome.miniBiomeNumber; i++)
        {
            int planetoidCol = (int)(width * Random.Range(0.1f, 0.9f));
            int planetoidRow = (int)(height * Random.Range(0.1f, surfaceHeight - 0.1f));
            int biomeRadius = Random.Range(20, 30);

            for (int row = -biomeRadius; row < biomeRadius; row++)
            {
                for (int col = -biomeRadius; col < biomeRadius; col++)
                {
                    if (row * row + col * col < biomeRadius * biomeRadius)
                        map[col + planetoidCol, row + planetoidRow] = CheckProbability(phaseMiniBiome.initialPercentage) ? wc.allTiles[Constants.BlockTypes.Jade] : wc.allTiles[Constants.BlockTypes.Air];
                }
            }

            for (int iteration = 0; iteration < phaseMiniBiome.iterations; iteration++)
            {
                Tile[,] newMap = map.Clone() as Tile[,];
                for (int row = -biomeRadius; row < biomeRadius; row++)
                {
                    for (int col = -biomeRadius; col < biomeRadius; col++)
                    {
                        if (row * row + col * col < biomeRadius * biomeRadius)
                            newMap[col + planetoidCol, row + planetoidRow] = GetTileFromRulesMiniBiome(col + planetoidCol, row + planetoidRow);
                    }
                }
                map = newMap;
            }
        }
    }

    private Tile GetTileFromRulesMiniBiome(int currentCol, int currentRow)
    {
        int air = 0;
        for (int row = currentRow - phaseMiniBiome.neighborhoodSize; row <= currentRow + phaseMiniBiome.neighborhoodSize; row++)
        {
            for (int col = currentCol - phaseMiniBiome.neighborhoodSize; col <= currentCol + phaseMiniBiome.neighborhoodSize; col++)
            {
                if (row < 0 || row >= height || col < 0 || col >= width || (row == currentRow && col == currentCol)) continue;
                if (map[col, row].id == Constants.BlockTypes.Air) air++;
            }
        }

        if (air >= phaseMiniBiome.neighborhoodThreshold) return wc.allTiles[Constants.BlockTypes.Air];
        else return wc.allTiles[Constants.BlockTypes.Jade];
    }

    private Tile GetTileFromRulesRuby(int currentCol, int currentRow)
    {
        if (currentRow - 1 < 0 || currentCol - 1 < 0 || currentCol + 1 >= width) return map[currentCol, currentRow];

        if (map[currentCol, currentRow].id == Constants.BlockTypes.Air &&
            (map[currentCol - 1, currentRow].id == Constants.BlockTypes.Jade || map[currentCol + 1, currentRow].id == Constants.BlockTypes.Jade) &&
            map[currentCol, currentRow - 1].id == Constants.BlockTypes.Air &&
            map[currentCol, currentRow + 1].id == Constants.BlockTypes.Air &&
            CheckProbability(phaseMiniBiome.rubyProbability))
            return wc.allTiles[Constants.BlockTypes.Ruby];
        return map[currentCol, currentRow];
    }

    private Tile GetTileFromRulesExpandMiniBiome(int currentCol, int currentRow)
    {
        if (map[currentCol, currentRow].id != Constants.BlockTypes.Dirt) return map[currentCol, currentRow];

        for (int row = currentRow - phaseMiniBiome.expandNeighborhoodSize; row <= currentRow + phaseMiniBiome.expandNeighborhoodSize; row++)
        {
            for (int col = currentCol - phaseMiniBiome.expandNeighborhoodSize; col <= currentCol + phaseMiniBiome.expandNeighborhoodSize; col++)
            {
                if (row < 0 || row >= height || col < 0 || col >= width || (row == currentRow && col == currentCol)) continue;
                if (map[col, row].id == Constants.BlockTypes.Jade && map[col, row].id != Constants.BlockTypes.Air)
                {
                    if (CheckProbability(phaseMiniBiome.expandProbability))
                        return wc.allTiles[Constants.BlockTypes.Jade];
                    else
                        return map[currentCol, currentRow];
                }
            }
        }
        return map[currentCol, currentRow];
    }
    #endregion

    #region Structures
    private void SetTrees()
    {
        int treesNumber = Random.Range(phaseStructures.minTrees, phaseStructures.maxTrees);
        int maxRetries = 20;
        while (treesNumber > 0 && maxRetries > 0)
        {
            int treeRow = height;
            int treeCol = (int)(width * Random.Range(0.05f, 0.95f));
            bool goodPlace = false;
            while (treeRow > height / 2)
            {
                if (map[treeCol, treeRow - 1].id != 0)
                {
                    int downId = map[treeCol, treeRow - 1].id;
                    if ((downId == Constants.BlockTypes.Dirt || downId == Constants.BlockTypes.Snow) && map[treeCol - 1, treeRow + 1].id == Constants.BlockTypes.Air && map[treeCol + 1, treeRow + 1].id == Constants.BlockTypes.Air)
                    {
                        goodPlace = true;
                    }
                    break;
                }
                treeRow--;
            }

            if (!goodPlace)
            {
                maxRetries--;
                continue;
            }

            Structure tree = new Tree();
            treeCol -= tree.structureCenterCol;
            treeRow -= tree.structureCenterRow;
            PlaceStructure(tree, treeCol, treeRow);
            treesNumber--;
        }
    }

    private void SetLab()
    {
        int labRow = (int)(height * Random.Range(phaseStructures.minLabHeight, phaseStructures.maxLabHeight));
        int labCol = (int)(width * Random.Range(phaseStructures.minLabWidth, phaseStructures.maxLabWidth));

        Structure lab = new Lab();
        labCol -= lab.structureCenterCol;
        labRow -= lab.structureCenterRow;
        PlaceStructure(lab, labCol, labRow);
    }

    private void SetDungeon()
    {
        Structure dungeon = new Dungeon(width, height);

        int dungeonRow = (int)(height * 0.85);
        int dungeonCol = (int)(width * Random.Range(phaseStructures.minDungeonWidth, phaseStructures.maxDungeonWidth));
        while (dungeonRow > height / 2)
        {
            if (map[dungeonCol, dungeonRow - 1].id != 0)
            {
                break;
            }
            dungeonRow--;
        }
        dungeonRow -= dungeon.structureCenterRow;
        dungeonCol -= dungeon.structureCenterCol;
        PlaceStructure(dungeon, dungeonCol, dungeonRow);
    }

    private void PlaceStructure(Structure structure, int structureCol, int structureRow)
    {
        for (int row = 0; row < structure.structure.GetLength(1); row++)
        {
            for (int col = 0; col < structure.structure.GetLength(0); col++)
            {
                int mapCol = structureCol + col;
                int mapRow = structureRow + row;
                if (mapCol < 0 || mapCol >= width || mapRow < 0 || mapRow >= height) continue;
                if (structure.structure[col, row] != null)
                {
                    map[mapCol, mapRow] = structure.structure[col, row];
                }

            }
        }
    }
    #endregion

    #region Underground
    private void SetUnderground()
    {
        int lowerMaxRow = (int)(height * phaseUndergroundBiome.downMax);
        int lowerMinRow = (int)(height * phaseUndergroundBiome.downMin);
        int probabilityIncreaser = (lowerMaxRow - lowerMinRow) / 2;
        for (int row = lowerMinRow; row < lowerMaxRow; row++)
        {
            for (int col = 0; col < width; col++)
            {
                map[col, row] = CheckProbability(20 * (row - probabilityIncreaser)) ? wc.allTiles[Constants.BlockTypes.Obsidian] : wc.allTiles[Constants.BlockTypes.Magma];
            }
        }

        for (int row = lowerMaxRow; row < height * phaseUndergroundBiome.upMin; row++)
        {
            for (int col = 0; col < width; col++)
            {
                if (map[col, row].id != Constants.BlockTypes.Magma)
                    map[col, row] = CheckProbability(phaseUndergroundBiome.initialPercentage) ? wc.allTiles[Constants.BlockTypes.Obsidian] : wc.allTiles[Constants.BlockTypes.Air];
            }
        }

        int lowerLimit = (int)(height * phaseUndergroundBiome.upMin);
        for (int row = 0; row < height * phaseUndergroundBiome.upMax - lowerLimit; row++)
        {
            for (int col = 0; col < width; col++)
            {
                map[col, row + lowerLimit] = CheckProbability(100 - 15 * row) ? wc.allTiles[Constants.BlockTypes.Obsidian] : map[col, row + lowerLimit];
            }
        }
    }

    private Tile GetTileFromRulesUnderground(int currentCol, int currentRow)
    {
        if (currentRow < height * phaseUndergroundBiome.downMax - 1 || currentRow > height * phaseUndergroundBiome.upMin + 1 || map[currentCol, currentRow].id == Constants.BlockTypes.Magma) return map[currentCol, currentRow];

        int air = 0;
        for (int row = currentRow - phaseUndergroundBiome.neighborhoodSize; row <= currentRow + phaseUndergroundBiome.neighborhoodSize; row++)
        {
            for (int col = currentCol - phaseUndergroundBiome.neighborhoodSize; col <= currentCol + phaseUndergroundBiome.neighborhoodSize; col++)
            {
                if (row < height * phaseUndergroundBiome.downMax || row > height * phaseUndergroundBiome.upMin || col < 0 || col >= width || (row == currentRow && col == currentCol)) continue;
                if (map[col, row].id == Constants.BlockTypes.Air) air++;
            }
        }

        if (air >= phaseUndergroundBiome.neighborhoodThreshold) return wc.allTiles[Constants.BlockTypes.Air];
        else return wc.allTiles[Constants.BlockTypes.Obsidian];
    }

    private Tile GetTileFromRulesCrystals(int currentCol, int currentRow)
    {
        if (currentRow <= height * phaseUndergroundBiome.downMax || currentRow >= height * phaseUndergroundBiome.upMin) return map[currentCol, currentRow];

        if (map[currentCol, currentRow].id == Constants.BlockTypes.Air &&
            map[currentCol, currentRow + 1].id == Constants.BlockTypes.Obsidian &&
            CheckProbability(phaseUndergroundBiome.crystalProbability))
            return wc.allTiles[Constants.BlockTypes.Crystal];

        return map[currentCol, currentRow];
    }
    #endregion

    #region SettleLiquids
    private Tile GetTileFromSettleLiquids(int currentCol, int currentRow)
    {
        if (map[currentCol, currentRow].id != Constants.BlockTypes.Water && map[currentCol, currentRow].id != Constants.BlockTypes.Magma) return map[currentCol, currentRow];

        if ((currentCol - 1 >= 0 && map[currentCol - 1, currentRow].id == Constants.BlockTypes.Air)
            || (currentCol + 1 < width && map[currentCol + 1, currentRow].id == Constants.BlockTypes.Air)
                || (currentRow - 1 >= 0 && map[currentCol, currentRow - 1].id == Constants.BlockTypes.Air))
        {
            return wc.allTiles[Constants.BlockTypes.Dirt];
        }

        return map[currentCol, currentRow];
    }
    #endregion

    #region Grass
    private Tile GetTileFromGrowGrass(int currentCol, int currentRow)
    {
        if (currentRow < (int)(height * (surfaceHeight - 0.05)) || map[currentCol, currentRow].id != Constants.BlockTypes.Dirt) return map[currentCol, currentRow];

        if (currentRow + 1 < height && map[currentCol, currentRow + 1].id == Constants.BlockTypes.Air)
        {
            return wc.allTiles[Constants.BlockTypes.Grass];
        }

        return map[currentCol, currentRow];
    }
    #endregion

}
