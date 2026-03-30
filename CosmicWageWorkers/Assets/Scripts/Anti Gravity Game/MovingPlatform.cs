using UnityEngine;

public class AGMovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speedX = 0f;
    [SerializeField] private float speedY = 0f;
    [SerializeField] private float speedVarianceX = 0f;
    [SerializeField] private float speedVarianceY = 0f;
    [SerializeField] private float travelDistanceX = 5f;
    [SerializeField] private float travelDistanceY = 5f;

    private Vector3 startPosition;
    private Vector3 currentDirection = Vector3.one;
    private Transform playerTransform;
    private Rigidbody playerRb;
    private float actualSpeedX;
    private float actualSpeedY;

    private void Start()
    {
        startPosition = transform.position;
        RandomizeSpeed();
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void RandomizeSpeed()
    {
        actualSpeedX = speedX + Random.Range(-speedVarianceX, speedVarianceX);
        actualSpeedY = speedY + Random.Range(-speedVarianceY, speedVarianceY);
    }

    private void MovePlatform()
    {
        Vector3 movement = Vector3.zero;

        // Calculate X movement with looping
        if (actualSpeedX != 0)
        {
            movement.x = actualSpeedX * currentDirection.x * Time.fixedDeltaTime;
            float nextX = transform.position.x + movement.x;
            float distanceFromStart = Mathf.Abs(nextX - startPosition.x);

            // Reverse direction if we've exceeded travel distance
            if (distanceFromStart > travelDistanceX)
            {
                currentDirection.x *= -1f;
                movement.x = actualSpeedX * currentDirection.x * Time.fixedDeltaTime;
            }
        }

        // Calculate Y movement with looping
        if (actualSpeedY != 0)
        {
            movement.y = actualSpeedY * currentDirection.y * Time.fixedDeltaTime;
            float nextY = transform.position.y + movement.y;
            float distanceFromStart = Mathf.Abs(nextY - startPosition.y);

            // Reverse direction if we've exceeded travel distance
            if (distanceFromStart > travelDistanceY)
            {
                currentDirection.y *= -1f;
                movement.y = actualSpeedY * currentDirection.y * Time.fixedDeltaTime;
            }
        }

        transform.Translate(movement, Space.World);

        // Move player with platform if they're on it
        if (playerTransform != null)
        {
            playerTransform.Translate(movement, Space.World);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerRb = collision.rigidbody;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = null;
            playerRb = null;
        }
    }
}