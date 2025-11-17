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
    }

    private void Update()
    {
        // === Position movement ===
        Vector3 target = movingToB ? pointB.position : pointA.position;
        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingToB = !movingToB;
        }

        // === Rotation ===
        if (rotationSpeed != Vector3.zero)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }

        // === Track deltas for syncing ===
        PlatformDelta = transform.position - lastPosition;
        RotationDelta = transform.rotation * Quaternion.Inverse(lastRotation);

        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }
}
