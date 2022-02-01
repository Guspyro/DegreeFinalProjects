using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;

public class WorldController : MonoBehaviour
{
    public static WorldController instance;

    public World world;
    private GameController gc;
    private PlayerController pc;

    public GameObject worldContainer;
    public GameObject baseTile;

    [HideInInspector]
    public Tile[] allTiles;

    private string worldName = "";
    private string worldTextSeed = "";
    private int worldType = Constants.GenerationTypes.Final;
    private int[] worldSize = Constants.WorldSizes.Small;

    public GameObject[,] playableWorld;
    public int loadedRadius = 50;
    private int lastPlayerX = 0;
    private int lastPlayerY = 0;

    [HideInInspector]
    public bool gameStarted = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        allTiles = Resources.LoadAll<Tile>("Tiles");
        System.Array.Sort(allTiles,delegate (Tile first, Tile second) { return first.id.CompareTo(second.id); });
    }

    private void Update()
    {
        if (gameStarted)
        {
            if ((int)pc.transform.position.x > lastPlayerX) LoadColumn(true);
            else if ((int)pc.transform.position.x < lastPlayerX) LoadColumn(false);

            if ((int)pc.transform.position.y > lastPlayerY) LoadRow(true);
            else if ((int)pc.transform.position.y < lastPlayerY) LoadRow(false);

            lastPlayerX = (int)pc.transform.position.x;
            lastPlayerY = (int)pc.transform.position.y;
        }
    }

    public void CreateWorld()
    {
        int seed = GenerateSeed(worldTextSeed);

        switch (worldType)
        {
            case Constants.GenerationTypes.Final:
                world = new FinalWorld(worldName, seed, worldSize);
                break;
            case Constants.GenerationTypes.PerlinNoise:
                world = new PerlinNoiseWorld(worldName, seed, worldSize);
                break;

            case Constants.GenerationTypes.CelularAutomata:
                world = new CelularAutomataWorld(worldName, seed, worldSize);
                break;
            case Constants.GenerationTypes.Templates:
                world = new TemplatesWorld(worldName, seed, worldSize);
                break;
            default:
                world = new RandomWorld(worldName, seed, worldSize);
                break;
        }
        WorldCreated();
    }

    public void LoadWorld(string file)
    {
        world = WorldIO.LoadWorld(file);
        WorldCreated();
    }

    public void LoadWorld(World newWorld)
    {
        world = newWorld;
        WorldCreated();
    }

    private void WorldCreated()
    {
        SceneManager.LoadScene("Game"); 
        //MiniMapController.instance.RenderMiniMap(world); //Testing Proupose
    }

    public bool SetWorldAttributes(string name, string textSeed, int type, int[] size)
    {
        if(Regex.IsMatch(name, Constants.WorldNamePattern))
        {
            AlertController.instance.CreateAlert(Constants.ErrorMessages.InvalidName);
            return false;
        }
        else if(name == "")
        {
            AlertController.instance.CreateAlert(Constants.ErrorMessages.EmptyName);
            return false;
        }
        else if (WorldIO.CheckIfWorldExists(name))
        {
            AlertController.instance.CreateAlert(Constants.ErrorMessages.WorldAlreadyExists);
            return false;
        }
        worldName = name;
        worldTextSeed = textSeed;
        worldType = type;
        worldSize = size;

        return true;
    }

    public void SaveWorld()
    {
        WorldIO.SaveWorld(world);
    }

    public void GameReady()
    {
        gc = GameController.instance;
        pc = PlayerController.instance;
        if (worldContainer == null) worldContainer = GameObject.FindGameObjectWithTag("WorldContainer");
        if(world == null) world = new FinalWorld("", 0, Constants.WorldSizes.Small); //Testing Proupose

        MiniMapController.instance.RenderMiniMap(world);

        playableWorld = new GameObject[world.width, world.height];
        IEnumerator coroutine = CreatePlayableWorld();
        StartCoroutine(coroutine);
    }

    IEnumerator CreatePlayableWorld()
    {
        for (int row = (int)world.spawnPoint.y - loadedRadius; row < world.spawnPoint.y + loadedRadius; row++)
        {
            if (row < 0 || row >= world.height) continue;
            for (int col = (int)world.spawnPoint.x - loadedRadius; col < world.spawnPoint.x + loadedRadius; col++)
            {
                if (col < 0 || col >= world.width || world.map[col, row].id == 0) continue;
                playableWorld[col, row] = Instantiate(baseTile, new Vector2(col, row), Quaternion.identity);
                playableWorld[col, row].transform.parent = worldContainer.transform;
                playableWorld[col, row].GetComponent<TileScript>().SetTile(world.map[col, row]);
                if (col == loadedRadius) yield return null;
            }
        }
        PlayableWorldCreated();
    }

    void LoadColumn(bool isRight)
    {
        int newCol = lastPlayerX;
        newCol += isRight ? loadedRadius : -loadedRadius;
        int currentRow = lastPlayerY;
        if (!(newCol < 0 || newCol >= world.width))
        {
            for (int row = currentRow - loadedRadius; row < currentRow + loadedRadius + 1; row++)
            {
                if (row < 0 || row >= world.height || world.map[newCol, row].id == 0) continue;
                if (playableWorld[newCol, row] == null)
                {
                    playableWorld[newCol, row] = Instantiate(baseTile, new Vector2(newCol, row), Quaternion.identity);
                    playableWorld[newCol, row].transform.parent = worldContainer.transform;
                    playableWorld[newCol, row].GetComponent<TileScript>().SetTile(world.map[newCol, row]);
                }
                else
                {
                    playableWorld[newCol, row].SetActive(true);
                }
            }
        }

        int removeCol = lastPlayerX;
        removeCol += isRight ? -loadedRadius : loadedRadius;
        if (removeCol < 0 || removeCol >= world.width) return;
        for (int row = currentRow - loadedRadius - 1; row < currentRow + loadedRadius + 1; row++)
        {
            if (row < 0 || row >= world.height) continue;
            if (playableWorld[removeCol, row] != null)
            {
                playableWorld[removeCol, row].SetActive(false);
            }

        }
    }

    void LoadRow(bool isUp)
    {
        int newRow = lastPlayerY;
        newRow += isUp ? loadedRadius : -loadedRadius;
        int currentCol = lastPlayerX;
        if (!(newRow < 0 || newRow >= world.height))
        {
            for (int col = currentCol - loadedRadius + 1; col < currentCol + loadedRadius; col++)
            {
                if (col < 0 || col >= world.width || world.map[col, newRow].id == 0) continue;
                if (playableWorld[col, newRow] == null)
                {
                    playableWorld[col, newRow] = Instantiate(baseTile, new Vector2(col, newRow), Quaternion.identity);
                    playableWorld[col, newRow].transform.parent = worldContainer.transform;
                    playableWorld[col, newRow].GetComponent<TileScript>().SetTile(world.map[col, newRow]);
                }
                else
                {
                    playableWorld[col, newRow].SetActive(true);
                }
            }
        }
        int removeRow = lastPlayerY;
        removeRow += isUp ? -loadedRadius : loadedRadius;
        if (removeRow < 0 || removeRow >= world.height) return;
        for (int col = currentCol - loadedRadius - 1; col < currentCol + loadedRadius + 1; col++)
        {
            if (col < 0 || col >= world.width) continue;
            if (playableWorld[col, removeRow] != null)
            {
                playableWorld[col, removeRow].SetActive(false);
            }

        }
    }

    public void PlayableWorldCreated()
    {
        gc.GameStart();
        gameStarted = true;
        lastPlayerX = (int)pc.transform.position.x;
        lastPlayerY = (int)pc.transform.position.y;
    }

    public void DestroyTileAt(int x, int y)
    {
        int row = y;
        int col = x;
        if (row < 0 || row >= world.height || col < 0 || col >= world.width) return;

        if (world.map[col, row].id != 0)
        {
            world.map[col, row] = allTiles[0];
            Destroy(playableWorld[col, row]);
        }
    }

    public void PlaceTileAt(int x, int y, Tile tile)
    {
        int row = y;
        int col = x;
        if (row < 0 || row >= world.height || col < 0 || col >= world.width) return;

        if (world.map[col, row].id == 0)
        {
            world.map[col, row] = tile;
            playableWorld[col, row] = Instantiate(baseTile, new Vector2(col, row), Quaternion.identity);
            playableWorld[col, row].transform.parent = worldContainer.transform;
            playableWorld[col, row].GetComponent<TileScript>().SetTile(world.map[col, row]);
        }

    }

    private int GenerateSeed(string textSeed)
    {
        return textSeed == "" ? Random.Range(int.MinValue, int.MaxValue) : textSeed.GetHashCode();
    }
}
