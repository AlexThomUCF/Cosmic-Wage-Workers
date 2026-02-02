using UnityEngine;
using System.Collections;

public class Climbing : MonoBehaviour
{
    [Header("Grid Size")]
    public int columns = 5;
    public int rows = 30;

    [Header("Cell Size")]
    public float cellWidth = 1.2f;   // Z axis
    public float cellHeight = 1.2f;  // Y axis

    [Header("Grid Origin (Hold 0,0)")]
    public Vector3 gridOrigin;

    [Header("Movement")]
    public float moveDuration = 0.25f;

    [Header("Body Offset")]
    public float bodyXOffset = -0.4f;

    [Header("Hands (World Space)")]
    public Transform leftHand;
    public Transform rightHand;

    private int currentColumn;
    private int currentRow;

    private bool isMoving;
    private bool rightHandNext = true;

    void Start()
    {
        // Start in middle column, bottom row
        currentColumn = columns / 2;
        currentRow = 0;

        // Hands stay EXACTLY where you placed them in the scene
        // Body moves toward their midpoint
        transform.position = GetBodyTargetFromHands();
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.A)) TryMove(-1, 0);   // Left
        if (Input.GetKeyDown(KeyCode.D)) TryMove(1, 0);    // Right
        if (Input.GetKeyDown(KeyCode.W)) TryMove(0, 1);    // Up
        if (Input.GetKeyDown(KeyCode.Q)) TryMove(-1, 1);   // Left-Up
        if (Input.GetKeyDown(KeyCode.E)) TryMove(1, 1);    // Right-Up
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
        StartCoroutine(MoveBodyToHands());
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

    Vector3 GetBodyTargetFromHands()
    {
        Vector3 midpoint = (leftHand.position + rightHand.position) * 0.5f;

        return new Vector3(
            midpoint.x + bodyXOffset,
            midpoint.y,
            midpoint.z
        );
    }

    IEnumerator MoveBodyToHands()
    {
        isMoving = true;

        Vector3 start = transform.position;
        Vector3 target = GetBodyTargetFromHands();

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