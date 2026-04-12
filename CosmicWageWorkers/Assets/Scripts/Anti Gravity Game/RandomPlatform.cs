using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    [SerializeField] private RandomPlatformGroup platformGroup;
    [SerializeField] private float shakeIntensity = 0.02f;
    [SerializeField] private float shakeSpeed = 8f;
    
    private bool isSafe;
    private Collider platformCollider;
    private Renderer platformRenderer;
    private Vector3 startPosition;
    private Rigidbody platformRb;

    private void Start()
    {
        platformCollider = GetComponent<Collider>();
        platformRenderer = GetComponent<Renderer>();
        platformRb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        
        // Enable interpolation for smooth movement
        if (platformRb != null)
        {
            platformRb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }

    private void LateUpdate()
    {
        if (!isSafe)
        {
            ApplySubtleShake();
        }
        else
        {
            // Keep safe platform at original position
            transform.position = startPosition;
        }
    }

    private void ApplySubtleShake()
    {
        float shakeX = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
        float shakeZ = Mathf.Cos(Time.time * shakeSpeed * 0.7f) * shakeIntensity;
        
        transform.position = startPosition + new Vector3(shakeX, 0, shakeZ);
    }

    public void SetSafe(bool safe)
    {
        isSafe = safe;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isSafe)
            {
                DisappearPlatform(collision.gameObject);
            }
        }
    }

    private void DisappearPlatform(GameObject player)
    {
        if (platformCollider == null)
            platformCollider = GetComponent<Collider>();
        if (platformRenderer == null)
            platformRenderer = GetComponent<Renderer>();

        platformCollider.enabled = false;
        platformRenderer.enabled = false;
        
        // Force player to lose ground state
        PM playerMovement = player.GetComponent<PM>();
        if (playerMovement != null)
        {
            playerMovement.ForceAirborne();
        }
    }

    public void ReappearPlatform()
    {
        if (platformCollider == null)
            platformCollider = GetComponent<Collider>();
        if (platformRenderer == null)
            platformRenderer = GetComponent<Renderer>();

        platformCollider.enabled = true;
        platformRenderer.enabled = true;
    }
}