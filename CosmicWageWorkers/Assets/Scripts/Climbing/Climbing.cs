using UnityEngine;
using System.Collections;

public class Climbing : MonoBehaviour
{
    [Header("Grid Size")]
    public int columns = 5;
    public int rows = 30;

    [Header("Cell Size")]
    public float cellWidth = 1.2f;   // Z
    public float cellHeight = 1.2f;  // Y

    [Header("Grid Origin")]
    public Vector3 gridOrigin;

    [Header("Movement")]
    public float moveDuration = 0.25f;

    [Header("Body Offset")]
    public float bodyXOffset = -0.4f;

    [Header("Hands")]
    public Transform leftHand;
    public Transform rightHand;

    private int currentColumn;
    private int currentRow;

    private bool isMoving;
    private bool rightHandNext = true;

    void Start()
    {
        currentColumn = columns / 2;
        currentRow = 0;

        // Do NOT touch hand positions — they stay where you placed them
        Vector3 startHold = GetHoldPosition(currentColumn, currentRow);

        // Body starts slightly back from the shelf
        transform.position = GetBodyPosition(startHold);
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.A)) TryMove(-1, 0);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(1, 0);
        if (Input.GetKeyDown(KeyCode.W)) TryMove(0, 1);
        if (Input.GetKeyDown(KeyCode.Q)) TryMove(-1, 1);
        if (Input.GetKeyDown(KeyCode.E)) TryMove(1, 1);
    }

    void TryMove(int colDelta, int rowDelta)
    {
        int newCol = currentColumn + colDelta;
        int newRow = currentRow + rowDelta;

        if (newCol < 0 || newCol >= columns) return;
        if (newRow < 0 || newRow >= rows) return;

        currentColumn = newCol;
        currentRow = newRow;

        Vector3 holdPos = GetHoldPosition(currentColumn, currentRow);

        SnapHandToHold(holdPos);
        StartCoroutine(MoveBodyToHold(holdPos));
    }

    void SnapHandToHold(Vector3 holdPosition)
    {
        if (rightHandNext)
            rightHand.position = holdPosition;
        else
            leftHand.position = holdPosition;

        rightHandNext = !rightHandNext;
    }

    Vector3 GetHoldPosition(int col, int row)
    {
        return new Vector3(
            gridOrigin.x,
            gridOrigin.y + row * cellHeight,
            gridOrigin.z + col * cellWidth
        );
    }

    Vector3 GetBodyPosition(Vector3 holdPosition)
    {
        return new Vector3(
            holdPosition.x + bodyXOffset,
            holdPosition.y,
            holdPosition.z
        );
    }

    IEnumerator MoveBodyToHold(Vector3 holdPosition)
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 target = GetBodyPosition(holdPosition);

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
}