using UnityEngine;
using System.Collections.Generic;


public class TurnManager : MonoBehaviour
{
    // Enum for plant growth stages
    public enum PlantGrowthStage
    {
        Grass,
        Shrub,
        Tree
    }

    [System.Serializable]
    public class GridCell
    {
        public float sunLevel;       
        public float waterLevel;     
        public float maxWaterStorage = 5f;  
        public List<Plant> plantsInCell = new List<Plant>();
    }

    [System.Serializable]
    public class Plant
    {
        public PlantGrowthStage currentStage;
        public int growthTurns; // Tracks turns in current stage
        public Vector2 position;
        public int turnsSinceLastGrowth; // Tracks unique growth rate for cell
        public int spriteSetIndex; // Index to determine which sprite set to use (0, 1, or 2)
    }

    public int gridWidth = 10;
    public int gridHeight = 5;
    public GridCell[,] grid;

    // Growth requirements
    public int turnsToAdvanceStage = 3;
    public float minSunRequirement = 5f;
    public float minWaterRequirement = 2f;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new GridCell[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y] = new GridCell();
            }
        }
    }

    public void AdvanceTurn()
    {
        // Generate random resource levels
        GenerateResourceLevels();

        // Process plant growth
        ProcessPlantGrowth();

        // Optional: Check game completion
        CheckGameCompletion();
    }

    void GenerateResourceLevels()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Randomize sun and water for each cell
                grid[x, y].sunLevel = Random.Range(0f, 10f);
                
                // Water can accumulate with a max storage
                float newWater = Random.Range(0f, 2f);
                grid[x, y].waterLevel = Mathf.Min(
                    grid[x, y].waterLevel + newWater, 
                    grid[x, y].maxWaterStorage
                );
            }
        }
    }

    void ProcessPlantGrowth()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GridCell currentCell = grid[x, y];

                // Process each plant in the cell
                foreach (Plant plant in currentCell.plantsInCell)
                {
                    // Increment turns since last growth
                    plant.turnsSinceLastGrowth++;

                    // Check if plant can grow based on resources
                    if (CanPlantGrow(currentCell, plant))
                    {
                        TryAdvancePlantStage(plant, currentCell);
                    }
                }
            }
        }
    }

    bool CanPlantGrow(GridCell cell, Plant plant)
    {
        // Growth conditions based on sun, water, and cell-specific variations
        bool hasSufficientSun = cell.sunLevel >= minSunRequirement;
        bool hasSufficientWater = cell.waterLevel >= minWaterRequirement;
        
        // Additional randomness in growth rate
        bool readyToGrow = plant.turnsSinceLastGrowth >= (turnsToAdvanceStage + Random.Range(-1, 2));

        return hasSufficientSun && hasSufficientWater && readyToGrow;
    }

    void TryAdvancePlantStage(Plant plant, GridCell cell)
    {
        // Advance to next growth stage
        switch (plant.currentStage)
        {
            case PlantGrowthStage.Grass:
                plant.currentStage = PlantGrowthStage.Shrub;
                break;
            case PlantGrowthStage.Shrub:
                plant.currentStage = PlantGrowthStage.Tree;
                break;
            case PlantGrowthStage.Tree:
                // Reached maximum stage
                break;
        }

        // Reset growth tracking
        plant.turnsSinceLastGrowth = 0;

        // Consume some resources
        cell.sunLevel = Mathf.Max(0, cell.sunLevel - 2f);
        cell.waterLevel = Mathf.Max(0, cell.waterLevel - 1f);
    }

    void CheckGameCompletion()
    {
        int treePlantCount = 0;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                foreach (Plant plant in grid[x, y].plantsInCell)
                {
                    if (plant.currentStage == PlantGrowthStage.Tree)
                    {
                        treePlantCount++;
                    }
                }
            }
        }

        // Example completion condition
        if (treePlantCount >= 10)
        {
            Debug.Log("Game Completed! Reached required number of tree-stage plants.");
        }
    }

    public void PlantInCell(int x, int y, PlantGrowthStage stage = PlantGrowthStage.Grass, int spriteSetIndex = 0)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            Plant newPlant = new Plant
            {
                currentStage = stage,
                growthTurns = 0,
                position = new Vector2(x, y),
                turnsSinceLastGrowth = 0,
                spriteSetIndex = spriteSetIndex // Assign sprite set here
            };

            grid[x, y].plantsInCell.Add(newPlant);
        }
    }
}