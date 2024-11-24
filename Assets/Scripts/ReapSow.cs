using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReapSow : MonoBehaviour
{
    //ReapSow Function from ChatGPT
    public Sprite objectSprite; // Assign the sprite to plant
    public Vector2 gridSize = new Vector2(1f, 1f); // Grid size for snapping
    public float placementRange = 1f; // Maximum distance to allow planting

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }
    }

    void HandleMouseClick()
    {
        // Get the world position of the mouse click
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePosition2D = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

        // Check if an object is clicked
        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

        if (hit.collider != null)
        {
            // If clicked object is a planted sprite, remove it
            if (hit.collider.CompareTag("Planted"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            // Snap the position to the nearest grid point
            float snappedX = Mathf.Round(mouseWorldPosition.x / gridSize.x) * gridSize.x;
            float snappedY = Mathf.Round(mouseWorldPosition.y / gridSize.y) * gridSize.y;
            Vector2 snappedPosition = new Vector2(snappedX, snappedY);

            // Check if the snapped position is within range
            if (Vector2.Distance(snappedPosition, transform.position) <= placementRange)
            {
                // Create a new GameObject for the sprite
                GameObject newObject = new GameObject("PlantedSprite");

                // Add a SpriteRenderer and assign the sprite
                SpriteRenderer spriteRenderer = newObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = objectSprite;

                // Add a Collider2D for click detection
                newObject.AddComponent<BoxCollider2D>();

                // Tag the object as "Planted"
                newObject.tag = "Planted";

                // Set the position of the new object
                newObject.transform.position = snappedPosition;
            }
            else
            {
                Debug.Log("Cannot place object here. Too far from the character.");
            }
        }
    }
}







