using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSwitch : MonoBehaviour
{
    public Sprite grass1, shrub1, tree1;
    public Sprite grass2, shrub2, tree2;
    public Sprite grass3, shrub3, tree3;

    public static List<Sprite> grassSprites;
    public static List<Sprite> shrubSprites;
    public static List<Sprite> treeSprites;

    public static List<Sprite> currentSprites;

    void Start()
    {
        // Initialize default sprites
        grassSprites = new List<Sprite> { grass1, grass2, grass3 };
        shrubSprites = new List<Sprite> { shrub1, shrub2, shrub3 };
        treeSprites = new List<Sprite> { tree1, tree2, tree3 };

        // Default to the first set
        currentSprites = grassSprites;
    }

    void Update()
    {
        // Switch sprite sets based on user input
        if (Input.GetKeyDown("1"))
        {
            UpdateGlobalSprites(0);
        }
        else if (Input.GetKeyDown("2"))
        {
            UpdateGlobalSprites(1);
        }
        else if (Input.GetKeyDown("3"))
        {
            UpdateGlobalSprites(2);
        }
    }

    public static void UpdateGlobalSprites(int index)
    {
        // Update the current sprite set for new plants
        currentSprites = new List<Sprite> { grassSprites[index], shrubSprites[index], treeSprites[index] };

        // Only affects new plants — existing plants retain their sprite set index
        Debug.Log($"Current sprite set updated to index: {index}");
    }
}
