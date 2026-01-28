using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    private bool movingToB = true;

    [Header("Rotation (Optional)")]
    public Vector3 rotationSpeed = Vector3.zero;

    [Header("Randomization")]
    [Tooltip("Start each platform at a random height between A and B, and random direction.")]
    public bool randomizeStartPhase = true;

    [Tooltip("Add a random wait whenever the platform reaches the top or bottom.")]
    public bool useRandomWaitAtEnds = true;
    public Vector2 waitTimeRange = new Vector2(0.2f, 1.5f);

    // internal timing
    private bool isWaiting = false;
    private float waitTimer = 0f;

    // Movement delta tracking
    private Vector3 lastPosition;
    public Vector3 PlatformDelta { get; private set; }

    // Rotation delta tracking
    private Quaternion lastRotation;
    public Quaternion RotationDelta { get; private set; }

    private void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("MovingPlatform: Please assign pointA and pointB in the inspector.");
            enabled = false;
            return;
        }

        lastPosition = transform.position;
        lastRotation = transform.rotation;

        // Random starting height & direction so they don't all move in sync
        if (randomizeStartPhase)
        {
            float t = Random.value; // 0–1
            float startY = Mathf.Lerp(pointA.position.y, pointB.position.y, t);

            transform.position = new Vector3(
                transform.position.x,
                startY,
                transform.position.z
            );

            // Random starting direction
            movingToB = (Random.value > 0.5f);
        }
    }

    private void Update()
    {
        // Handle waiting at ends
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
                isWaiting = false;
        }
        else
        {
            // === Position movement (Y only) ===
            Vector3 target = movingToB ? pointB.position : pointA.position;

            float newY = Mathf.MoveTowards(
                transform.position.y,
                target.y,
                moveSpeed * Time.deltaTime
            );

            transform.position = new Vector3(
                transform.position.x,
                newY,
                transform.position.z
            );

            // Check if we've reached the target height (ignore X/Z)
            if (Mathf.Abs(transform.position.y - target.y) < 0.01f)
            {
                movingToB = !movingToB;

                if (useRandomWaitAtEnds)
                {
                    isWaiting = true;
                    waitTimer = Random.Range(waitTimeRange.x, waitTimeRange.y);
                }
            }

            // === Rotation ===
            if (rotationSpeed != Vector3.zero)
            {
                transform.Rotate(rotationSpeed * Time.deltaTime);
            }
        }

        // === Track deltas for syncing ===
        PlatformDelta = transform.position - lastPosition;
        RotationDelta = transform.rotation * Quaternion.Inverse(lastRotation);

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }
}
