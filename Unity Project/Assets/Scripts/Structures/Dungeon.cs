using System.Collections.Generic;
using UnityEngine;

public class Dungeon : Structure
{
    class Agent
    {

        public int startCol = 0;
        public int startRow = 0;
        public bool goingRight = true;
    }
    static readonly string[,] entrance =
    {
        {"..", "..", "..", "..", "..", "13", "13", "..", "..", "13", "13", "..", "..", "13", "13", "..", "..", "13", "13"},
        {"..", "..", "..", "..", "..", "13", "13", "..", "..", "13", "13", "..", "..", "13", "13", "..", "..", "13", "13"},
        {"..", "..", "..", "..", "..", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"..", "..", "..", "..", "..", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "..", "..", "..", "..", "13", "13", "13", "13", "13", "13", "13", "13", "00", "00", "13", "13", "13", "13"},
        {"..", "..", "..", "..", "13", "13", "13", "13", "13", "13", "13", "00", "00", "00", "00", "13", "13", "13", "13"},
        {"..", "..", "..", "13", "13", "13", "13", "13", "13", "13", "13", "00", "00", "00", "00", "13", "13", "13", "13"},
        {"..", "..", "13", "13", "13", "13", "13", "13", "13", "13", "13", "00", "00", "00", "00", "13", "13", "13", "13"},
        {"..", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "00", "00", "13", "13", "13", "13", "13", "13"},
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "00", "00", "13", "13", "13", "13", "13", "13"},
    };
    int entranceExit = 11;

    static readonly string[,] leftStairTopExit =
    {
        {"13", "00", "00", "13", "13", "..", "..", "..", ".."},
        {"13", "00", "00", "00", "13", "13", "..", "..", ".."},
        {"13", "00", "00", "00", "00", "13", "13", "..", ".."},
        {"13", "00", "00", "00", "00", "00", "13", "13", ".."},
        {"13", "13", "00", "00", "00", "00", "00", "13", "13"},
        {"..", "13", "13", "00", "00", "00", "00", "00", "13"},
        {"..", "..", "13", "13", "00", "00", "00", "00", "00"},
        {"..", "..", "..", "13", "13", "00", "00", "00", "00"},
        {"..", "..", "..", "..", "13", "13", "13", "13", "13"},

    };
    int leftStairTopExitEntrance = 1;

    static readonly string[,] corridor =
    {
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
    };

    static readonly string[,] corridor2 =
    {
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00"},
        {"00", "00", "13", "14", "13", "14", "13", "14", "13", "00", "00"},
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
    };

    private int roomsWidth = 13;
    static readonly string[,] roomSpikes =
    {
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "13", "00", "00", "00", "00", "00", "13", "00", "00", "00"},
        {"00", "00", "00", "13", "14", "14", "14", "14", "14", "13", "00", "00", "00"},
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
    };

    static readonly string[,] roomTreasure =
    {
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"13", "14", "14", "14", "14", "14", "14", "14", "14", "14", "14", "14", "13"},
        {"13", "14", "00", "00", "00", "00", "00", "00", "00", "00", "00", "14", "13"},
        {"13", "14", "00", "00", "00", "00", "00", "00", "00", "00", "00", "14", "13"},
        {"13", "14", "00", "00", "00", "00", "00", "00", "00", "00", "00", "14", "13"},
        {"00", "00", "00", "00", "00", "gg", "gg", "gg", "00", "00", "00", "00", "00"},
        {"00", "00", "00", "00", "00", "gg", "gg", "gg", "00", "00", "00", "00", "00"},
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
    };

    static readonly string[,] roomMagma =
    {
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"13", "11", "13", "00", "00", "00", "00", "00", "00", "00", "13", "11", "13"},
        {"13", "13", "13", "00", "00", "00", "00", "00", "00", "00", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "13", "13", "13", "13", "13", "13", "13", "00", "00", "00"},
        {"00", "00", "13", "13", "11", "11", "11", "11", "11", "13", "13", "00", "00"},
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
    };

    static readonly string[,] roomMagma2 =
    {
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"13", "14", "14", "14", "14", "14", "14", "14", "14", "14", "14", "14", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "00", "00", "00", "13", "00", "00", "00", "00", "00", "00"},
        {"00", "00", "00", "13", "11", "11", "13", "11", "11", "13", "00", "00", "00"},
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
    };
    
    static readonly string[,] roomMagmaBridge =
    {
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
        {"13", "14", "14", "00", "00", "00", "00", "00", "00", "00", "14", "14", "13"},
        {"13", "14", "00", "00", "00", "00", "00", "00", "00", "00", "00", "14", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "13", "ww", "ww", "ww", "ww", "ww", "13", "00", "00", "00"},
        {"00", "00", "13", "13", "11", "11", "11", "11", "11", "13", "13", "00", "00"},
        {"13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13", "13"},
    };

    static readonly string[,] corridorIS =
    {
        {"13", "13", "13", "13", "13", "13", "13"},
        {"00", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "00", "00", "00", "13"},
        {"13", "13", "13", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "00"},
        {"13", "00", "00", "00", "00", "00", "00"},
        {"13", "13", "13", "13", "13", "13", "13"},
    };
    private int corridorISExit = -8;

    static readonly string[,] corridorS =
    {
        {"13", "13", "13", "13", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "00"},
        {"13", "00", "00", "00", "00", "00", "00"},
        {"13", "00", "00", "00", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "13", "13", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "00", "00", "00", "13"},
        {"13", "13", "13", "13", "13", "13", "13"},
    };
    private int corridorSExit = -8;

    static readonly string[,] corridorIC =
    {
        {"13", "13", "13", "13", "13", "13", "13"},
        {"00", "00", "00", "00", "00", "00", "13"},
        {"00", "00", "00", "00", "00", "00", "13"},
        {"13", "13", "13", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "13", "13", "13", "13"},
        {"13", "00", "00", "13", "..", "..", ".."},
        {"13", "00", "00", "13", "..", "..", ".."},
        {"00", "00", "00", "13", "..", "..", ".."},
        {"00", "00", "00", "13", "..", "..", ".."},
        {"13", "13", "13", "13", "..", "..", ".."},
    };
    private int corridorICExit = -11;

    static readonly string[,] corridorC =
{
        {"13", "13", "13", "13", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "00"},
        {"13", "00", "00", "00", "00", "00", "00"},
        {"13", "00", "00", "00", "13", "13", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "13", "13", "13", "00", "00", "13"},
        {"..", "..", "..", "13", "00", "00", "13"},
        {"..", "..", "..", "13", "00", "00", "13"},
        {"..", "..", "..", "13", "00", "00", "00"},
        {"..", "..", "..", "13", "00", "00", "00"},
        {"..", "..", "..", "13", "13", "13", "13"},
    };
    private int corridorCExit = -11;

    static readonly string[,] corridorBIC =
    {
        {"13", "13", "13", "13", "13", "13", "13"},
        {"00", "00", "00", "00", "00", "00", "00"},
        {"00", "00", "00", "00", "00", "00", "00"},
        {"13", "00", "00", "00", "00", "00", "13"},
        {"13", "13", "00", "00", "00", "00", "13"},
        {"13", "13", "13", "00", "00", "13", "13"},
        {"13", "00", "00", "00", "00", "13", ".."},
        {"13", "00", "00", "00", "00", "13", ".."},
        {"13", "00", "00", "13", "13", "13", ".."},
        {"13", "00", "00", "00", "13", "13", ".."},
        {"13", "13", "00", "00", "13", "13", ".."},
        {"13", "00", "00", "00", "13", "..", ".."},
        {"00", "00", "00", "00", "13", "..", ".."},
        {"00", "00", "00", "00", "13", "..", ".."},
        {"13", "13", "13", "13", "13", "..", ".."},
    };
    private int corridorBICExit = -11;

    List<string[,]> rooms = new List<string[,]> { roomSpikes, roomTreasure, roomMagma, roomMagma2, roomMagmaBridge };
    List<string[,]> corridors = new List<string[,]> { corridor, corridor2 };

    const int minRooms = 10;
    const int maxRooms = 17;

    bool roomGold = false;

    public Dungeon(int mapWidth, int mapHeight)
    {
        int entranceWidth = entrance.GetLength(1);
        int entranceHeight = entrance.GetLength(0);

        int leftStairTopExitWidth = leftStairTopExit.GetLength(1);
        int leftStairTopExitHeight = leftStairTopExit.GetLength(0);

        int width = mapWidth;
        int height = mapHeight;
        structure = new Tile[width, height];
        int currentCol = width/2;
        int currentRow = height - entranceHeight;
        structureCenterCol = currentCol;
        structureCenterRow = currentRow;

        for (int row = entranceHeight - 1; row >= 0; row--)
        {
            for (int col = 0; col < entranceWidth; col++)
            {
                structure[col + currentCol, entranceHeight - 1 - row + currentRow] = GetTile(entrance[row, col]);
            }
        }

        currentCol += entranceExit - leftStairTopExitEntrance;
        currentRow -= leftStairTopExitHeight;
        for (int row = leftStairTopExitHeight - 1; row >= 0; row--)
        {
            for (int col = 0; col < leftStairTopExitWidth; col++)
            {
                structure[col + currentCol, leftStairTopExitHeight - 1 - row + currentRow] = GetTile(leftStairTopExit[row, col]);
            }
        }

        currentCol += leftStairTopExitWidth;

        bool isRoom = true;
        int roomsCount = 0;
        List<Agent> agents = new List<Agent>();
        agents.Add(new Agent { startCol = currentCol, startRow = currentRow, goingRight = true });
        while (agents.Count > 0)
        {
            string[,] currentStructure;
            int columnIncrease = 0;
            int rowIncrease = 0;
            if (isRoom)
            {
                roomGold = Random.Range(0, 3) < 1;
                currentStructure = rooms[Random.Range(0, rooms.Count)];
                if (agents[0].goingRight)
                {
                    columnIncrease = currentStructure.GetLength(1);
                }
                roomsCount++;
            }
            else
            {
                int random = Random.Range(0, 100);
                if (agents[0].goingRight)
                {
                    if (random > 80)
                    {
                        currentStructure = corridors[Random.Range(0,corridors.Count)];
                        columnIncrease = currentStructure.GetLength(1);
                    }
                    else if (random > 50)
                    {
                        currentStructure = corridorIS;
                        columnIncrease = currentStructure.GetLength(1);
                        currentRow += corridorISExit;
                    }
                    else if (random > 20)
                    {
                        currentStructure = corridorIC;
                        currentRow += corridorICExit;
                        agents[0].goingRight = false;
                        columnIncrease = -roomsWidth;
                    }
                    else
                    {
                        currentStructure = corridorBIC;
                        agents.Add(new Agent { startCol = currentCol + currentStructure.GetLength(1), startRow = currentRow, goingRight = true });
                        currentRow += corridorBICExit;
                        agents[0].goingRight = false;
                        columnIncrease = -roomsWidth;
                    }
                }
                else
                {
                    if (random > 75)
                    {
                        currentStructure = corridors[Random.Range(0, corridors.Count)];
                        currentCol -= currentStructure.GetLength(1);
                        columnIncrease = -roomsWidth;
                    }
                    else if (random > 50)
                    {
                        currentStructure = corridorS;
                        currentCol -= currentStructure.GetLength(1);
                        columnIncrease = -roomsWidth;
                        currentRow += corridorSExit;
                    }
                    else
                    {
                        currentStructure = corridorC;
                        currentCol -= currentStructure.GetLength(1);
                        columnIncrease = currentStructure.GetLength(1);
                        currentRow += corridorCExit;
                        agents[0].goingRight = true;
                    }
                }

            }
            int currentRoomWidth = currentStructure.GetLength(1);
            int currentRoomHeight = currentStructure.GetLength(0);

            for (int row = currentRoomHeight - 1; row >= 0; row--)
            {
                for (int col = 0; col < currentRoomWidth; col++)
                {
                    int structureCol = col + currentCol;
                    int structureRow = currentRoomHeight - 1 - row + currentRow;
                    if (structureCol < 0 || structureCol >= width || structureRow < 0 || structureRow >= height) continue;
                    if (structure[structureCol, structureRow] != null)
                    {
                        structure[structureCol, structureRow] = WorldController.instance.allTiles[Constants.BlockTypes.Air];
                    }
                    else
                    {
                        structure[structureCol, structureRow] = GetTile(currentStructure[row, col]);
                    }
                }
            }
            currentCol += columnIncrease;
            currentRow += rowIncrease;
            isRoom = !isRoom;

            if (!isRoom)
            {
                if ((roomsCount >= minRooms || agents.Count > 1) && Random.Range(0, 100) > 50)
                {
                    agents.RemoveAt(0);
                    if (agents.Count > 0)
                    {
                        currentCol = agents[0].startCol;
                        currentRow = agents[0].startRow;
                        isRoom = true;
                    }
                }
                else if(roomsCount >= maxRooms)
                {
                    agents.Clear();
                }
            }
        }
    }

    private Tile GetTile(string c)
    {
        switch (c)
        {
            case "..": return null;
            case "gg": return roomGold ? WorldController.instance.allTiles[Constants.BlockTypes.Gold] : WorldController.instance.allTiles[Constants.BlockTypes.Air];
            case "ww": return Random.Range(0,2) == 1 ? WorldController.instance.allTiles[Constants.BlockTypes.Wood] : WorldController.instance.allTiles[Constants.BlockTypes.Air];
            default: return WorldController.instance.allTiles[int.Parse(c)];
        }
    }
}
