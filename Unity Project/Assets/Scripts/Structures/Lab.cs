using System.Collections.Generic;
using UnityEngine;

public class Lab : Structure
{
    static readonly string[,] core =
    {
        {"..", "..", "..", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "..", "..", ".."},
        {"..", "..", "10", "10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10", "10", "..", ".."},
        {"..", "10", "10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10", "10", ".."},
        {"10", "10", "00", "00", "00", "00", "10", "10", "10", "10", "10", "00", "00", "00", "00", "10", "10"},
        {"10", "00", "00", "00", "00", "10", "10", "10", "10", "10", "10", "10", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "10", "10", "10", "11", "11", "11", "10", "10", "10", "00", "00", "00", "10"},
        {"10", "00", "00", "10", "10", "10", "11", "11", "11", "11", "11", "10", "10", "10", "00", "00", "10"},
        {"10", "00", "00", "10", "10", "11", "11", "11", "11", "11", "11", "11", "10", "10", "00", "00", "10"},
        {"10", "00", "00", "10", "10", "11", "11", "11", "11", "11", "11", "11", "10", "10", "00", "00", "10"},
        {"10", "00", "00", "10", "10", "11", "11", "11", "11", "11", "11", "11", "10", "10", "00", "00", "10"},
        {"10", "00", "00", "10", "10", "10", "11", "11", "11", "11", "11", "10", "10", "10", "00", "00", "10"},
        {"00", "00", "00", "00", "10", "10", "10", "11", "11", "11", "10", "10", "10", "00", "00", "00", "00"},
        {"00", "00", "00", "00", "00", "10", "10", "10", "10", "10", "10", "10", "00", "00", "00", "00", "00"},
        {"10", "10", "00", "00", "00", "00", "10", "10", "10", "10", "10", "00", "00", "00", "00", "10", "10"},
        {"..", "10", "10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10", "10", ".."},
        {"..", "..", "10", "10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10", "10", "..", ".."},
        {"..", "..", "..", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "..", "..", ".."},
    };
    static readonly string[,] corridor =
    {
        {"10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {"10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10"},
    };
    static readonly string[,] rightStair =
    {
        {"..", "..", "..", "..", "10", "10", "10", "10", "10"},
        {"..", "..", "..", "10", "10", "00", "00", "00", "10"},
        {"..", "..", "10", "10", "00", "00", "00", "00", "00"},
        {"..", "10", "10", "00", "00", "00", "00", "00", "00"},
        {"10", "10", "00", "00", "00", "00", "10", "10", "10"},
        {"10", "00", "00", "00", "00", "10", "10", "..", ".."},
        {"00", "00", "00", "00", "10", "10", "..", "..", ".."},
        {"00", "00", "00", "10", "10", "..", "..", "..", ".."},
        {"10", "10", "10", "10", "..", "..", "..", "..", ".."},
    };
    static readonly string[,] leftStair =
    {
        {"10", "10", "10", "10", "10", "..", "..", "..", ".."},
        {"10", "00", "00", "00", "10", "10", "..", "..", ".."},
        {"00", "00", "00", "00", "00", "10", "10", "..", ".."},
        {"00", "00", "00", "00", "00", "00", "10", "10", ".."},
        {"10", "10", "00", "00", "00", "00", "00", "10", "10"},
        {"..", "10", "10", "00", "00", "00", "00", "00", "10"},
        {"..", "..", "10", "10", "00", "00", "00", "00", "00"},
        {"..", "..", "..", "10", "10", "00", "00", "00", "00"},
        {"..", "..", "..", "..", "10", "10", "10", "10", "10"},

    };

    static readonly string[,] room1 =
    {
        {"10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {"10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10"},
    };
    static readonly string[,] room2 =
    {
        {"10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"00", "00", "00", "10", "00", "00", "00", "00", "00", "10", "00", "00", "00"},
        {"00", "00", "00", "10", "11", "11", "11", "11", "11", "10", "00", "00", "00"},
        {"10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10"},
    };
    static readonly string[,] room3 =
    {
        {"10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "10"},
        {"10", "00", "00", "00", "00", "10", "hh", "hh", "10", "00", "00", "00", "00", "10"},
        {"00", "00", "00", "00", "00", "10", "gg", "gg", "10", "00", "00", "00", "00", "00"},
        {"00", "00", "00", "00", "00", "10", "gg", "gg", "10", "00", "00", "00", "00", "00"},
        {"10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10", "10"},
    };
    List<string[,]> leftCoridors = new List<string[,]> { leftStair, corridor };
    List<string[,]> rightCoridors = new List<string[,]> { rightStair, corridor };
    List<string[,]> rooms = new List<string[,]> { room1, room2, room3 };

    bool roomGold = false;

    public Lab()
    {
        int coreWidth = core.GetLength(1);
        int coreHeight = core.GetLength(0);

        int random = Random.Range(0, leftCoridors.Count);
        string[,] leftCorridor = leftCoridors[random];
        int leftCorridorWidth = leftCorridor.GetLength(1);
        int leftCorridorHeight = leftCorridor.GetLength(0);
        bool isLeftStairs = random == 0;

        random = Random.Range(0, rightCoridors.Count);
        string[,] rightCorridor = rightCoridors[random];
        int rightCorridorWidth = rightCorridor.GetLength(1);
        int rightCorridorHeight = rightCorridor.GetLength(0);
        bool isRightStairs = random == 0;

        string[,] room = rooms[Random.Range(0, rooms.Count)];
        int roomWidth = room.GetLength(1);
        int roomHeight = room.GetLength(0);

        string[,] room2 = rooms[Random.Range(0, rooms.Count)];
        int roomWidth2 = room2.GetLength(1);
        int roomHeight2 = room2.GetLength(0);

        int width = coreWidth + leftCorridorWidth + rightCorridorWidth + roomWidth + roomWidth2;
        int height = coreHeight + leftCorridorHeight + rightCorridorHeight + roomHeight + roomHeight2;

        structure = new Tile[width, height];
        int currentCol = 0;
        int currentRow = 0;

        if (isLeftStairs) currentRow += leftCorridorHeight - 5;

        roomGold = Random.Range(0, 3) < 1;
        for (int row = roomHeight - 1; row >= 0; row--)
        {
            for (int col = 0; col < roomWidth; col++)
            {
                structure[col + currentCol, roomHeight - row + 2 + currentRow] = GetTile(room[row, col]);
            }
        }
        currentCol += roomWidth;
        if (isLeftStairs) currentRow = 0;

        for (int row = leftCorridorHeight - 1; row >= 0; row--)
        {
            for (int col = 0; col < leftCorridorWidth; col++)
            {
                structure[col + currentCol, leftCorridorHeight - row + 2 + currentRow] = GetTile(leftCorridor[row, col]);
            }
        }
        currentCol += leftCorridorWidth;

        structureCenterCol = currentCol;
        structureCenterRow = currentRow;
        for (int row = coreHeight - 1; row >= 0; row--)
        {
            for (int col = 0; col < coreWidth; col++)
            {
                structure[col + currentCol, coreHeight - 1 - row + currentRow] = GetTile(core[row, col]);
            }
        }
        currentCol += coreWidth;

        for (int row = rightCorridorHeight - 1; row >= 0; row--)
        {
            for (int col = 0; col < rightCorridorWidth; col++)
            {
                structure[col + currentCol, rightCorridorHeight - row + 2 + currentRow] = GetTile(rightCorridor[row, col]);
            }
        }
        currentCol += rightCorridorWidth;
        if (isRightStairs) currentRow += rightCorridorHeight - 5;


        roomGold = Random.Range(0, 3) < 1;
        for (int row = roomHeight2 - 1; row >= 0; row--)
        {
            for (int col = 0; col < roomWidth2; col++)
            {
                structure[col + currentCol, roomHeight2 - row + 2 + currentRow] = GetTile(room2[row, col]);
            }
        }
    }

    private Tile GetTile(string c)
    {
        switch (c)
        {
            case "..": return null;
            case "gg": return roomGold ? WorldController.instance.allTiles[Constants.BlockTypes.Gold] : WorldController.instance.allTiles[Constants.BlockTypes.Air];
            case "hh": return roomGold ? WorldController.instance.allTiles[Constants.BlockTypes.Iron] : WorldController.instance.allTiles[Constants.BlockTypes.Air];
            default: return WorldController.instance.allTiles[int.Parse(c)];
        }
    }
}
