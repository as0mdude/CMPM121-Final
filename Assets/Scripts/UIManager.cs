using UnityEngine;
using TMPro;

public class ResourceUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshPro sunText;
    [SerializeField] private TextMeshPro waterText;
    [SerializeField] private TextMeshPro growthStageText;

    private TurnManager turnManager;

    void Start()
    {
        // Find the TurnManager in the scene
        turnManager = FindObjectOfType<TurnManager>();

        if (turnManager == null)
        {
            Debug.LogError("TurnManager not found in the scene.");
        }
    }

    void Update()
    {
        // Update the UI with current resource values
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Example: Display the sun and water levels of the first grid cell
        TurnManager.GridCell firstCell = turnManager.grid[0, 0];
        sunText.text = $"Sun: {firstCell.sunLevel:F1}";
        waterText.text = $"Water: {firstCell.waterLevel:F1}";

        // Example: Display the growth stage of the first plant in the first cell
        if (firstCell.plantsInCell.Count > 0)
        {
            TurnManager.Plant firstPlant = firstCell.plantsInCell[0];
            growthStageText.text = $"Growth Stage: {firstPlant.currentStage}";
        }
        else
        {
            growthStageText.text = "Growth Stage: None";
        }
    }
}
