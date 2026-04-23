using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <-- New Input System

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
    private Transform activeHandDuringMove;

    [Header("Hand Sprites")]
    public Sprite leftHandOpen;
    public Sprite leftHandClosed;
    public Sprite rightHandOpen;
    public Sprite rightHandClosed;

    private SpriteRenderer leftHandRenderer;
    private SpriteRenderer rightHandRenderer;

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

    [Header("Climb Cooldown")]
    public float climbCooldown = 0.5f;
    private float lastClimbTime = -999f;

    [Header("Lose/Fall")]
    public bool isFalling = false;
    public float fallSpeed = 10f;
    public float groundY = 0f;
    public CanvasGroup fadeCanvas;

    [Header("Falling Hand Targets")]
    public Transform leftFallTarget;
    public Transform rightFallTarget;
    public float handFallSpeed = 3f;
    public float handFlailAmplitude = 0.3f;
    public float handFlailSpeed = 5f;

    private int currentColumn;
    private int currentRow;
    private int previousColumn;
    private int previousRow;

    private bool isMoving;

    public PlayerAudio playerAudio;

    // --- Input System ---
    private PlayerControls controls;
    private bool isLookingUp = false;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.ClimbLeft.performed += ctx => TryMove(-1, 0);
        controls.Gameplay.ClimbRight.performed += ctx => TryMove(1, 0);
        controls.Gameplay.ClimbUp.performed += ctx => TryMove(0, 1);
        controls.Gameplay.ClimbUpLeft.performed += ctx => TryMove(-1, 1);
        controls.Gameplay.ClimbUpRight.performed += ctx => TryMove(1, 1);
        controls.Gameplay.LookUp.performed += ctx => isLookingUp = true;
        controls.Gameplay.LookUp.canceled += ctx => isLookingUp = false;
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        AudioListener.pause = false;

        currentColumn = columns / 2;
        currentRow = 0;
        transform.position = GetBodyTargetFromHands();

        if (fadeCanvas != null)
            fadeCanvas.alpha = 0f;

        leftHandRenderer = leftHand.GetComponent<SpriteRenderer>();
        rightHandRenderer = rightHand.GetComponent<SpriteRenderer>();

        UpdateHandSprites();
    }

    void Update()
    {
        // Camera tilt
        if (cameraTransform != null)
        {
            if (isFalling)
            {
                Vector3 euler = cameraTransform.rotation.eulerAngles;
                cameraTransform.rotation = Quaternion.Euler(lookAheadTilt, euler.y, euler.z);
            }
            else
            {
                float targetTilt = isLookingUp ? lookAheadTilt : normalTilt;
                Vector3 euler = cameraTransform.rotation.eulerAngles;
                float tilt = Mathf.LerpAngle(euler.x, targetTilt, Time.deltaTime * tiltSpeed);
                cameraTransform.rotation = Quaternion.Euler(tilt, euler.y, euler.z);
            }
        }
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            Vector3 targetPos;

            if (isFalling)
                targetPos = transform.position + cameraOffset;
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
        // Block input if already moving OR falling
        if (isMoving || isFalling)
            return;

        // Cooldown check
        if (Time.time < lastClimbTime + climbCooldown)
            return;

        int newCol = currentColumn + colDelta;
        int newRow = currentRow + rowDelta;

        if (newCol < 0 || newCol >= columns) return;
        if (newRow < 0 || newRow >= rows) return;

        previousColumn = currentColumn;
        previousRow = currentRow;

        currentColumn = newCol;
        currentRow = newRow;

        lastClimbTime = Time.time; // Start cooldown immediately

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
        }
        else
        {
            rightHandNext = !rightHandNext;
        }

        activeHandDuringMove = null;
        isMoving = false;

        UpdateHandSprites();
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

    void UpdateHandSprites()
    {
        if (rightHandNext)
        {
            leftHandRenderer.sprite = leftHandClosed;
            rightHandRenderer.sprite = rightHandOpen;
        }
        else
        {
            leftHandRenderer.sprite = leftHandOpen;
            rightHandRenderer.sprite = rightHandClosed;
        }
    }
}