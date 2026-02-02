using UnityEngine;
using System.Collections;

public class Climbing : MonoBehaviour
{
    [Header("Grid Size")]
    public int columns = 5;
    public int rows = 30;

    [Header("Cell Size")]
    public float cellWidth = 1.2f;   // Z spacing
    public float cellHeight = 1.2f;  // Y spacing

    [Header("Grid Origin")]
    public Vector3 gridOrigin;

    [Header("Movement")]
    public float moveDuration = 0.25f;

    private int currentColumn;
    private int currentRow;

    private float fixedX;
    private bool isMoving;

    void Start()
    {
        currentColumn = columns / 2;
        currentRow = 0;

        fixedX = transform.position.x;

        transform.position = GetWorldPosition(currentColumn, currentRow);
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.A))
            TryMove(-1, 0);

        if (Input.GetKeyDown(KeyCode.D))
            TryMove(1, 0);

        if (Input.GetKeyDown(KeyCode.W))
            TryMove(0, 1);

        if (Input.GetKeyDown(KeyCode.Q))
            TryMove(-1, 1);

        if (Input.GetKeyDown(KeyCode.E))
            TryMove(1, 1);
    }

    void TryMove(int colDelta, int rowDelta)
    {
        int newCol = currentColumn + colDelta;
        int newRow = currentRow + rowDelta;

        if (newCol < 0 || newCol >= columns) return;
        if (newRow < 0 || newRow >= rows) return;

        currentColumn = newCol;
        currentRow = newRow;

        Vector3 targetPos = GetWorldPosition(currentColumn, currentRow);
        StartCoroutine(MoveToPosition(targetPos));
    }

    Vector3 GetWorldPosition(int col, int row)
    {
        return new Vector3(
            fixedX,
            gridOrigin.y + row * cellHeight,
            gridOrigin.z + col * cellWidth
        );
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;

        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;

            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
}
