using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    private WorldController wc;
    private GameController gc;
    private Rigidbody2D rb;

    public float speed = 10f;
    public float jumpForce = 400f;

    float horizontalMove = 0f;
    float verticalMove = 0f;
    private Vector3 velocity = Vector3.zero;
    bool jump = false;

    private Tile selectedTile;

    bool debugMode = false;

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

    void Start()
    {
        wc = WorldController.instance;
        gc = GameController.instance;
        selectedTile = wc.allTiles[1];
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        verticalMove = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            debugMode = !debugMode;
            if (debugMode) EnableDebugMode();
            else DisableDebugMode();
        }

        for(int key = 1; key < 9; key++)
        {
            if (Input.GetKeyDown("" + key)) selectedTile = wc.allTiles[key];
        }

        if (Input.GetMouseButtonDown(0) && !gc.gamePaused)
        {
            gc.DestroyTileAtMouse();
        }
        else if (Input.GetMouseButtonDown(1) && !gc.gamePaused)
        {
            gc.PlaceTileAtMouse(selectedTile);
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(horizontalMove * speed, debugMode ? verticalMove * speed : rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.4f);

        if (jump)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
    }

    private void EnableDebugMode()
    {
        rb.gravityScale = 0;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void DisableDebugMode()
    {
        rb.gravityScale = 1;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
