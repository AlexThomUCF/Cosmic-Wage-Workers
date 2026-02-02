using UnityEngine;

public class ShelfClimber : MonoBehaviour
{
    [Header("Grid Size")]
    public int columns = 5;
    public int rows = 30;

    [Header("Cell Size")]
    public float cellWidth = 1.2f;   // Z spacing
    public float cellHeight = 1.2f;  // Y spacing

    [Header("Grid Origin")]
    public Vector3 gridOrigin;

    private int currentColumn;
    private int currentRow;

    private float fixedX;

    void Start()
    {
        currentColumn = columns / 2; // middle (2)
        currentRow = 0;

        fixedX = transform.position.x;

        UpdateWorldPosition();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            TryMove(-1, 0);      // Left

        if (Input.GetKeyDown(KeyCode.D))
            TryMove(1, 0);       // Right

        if (Input.GetKeyDown(KeyCode.W))
            TryMove(0, 1);       // Up

        if (Input.GetKeyDown(KeyCode.Q))
            TryMove(-1, 1);      // Left-Up

        if (Input.GetKeyDown(KeyCode.E))
            TryMove(1, 1);       // Right-Up
    }

    void TryMove(int colDelta, int rowDelta)
    {
        int newCol = currentColumn + colDelta;
        int newRow = currentRow + rowDelta;

        // Grid bounds
        if (newCol < 0 || newCol >= columns)
            return;

        if (newRow < 0 || newRow >= rows)
            return;

        currentColumn = newCol;
        currentRow = newRow;

        UpdateWorldPosition();
    }

    void UpdateWorldPosition()
    {
        Vector3 newPos = new Vector3(
            fixedX,
            gridOrigin.y + currentRow * cellHeight,
            gridOrigin.z + currentColumn * cellWidth
        );

        transform.position = newPos;
    }
}