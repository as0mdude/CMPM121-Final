using UnityEngine;
using System.Collections.Generic;


public class ReapSow : MonoBehaviour
{
    public TurnManager turnManager;
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
    GameObject[] plantedObjects = GameObject.FindGameObjectsWithTag("Planted");
    
    foreach (GameObject plantObject in plantedObjects)
    {
        string[] coords = plantObject.name.Split('_');
        if (coords.Length >= 3)
        {
            int x = int.Parse(coords[1]);
            int y = int.Parse(coords[2]);
            TurnManager.Plant gridPlant = FindPlantAtCoordinate(x, y);
            
            if (gridPlant != null)
            {
                SpriteRenderer spriteRenderer = plantObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = GetSpriteForGrowthStage(gridPlant.currentStage, gridPlant.spriteSetIndex);
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
        GameObject newObject = new GameObject($"Plant_{x}_{y}");
        SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = PlantSwitch.currentSprites[0]; // Always starts as grass

        newObject.AddComponent<BoxCollider2D>();
        newObject.tag = "Planted";
        newObject.transform.position = new Vector2(x, y);

        // Determine the index of the current sprite set
        int spriteSetIndex = PlantSwitch.grassSprites.IndexOf(PlantSwitch.currentSprites[0]);

        // Add plant to turn manager's grid
        turnManager.PlantInCell(x, y, TurnManager.PlantGrowthStage.Grass, spriteSetIndex);
    }

    Sprite GetSpriteForGrowthStage(TurnManager.PlantGrowthStage stage, int spriteSetIndex)
    {
        switch (stage)
        {
            case TurnManager.PlantGrowthStage.Grass:
                return PlantSwitch.grassSprites[spriteSetIndex];
            case TurnManager.PlantGrowthStage.Shrub:
                return PlantSwitch.shrubSprites[spriteSetIndex];
            case TurnManager.PlantGrowthStage.Tree:
                return PlantSwitch.treeSprites[spriteSetIndex];
            default:
                return PlantSwitch.grassSprites[spriteSetIndex];
        }
    }
}