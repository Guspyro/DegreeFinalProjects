using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public PlayerController pc;
    private WorldController wc;

    public bool gamePaused = false;
    public GameObject pauseMenu;

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
    }

    public void Start()
    {
        wc = WorldController.instance;
        wc.GameReady();
        MiniMapController.instance.GameReady();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GameStart()
    {
        pc.enabled = true;
        pc.transform.position = wc.world.spawnPoint;
    }

    public void DestroyTileAtMouse()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        wc.DestroyTileAt(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y));
    }

    public void PlaceTileAtMouse(Tile tile)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        wc.PlaceTileAt(Mathf.RoundToInt(point.x), Mathf.RoundToInt(point.y), tile);
    }


    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

}
