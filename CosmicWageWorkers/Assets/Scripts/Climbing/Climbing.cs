using UnityEngine;
using System.Collections;

public class Climbing : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columns = 5;
    public int rows = 30;
    public float cellWidth = 1.2f;
    public float cellHeight = 1.2f;
    public Vector3 gridOrigin;

    [Header("Movement")]
    public float moveDuration = 0.25f;
    public float bodyXOffset = -0.4f;
    public float handReturnDuration = 0.2f;

    [Header("Hands")]
    public Transform leftHand;
    public Transform rightHand;
    private bool rightHandNext = true;

    [Header("Camera")]
    public ShelfCamera shelfCamera;

    [Header("Stamina")]
    public HandStamina handStamina;
    public float obstaclePenalty = 10f;

    private int currentColumn;
    private int currentRow;
    private int previousColumn;
    private int previousRow;

    private bool isMoving;

    void Start()
    {
        currentColumn = columns / 2;
        currentRow = 0;
        transform.position = GetBodyTargetFromHands();
    }

    void Update()
    {
        if (isMoving) return;

        if (shelfCamera != null && shelfCamera.IsLookingAhead())
            return;

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

        previousColumn = currentColumn;
        previousRow = currentRow;

        currentColumn = newCol;
        currentRow = newRow;

        Vector3 holdPos = GetHoldPosition(currentColumn, currentRow);

        Transform activeHand = rightHandNext ? rightHand : leftHand;

        if (handStamina != null)
            handStamina.lastMovedHand = activeHand;

        StartCoroutine(MoveHandAndResolve(activeHand, holdPos));
    }

    IEnumerator MoveHandAndResolve(Transform hand, Vector3 targetPos)
    {
        isMoving = true;

        HandObstacleDetector detector = hand.GetComponent<HandObstacleDetector>();
        Vector3 handStart = hand.position;

        // Move hand to target
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / moveDuration);
            hand.position = Vector3.Lerp(handStart, targetPos, t);
            yield return null;
        }

        hand.position = targetPos;

        bool hitObstacle = detector != null && detector.CheckForObstacle();

        if (hitObstacle)
        {
            if (handStamina != null)
                handStamina.DamageHand(hand, obstaclePenalty);

            if (detector != null)
                StartCoroutine(detector.FlashRed());

            // Roll grid position back
            currentColumn = previousColumn;
            currentRow = previousRow;

            // Smoothly return hand
            yield return StartCoroutine(LerpHand(hand, targetPos, handStart, handReturnDuration));
        }

        else
        {
            rightHandNext = !rightHandNext;
        }

        yield return StartCoroutine(MoveBodyToHands());
        isMoving = false;
    }

    IEnumerator LerpHand(Transform hand, Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            hand.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
        hand.position = to;
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
    }
}