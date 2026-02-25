using UnityEngine;
using UnityEngine.Audio;

public class FallingItem : MonoBehaviour
{
    private Climbing player;
    private float floorY;
    public float fallSpeed = 12f;

    private PlayerAudio playerAudio;

    // New fields for spinning
    private Vector3 spinAxis;
    private float spinSpeed;

    void Awake()
    {
        playerAudio = FindFirstObjectByType<PlayerAudio>();
    }

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

        // Setup random spin
        spinAxis = Random.onUnitSphere; // random rotation axis
        spinSpeed = Random.Range(60f, 120f); // degrees per second
    }

    void Update()
    {
        if (player != null && !player.isFalling)
        {
            // Move downward
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;

            // Spin
            transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.World);

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
            playerAudio.PlayOneShot(playerAudio.hitByItem);

            // Trigger player fall
            player.TriggerFall();
            Destroy(gameObject);
        }
    }
}