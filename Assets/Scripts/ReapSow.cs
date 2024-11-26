using UnityEngine;


public class ReapSow : MonoBehaviour
{
    public TurnManager turnManager;
    public Sprite grassSprite;
    public Sprite shrubSprite;
    public Sprite treeSprite;
    public Vector2 gridSize = new Vector2(1f, 1f);
    public float placementRange = 1f;

    public int maxPlantsPerTurn = 3;

    public int currentPlantsThisTurn = 0;

    public int currentTurn= 0;

    public int sowedPlants = 0;

    void Update()
    {
        // Left mouse click for planting/removing
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
            if (sowedPlants == 2){
            Debug.Log("Game is finished, total plants sowed: " + sowedPlants);
        }
        }

        // 'O' key press to advance turn
        if (Input.GetKeyDown(KeyCode.O))
        {
            AdvanceTurn();
        }

        
    }

    void AdvanceTurn()
    {
        if (turnManager != null)
        {
            turnManager.AdvanceTurn();
            UpdatePlantSprites();
            Debug.Log("Turn Advanced! Current turn: " + currentTurn);
            currentTurn++;
            currentPlantsThisTurn = 0;

        }
        else
        {
            Debug.LogWarning("Turn Manager is not assigned!");
        }
    }

    void UpdatePlantSprites()
    {
        // Find all planted objects and update their sprites based on the TurnManager's plant stage
        GameObject[] plantedObjects = GameObject.FindGameObjectsWithTag("Planted");
        
        foreach (GameObject plantObject in plantedObjects)
        {
            // Extract coordinates from the object name
            string[] coords = plantObject.name.Split('_');
            if (coords.Length >= 3)
            {
                int x = int.Parse(coords[1]);
                int y = int.Parse(coords[2]);

                // Find the corresponding plant in the TurnManager's grid
                TurnManager.Plant gridPlant = FindPlantAtCoordinate(x, y);
                
                if (gridPlant != null)
                {
                    // Update sprite based on growth stage
                    SpriteRenderer spriteRenderer = plantObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sprite = GetSpriteForGrowthStage(gridPlant.currentStage);
                    }
                }
            }
        }
    }

    TurnManager.Plant FindPlantAtCoordinate(int x, int y)
    {
        // Safely find plant at the specified coordinates
        if (x >= 0 && x < turnManager.gridWidth && y >= 0 && y < turnManager.gridHeight)
        {
            TurnManager.GridCell cell = turnManager.grid[x, y];
            return cell.plantsInCell.Count > 0 ? cell.plantsInCell[0] : null;
        }
        return null;
    }

    void HandleMouseClick()
{
    // Get the world position of the mouse click
    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 mousePosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);
    
    // Snap the position to the nearest grid point
    float snappedX = Mathf.Round(mouseWorldPosition.x / gridSize.x);
    float snappedY = Mathf.Round(mouseWorldPosition.y / gridSize.y);
    Vector2 snappedPosition = new Vector2(snappedX, snappedY);
    
    // Check if the snapped position is within range
    if (Vector2.Distance(snappedPosition, transform.position) <= placementRange)
    {
        // Check if there's already a plant in this cell
        RaycastHit2D hit = Physics2D.Raycast(snappedPosition, Vector2.zero);
        
        if (hit.collider != null)
        {
            // Correct type casting using (int)
            TurnManager.Plant validPlant = FindPlantAtCoordinate((int)snappedX, (int)snappedY);
           
            if(validPlant.currentStage == TurnManager.PlantGrowthStage.Tree)
            {
                Destroy(hit.collider.gameObject);
                sowedPlants++;
            }
            else
            {
                Debug.Log("You can only sow final stage plants (trees).");
            }
        }
        else if(currentPlantsThisTurn == maxPlantsPerTurn)
        {
            Debug.Log("Already planted max plants this turn.");
        }
        else
        {
            // Plant a new plant (always starts as grass)
            PlantNewPlant((int)snappedX, (int)snappedY);
            currentPlantsThisTurn += 1;
        }
    }
    else
    {
        Debug.Log("Cannot place object here. Too far from the character.");
    }
}
    void PlantNewPlant(int x, int y)
    {
        // Create a new GameObject for the sprite
        GameObject newObject = new GameObject($"Plant_{x}_{y}");
        
        // Add a SpriteRenderer and assign the sprite (starts as grass)
        SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = grassSprite;

        // Add a Collider2D for click detection
        newObject.AddComponent<BoxCollider2D>();

        // Tag the object as "Planted"
        newObject.tag = "Planted";

        // Set the position of the new object
        newObject.transform.position = new Vector2(x, y);

        // Add plant to turn manager's grid (starts as grass stage)
        turnManager.PlantInCell(x, y, TurnManager.PlantGrowthStage.Grass);
    }

    Sprite GetSpriteForGrowthStage(TurnManager.PlantGrowthStage stage)
    {
        switch (stage)
        {
            case TurnManager.PlantGrowthStage.Grass:
                return grassSprite;
            case TurnManager.PlantGrowthStage.Shrub:
                return shrubSprite;
            case TurnManager.PlantGrowthStage.Tree:
                return treeSprite;
            default:
                return grassSprite;
        }
    }
}