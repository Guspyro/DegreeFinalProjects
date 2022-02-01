
using System.Collections.Generic;
using UnityEngine;

public class Tree : Structure
{
    static readonly string[,] tree1 =
    {
        {"05", "05", "05"},
        {"05", "04", "05"},
        {"..", "04", ".."},
        {"..", "04", ".."},
        {"..", "04", ".."}
    };
    static readonly string[,] tree2 =
    {
        {"05", "05", "05"},
        {"05", "04", "05"},
        {"..", "04", ".."},
        {"..", "04", ".."}
    };
    List<string[,]> trees = new List<string[,]> { tree1, tree2 };
    public Tree()
    {
        string[,] tree = trees[Random.Range(0, trees.Count)];
        int width = tree.GetLength(1);
        int height = tree.GetLength(0);

        structureCenterCol = 1;
        structureCenterRow = 0;

        structure = new Tile[width, height];
        for (int row = height - 1; row >= 0; row--)
        {
            for (int col = 0; col < width; col++)
            {
                structure[col, height - 1 - row] = GetTile(tree[row, col]);
            }
        }
    }

    private Tile GetTile(string c)
    {
        switch (c)
        {
            case "..": return null;
            default: return WorldController.instance.allTiles[int.Parse(c)];
        }
    }
}
