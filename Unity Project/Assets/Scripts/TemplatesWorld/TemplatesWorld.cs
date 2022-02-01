using System.Collections.Generic;
using UnityEngine;

public class TemplatesWorld : World
{
    private Dictionary<string, List<Template>> templates = new Dictionary<string, List<Template>>();
    private bool sanctuaryPlaced = false;

    public TemplatesWorld(string newName, int newSeed, int newType, int newWidth, int newHeight, Tile[,] newMap, Vector2 newSpawnPoint) : base(newName, newSeed, newType, newWidth, newHeight, newMap, newSpawnPoint) { }
    public TemplatesWorld(string worldName, int seed, int[] size) : base(worldName, seed, size)
    {
        type = Constants.GenerationTypes.Templates;

        Template[] allTemplates = Resources.LoadAll<Template>("Templates");

        foreach (string type in Constants.TemplateTypes)
        {
            templates.Add(type, new List<Template>());
        }
        foreach (string type in Constants.SpecialTemplateTypes)
        {
            templates.Add(type, new List<Template>());
        }
        foreach (Template template in allTemplates)
        {
            templates[template.type].Add(template);
            template.Initialize();
        }
        WorldGen();
    }

    public override void WorldGen()
    {
        Template[,] templatesGrid = GetTemplatesGrid();

        for (int row = 0; row < height; row++)
        {
            for (int col = 0; col < width; col++)
            {
                Template template = templatesGrid[col / Constants.TemplateSize, row / Constants.TemplateSize];
                map[col, row] = GetTileFromChar(template.template[col % 10, row % 10]);
            }
        }
        EndWorldCreation();
    }

    private Template[,] GetTemplatesGrid()
    {
        Template[,] templatesGrid = new Template[width / Constants.TemplateSize, height / Constants.TemplateSize];

        for (int row = 0; row < templatesGrid.GetLength(1); row++)
        {
            for (int col = 0; col < templatesGrid.GetLength(0); col++)
            {
                if (row == 0)
                {
                    templatesGrid[col, row] = GetRandomTemplateOfType("none");
                }
                else if (row == templatesGrid.GetLength(1) - 1)
                {
                    templatesGrid[col, row] = GetRandomTemplateOfType("empty");
                }
                else if (row == templatesGrid.GetLength(1) - 2)
                {
                    templatesGrid[col, row] = GetRandomTemplateOfType("surface");
                }
                else if (col == 0)
                {
                    templatesGrid[col, row] = GetRandomTemplateWithoutExitOn("left");
                }
                else if (col == templatesGrid.GetLength(0) - 1)
                {
                    templatesGrid[col, row] = GetRandomTemplateWithoutExitOn("right");
                }
                else
                {
                    int random = Random.Range(0, 100);
                    if (random < 70)
                    {
                        templatesGrid[col, row] = GetRandomTemplateOfType("none");
                    }
                    else if (!sanctuaryPlaced && random < 73)
                    {
                        templatesGrid[col, row] = GetRandomTemplateOfType("sanctuary");
                        sanctuaryPlaced = true;
                    }
                    else
                    {

                        templatesGrid[col, row] = GetRandomTemplateOfType(Constants.TemplateTypes[Random.Range(0, Constants.TemplateTypes.Length)]);

                        bool hasLeftExit = templatesGrid[col-1, row].type.ToLower().Contains("right");

                        if (hasLeftExit && Random.Range(0, 100) < 70)
                        {
                            templatesGrid[col, row] = GetRandomTemplateWithExitOn("left");
                        }

                    }

                }
            }
        }
        return templatesGrid;
    }

    private Template GetRandomTemplateOfType(string type)
    {
        return templates[type][Random.Range(0, templates[type].Count)];
    }

    private Template GetRandomTemplateWithoutExitOn(string noExit)
    {
        string type = noExit;
        while (type.ToLower().Contains(noExit))
        {
            if (Random.Range(0, 100) < 50)
            {
                type = "none";
            }
            else
            {
                type = Constants.TemplateTypes[Random.Range(0, Constants.TemplateTypes.Length)];
            }
        }
        return GetRandomTemplateOfType(type);
    }

    private Template GetRandomTemplateWithExitOn(string exit)
    {
        string type = "";
        while (!type.ToLower().Contains(exit))
        {
            type = Constants.TemplateTypes[Random.Range(0, Constants.TemplateTypes.Length)];
        }
        return GetRandomTemplateOfType(type);
    }

    private Tile GetTileFromChar(char c)
    {
        switch (c)
        {
            case '1': return wc.allTiles[Constants.BlockTypes.Dirt];
            case '2': return Random.Range(0, 100) < 50 ? wc.allTiles[Constants.BlockTypes.Air] : wc.allTiles[Constants.BlockTypes.Dirt];
            case 'a': return Random.Range(0, 100) < 70 ? wc.allTiles[Constants.BlockTypes.Water] : wc.allTiles[Constants.BlockTypes.Dirt];
            case 'g': return wc.allTiles[Constants.BlockTypes.Gold];
            case 'l': return wc.allTiles[Constants.BlockTypes.Leaves];
            case 'r':
                int rand = Random.Range(0, 100);
                if (rand < 70) return wc.allTiles[Constants.BlockTypes.Water];
                else if (rand < 90) return wc.allTiles[Constants.BlockTypes.Stone];
                else return wc.allTiles[Constants.BlockTypes.Realgar];
            case 's': return Random.Range(0, 100) < 70 ? wc.allTiles[Constants.BlockTypes.Stone] : wc.allTiles[Constants.BlockTypes.Dirt];
            case 'w': return wc.allTiles[Constants.BlockTypes.Wood];
            default: return wc.allTiles[Constants.BlockTypes.Air];
        }
    }
}
