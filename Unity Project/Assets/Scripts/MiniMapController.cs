using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    public static MiniMapController instance;
    public Image miniMapRenderer;
    public Image fullMapRenderer;
    public GameObject miniMapContainer;
    public GameObject fullMapContainer;
    private ScrollRect scroll;

    private Vector2 fullmapSize = Vector2.zero;

    private bool fullScreen = false;
    private float zoom = 1.0f;
    private readonly float maxZoom = 5.0f;
    private readonly float minZoom = 0.5f;

    private bool gameStarted = false;

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

    public void GameReady()
    {
        scroll = fullMapContainer.GetComponent<ScrollRect>();
        gameStarted = true;
    }

    private void Update()
    {
        if (gameStarted && Input.GetKeyDown(KeyCode.M))
        {
            fullScreen = !fullScreen;
            if (fullScreen) OpenFullScreen();
            else CloseFullScreen();
        }

        if (fullScreen)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f && zoom < maxZoom)
            {
                zoom += 0.1f;
                UpdateZoom();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f && zoom > minZoom)
            {
                zoom -= 0.1f;
                UpdateZoom();
            }
        }
    }

    private void OpenFullScreen()
    {
        fullMapContainer.SetActive(true);
        zoom = 1f;
        UpdateZoom();
    }

    private void CloseFullScreen()
    {
        fullMapContainer.SetActive(false);
    }

    private void UpdateZoom()
    {
        scroll.content.sizeDelta = new Vector2(fullmapSize.x * zoom, fullmapSize.y * zoom);
    }

    public void RenderMiniMap(World world)
    {
        Texture2D texture = new Texture2D(world.width, world.height);
        for (int row = 0; row < world.height; row++)
        {
            for (int col = 0; col < world.width; col++)
            {
                texture.SetPixel(col, row, world.map[col, row].color);
            }
        }
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        miniMapRenderer.sprite = newSprite;
        if (fullMapRenderer != null) fullMapRenderer.sprite = newSprite;
        fullmapSize = new Vector2(texture.width*2, texture.height*2);
        miniMapContainer.SetActive(true);
    }

    public void RenderPerlinNoise(PerlinNoiseWorld world)
    {
        Texture2D texture = new Texture2D(world.width, world.height);
        for (int row = 0; row < world.height; row++)
        {
            for (int col = 0; col < world.width; col++)
            {
                float value = world.perlinValues[row, col];
                texture.SetPixel(col, row, new Color(value, value, value));
            }
        }
        texture.Apply();
        texture.filterMode = FilterMode.Point;
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        miniMapRenderer.sprite = newSprite;
        if (fullMapRenderer != null) fullMapRenderer.sprite = newSprite;
        fullmapSize = new Vector2(texture.width*2, texture.height*2);
        miniMapContainer.SetActive(true);
    }
}
