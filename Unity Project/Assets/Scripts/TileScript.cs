using UnityEngine;

public class TileScript : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Tile tile;

    public void SetTile(Tile newTile)
    {
        tile = newTile;
        spriteRenderer.color = tile.color;
        if(tile.id == Constants.BlockTypes.Water || tile.id == Constants.BlockTypes.Magma)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
        }
    }
}
