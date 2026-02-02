using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Climbing : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columns = 5;
    public int rows = 30;
    public float cellWidth = 1.2f;   // Z axis
    public float cellHeight = 1.2f;  // Y axis
    public Vector3 gridOrigin;

    [Header("Movement")]
    public float moveDuration = 0.25f;
    public float bodyXOffset = -0.4f; // offset from shelf to avoid clipping

    [Header("Hands")]
    public Transform leftHand;
    public Transform rightHand;
    private bool rightHandNext = true;

    [Header("Camera")]
    public ShelfCamera shelfCamera; // reference to camera for look mode

    [Header("Stamina")]
    public HandStamina handStamina;

    [Header("Lose Settings")]
    public string reloadSceneName = "MainScene";

    private int currentColumn;
    private int currentRow;
    private bool isMoving;

    void Start()
    {
        // Start middle column, bottom row
        currentColumn = columns / 2;
        currentRow = 0;

        // Hands stay where placed in scene
        transform.position = GetBodyTargetFromHands();
    }

    void Update()
    {
        if (isMoving) return;

        // Block climbing while holding space
        if (shelfCamera != null && shelfCamera.IsLookingAhead())
            return;

        // Climbing inputs
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
        {
            rightHand.position = holdPosition;
            if (handStamina != null) handStamina.lastMovedHand = rightHand;
        }
        else
        {
            leftHand.position = holdPosition;
            if (handStamina != null) handStamina.lastMovedHand = leftHand;
        }

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
            float t = Mathf.SmoothStep(0f, 1f, elapsed / moveDuration);

            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }
}