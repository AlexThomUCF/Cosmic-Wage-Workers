using UnityEngine;

public class RandomPlatform : MonoBehaviour
{
    [SerializeField] private RandomPlatformGroup platformGroup;
    private bool isSafe;
    private Collider platformCollider;
    private Renderer platformRenderer;

    private void Start()
    {
        platformCollider = GetComponent<Collider>();
        platformRenderer = GetComponent<Renderer>();
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