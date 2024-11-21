using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust speed as needed
    public Vector2 gridSize = new Vector2(1f, 1f);

    private Vector2 targetPosition;
    private int currentX = 0;
    private int currentY = 0;

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        HandleInput();
        MoveToTarget();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {   
            if(currentY<4)
            {
                targetPosition += Vector2.up * gridSize.y;
                currentY += 1; // Increment Y for upward movement
                printCoordinates();
            }
           
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if(currentY>0)
            {
                targetPosition += Vector2.down * gridSize.y;
                currentY -= 1; // Decrement Y for downward movement
                printCoordinates();
            }
            
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(currentX>0)
            {
                 targetPosition += Vector2.left * gridSize.x;
                currentX -= 1; // Decrement X for left movement
                printCoordinates();
            }
           
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(currentX<9)
            {
                 targetPosition += Vector2.right * gridSize.x;
                currentX += 1; // Increment X for right movement
                printCoordinates();
            }
           
        }
    }

    void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void printCoordinates()
    {
        Debug.Log(currentX + ", " + currentY);
    }
}
