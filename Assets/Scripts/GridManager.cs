using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab; // Assign a tile prefab with a SpriteRenderer in Unity
    public int rows = 5;
    public int cols = 5;
    public float tileSize = 1.0f;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject tile = Instantiate(tilePrefab, transform);
                tile.transform.position = new Vector2(x * tileSize, y * tileSize);
                tile.name = $"Tile {x},{y}";
            }
        }
    }
}