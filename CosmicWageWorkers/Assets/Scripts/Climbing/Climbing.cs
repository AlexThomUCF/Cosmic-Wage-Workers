using UnityEngine;
using System.Collections;

public class Climbing : MonoBehaviour
{
    [Header("Grid Settings")]
    public int columns = 5;
    public int rows = 25;
    public float cellWidth = 6f;
    public float cellHeight = 4.25f;
    public Vector3 gridOrigin;

    [Header("Movement")]
    public float moveDuration = 0.25f;
    public float bodyXOffset = 2.5f;
    public float handReturnDuration = 0.5f;

    [Header("Hands")]
    public Transform leftHand;
    public Transform rightHand;
    private bool rightHandNext = true;

    [Header("Camera")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new Vector3(2.5f, 0f, 0f);
    public float cameraFollowSpeed = 8f;
    public float normalTilt = -25f;
    public float lookAheadTilt = -45f;
    public float tiltSpeed = 6f;
    public float handLeanAmount = 0.5f;  // offset toward active hand
    public float obstacleShakeAmount = 0.3f;
    public float obstacleShakeDuration = 0.15f;

    [Header("Stamina")]
    public HandStamina handStamina;
    public float obstaclePenalty = 10f;

    private int currentColumn;
    private int currentRow;
    private int previousColumn;
    private int previousRow;

    private bool isMoving;
    private bool shaking = false;

    private Transform activeHandDuringMove;

    void Start()
    {
        currentColumn = columns / 2;
        currentRow = 0;
        transform.position = GetBodyTargetFromHands();
    }

    void Update()
    {
        // Camera tilt should always update
        if (cameraTransform != null)
        {
            float targetTilt = Input.GetKey(KeyCode.Space) ? lookAheadTilt : normalTilt;
            Vector3 euler = cameraTransform.rotation.eulerAngles;
            float tilt = Mathf.LerpAngle(euler.x, targetTilt, Time.deltaTime * tiltSpeed);
            cameraTransform.rotation = Quaternion.Euler(tilt, euler.y, euler.z);
        }

        if (isMoving) return;

        // Block climbing if looking ahead
        if (Input.GetKey(KeyCode.Space)) return;

        // Climbing inputs
        if (Input.GetKeyDown(KeyCode.A)) TryMove(-1, 0);
        if (Input.GetKeyDown(KeyCode.D)) TryMove(1, 0);
        if (Input.GetKeyDown(KeyCode.W)) TryMove(0, 1);
        if (Input.GetKeyDown(KeyCode.Q)) TryMove(-1, 1);
        if (Input.GetKeyDown(KeyCode.E)) TryMove(1, 1);
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Midpoint between hands
        Vector3 midpoint = (leftHand.position + rightHand.position) * 0.5f;

        // Start with the explicit offset
        Vector3 targetPos = new Vector3(
            midpoint.x + cameraOffset.x,   // X = move back/forward
            midpoint.y + cameraOffset.y,   // Y = height
            midpoint.z + cameraOffset.z    // Z = side offset
        );

        // Lean slightly toward the active hand
        if (activeHandDuringMove != null)
        {
            float leanZ = activeHandDuringMove == rightHand ? handLeanAmount : -handLeanAmount;
            targetPos.z += leanZ;
        }

        // Apply smooth camera follow
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, Time.deltaTime * cameraFollowSpeed);

        // Smoothly move player body toward hand midpoint
        Vector3 bodyTarget = GetBodyTargetFromHands();
        transform.position = Vector3.Lerp(transform.position, bodyTarget, Time.deltaTime * cameraFollowSpeed);
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

        activeHandDuringMove = rightHandNext ? rightHand : leftHand;

        if (handStamina != null)
            handStamina.lastMovedHand = activeHandDuringMove;

        StartCoroutine(MoveHandAndResolve(activeHandDuringMove, holdPos));
    }

    IEnumerator MoveHandAndResolve(Transform hand, Vector3 targetPos)
    {
        isMoving = true;

        HandObstacleDetector detector = hand.GetComponent<HandObstacleDetector>();
        Vector3 handStart = hand.position;

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

            // Trigger camera shake
            if (!shaking) StartCoroutine(CameraShake());

            // Roll grid back
            currentColumn = previousColumn;
            currentRow = previousRow;

            // Smoothly return hand
            yield return StartCoroutine(LerpHand(hand, targetPos, handStart, handReturnDuration));

            // Do NOT toggle hand; same hand goes next
        }
        else
        {
            rightHandNext = !rightHandNext;
        }

        activeHandDuringMove = null; // Stop leaning toward hand
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

    IEnumerator CameraShake()
    {
        shaking = true;
        Vector3 originalPos = cameraTransform.position;
        float elapsed = 0f;

        while (elapsed < obstacleShakeDuration)
        {
            elapsed += Time.deltaTime;
            Vector3 offset = new Vector3(
                Random.Range(-obstacleShakeAmount, obstacleShakeAmount),
                Random.Range(-obstacleShakeAmount, obstacleShakeAmount),
                Random.Range(-obstacleShakeAmount, obstacleShakeAmount)
            );
            cameraTransform.position += offset;
            yield return null;
        }

        cameraTransform.position = originalPos;
        shaking = false;
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
        return new Vector3(midpoint.x + bodyXOffset, midpoint.y, midpoint.z);
    }
}