using UnityEngine;

public class FallingItem : MonoBehaviour
{
    private Climbing player;
    private float floorY;
    public float fallSpeed = 12f;

    public void Initialize(Climbing playerClimbing, float groundY)
    {
        player = playerClimbing;
        floorY = groundY;

        // Ensure Rigidbody is set up
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        // Ensure collider is not a trigger
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = false;
    }

    void Update()
    {
        if (player != null && !player.isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // Hit floor
            if (transform.position.y <= floorY)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (player == null) return;

        if (other.CompareTag("Player"))
        {
            // Trigger player fall
            player.TriggerFall();
            Destroy(gameObject);
        }
    }
}