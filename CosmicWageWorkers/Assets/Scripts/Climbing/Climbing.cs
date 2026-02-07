using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    public float handLeanAmount = 0.5f;

    [Header("Stamina")]
    public HandStamina handStamina;
    public float obstaclePenalty = 10f;

    [Header("Lose/Fall")]
    public bool isFalling = false;
    public float fallSpeed = 10f;
    public float groundY = 0f;
    public CanvasGroup fadeCanvas;

    [Header("Falling Hand Targets")]
    public Transform leftFallTarget;   // Place in front of camera
    public Transform rightFallTarget;
    public float handFallSpeed = 3f;
    public float handFlailAmplitude = 0.3f;
    public float handFlailSpeed = 5f;

    private int currentColumn;
    private int currentRow;
    private int previousColumn;
    private int previousRow;

    private bool isMoving;
    private Transform activeHandDuringMove;

    public PlayerAudio playerAudio;

    void Start()
    {
        AudioListener.pause = false;

        currentColumn = columns / 2;
        currentRow = 0;
        transform.position = GetBodyTargetFromHands();

        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;
    }

    void Update()
    {
        // Camera tilt
        if (cameraTransform != null)
        {
            if (isFalling)
            {
                // Lock tilt when falling
                Vector3 euler = cameraTransform.rotation.eulerAngles;
                cameraTransform.rotation = Quaternion.Euler(lookAheadTilt, euler.y, euler.z);
            }
            else
            {
                float targetTilt = Input.GetKey(KeyCode.Space) ? lookAheadTilt : normalTilt;
                Vector3 euler = cameraTransform.rotation.eulerAngles;
                float tilt = Mathf.LerpAngle(euler.x, targetTilt, Time.deltaTime * tiltSpeed);
                cameraTransform.rotation = Quaternion.Euler(tilt, euler.y, euler.z);
            }
        }

        if (isMoving || isFalling) return;

        // Block climbing while holding space
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
        if (cameraTransform != null)
        {
            Vector3 targetPos;

            if (isFalling)
            {
                // Camera follows player while falling
                targetPos = transform.position + cameraOffset;
            }
            else
            {
                Vector3 midpoint = (leftHand.position + rightHand.position) * 0.5f;
                targetPos = new Vector3(
                    midpoint.x + cameraOffset.x,
                    midpoint.y + cameraOffset.y,
                    midpoint.z + cameraOffset.z
                );

                if (activeHandDuringMove != null)
                {
                    float leanZ = activeHandDuringMove == rightHand ? handLeanAmount : -handLeanAmount;
                    targetPos.z += leanZ;
                }
            }

            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPos, Time.deltaTime * cameraFollowSpeed);
        }

        if (isFalling)
            AnimateHandsWhileFalling();
        else
        {
            // Smoothly move player body toward hand midpoint
            Vector3 bodyTarget = GetBodyTargetFromHands();
            transform.position = Vector3.Lerp(transform.position, bodyTarget, Time.deltaTime * cameraFollowSpeed);
        }
    }

    void AnimateHandsWhileFalling()
    {
        if (leftFallTarget != null)
        {
            Vector3 leftPos = leftFallTarget.position;
            leftPos.y += Mathf.Sin(Time.time * handFlailSpeed) * handFlailAmplitude;
            leftHand.position = Vector3.Lerp(leftHand.position, leftPos, Time.deltaTime * handFallSpeed);
            leftHand.rotation = leftFallTarget.rotation;
        }

        if (rightFallTarget != null)
        {
            Vector3 rightPos = rightFallTarget.position;
            rightPos.y += Mathf.Sin(Time.time * handFlailSpeed + Mathf.PI) * handFlailAmplitude;
            rightHand.position = Vector3.Lerp(rightHand.position, rightPos, Time.deltaTime * handFallSpeed);
            rightHand.rotation = rightFallTarget.rotation;
        }
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

            playerAudio.PlayOneShot(playerAudio.handHitOccupied);

            currentColumn = previousColumn;
            currentRow = previousRow;

            yield return StartCoroutine(LerpHand(hand, targetPos, handStart, handReturnDuration));

            // same hand moves next
        }
        else
        {
            rightHandNext = !rightHandNext;
        }

        activeHandDuringMove = null;
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

    public void TriggerFall()
    {
        if (!isFalling)
        {
            isFalling = true;

            AudioListener.pause = true;
            playerAudio.source.ignoreListenerPause = true;
            playerAudio.source.Play();


            StartCoroutine(FallAndReload());
        }
    }

    IEnumerator FallAndReload()
    {
        while (transform.position.y > groundY)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            yield return null;
        }

        // Snap hands to final positions
        if (leftFallTarget != null)
        {
            leftHand.position = leftFallTarget.position;
            leftHand.rotation = leftFallTarget.rotation;
        }
        if (rightFallTarget != null)
        {
            rightHand.position = rightFallTarget.position;
            rightHand.rotation = rightFallTarget.rotation;
        }

        AudioListener.pause = false;
        playerAudio.source.Stop();
        playerAudio.PlayOneShot(playerAudio.hitGround);
        AudioListener.pause = true;

        // Instant black
        if (fadeCanvas != null)
            fadeCanvas.alpha = 1f;

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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