using UnityEngine;

public class RollingCan : MonoBehaviour
{
    [Header("Path")]
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float rollSpeed = 5f;
    [SerializeField] private float rotationSpeed = 360f;

    private float distanceTraveled;
    private float totalRampDistance;
    private bool isMoving;
    private bool isFalling = true;
    private Vector3 spawnPosition;
    private Vector3 targetSpawnPoint;
    private float dropSpeed = 15f;

    private void Start()
    {
        spawnPosition = transform.position;

        // Set rotation to 90 degrees on Y axis
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }

    public void SetPath(Transform start, Transform end)
    {
        startPoint = start;
        endPoint = end;
        CalculateDistance();
    }

    public void SetSpawnPoint(Vector3 spawnPoint)
    {
        targetSpawnPoint = spawnPoint;
    }

    public void SetDropSpeed(float speed)
    {
        dropSpeed = speed;
    }

    private void CalculateDistance()
    {
        if (startPoint != null && endPoint != null)
        {
            totalRampDistance = Vector3.Distance(startPoint.position, endPoint.position);
        }
    }

    public void Initialize()
    {
        distanceTraveled = 0f;
        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            // Drop down quickly
            transform.position -= Vector3.up * dropSpeed * Time.fixedDeltaTime;

            // Check if can has reached spawn point
            if (transform.position.y <= targetSpawnPoint.y)
            {
                transform.position = targetSpawnPoint;
                isFalling = false;
            }
        }
        else if (isMoving && startPoint != null && endPoint != null && totalRampDistance > 0)
        {
            // Move along the ramp
            distanceTraveled += rollSpeed * Time.fixedDeltaTime;

            // Check if reached the end
            if (distanceTraveled >= totalRampDistance)
            {
                isMoving = false;
                return;
            }

            // Find position along ramp path
            float t = distanceTraveled / totalRampDistance;
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);

            // Rotate the can as it rolls
            transform.Rotate(0f, 0f, rotationSpeed * Time.fixedDeltaTime, Space.Self);
        }
    }
}
