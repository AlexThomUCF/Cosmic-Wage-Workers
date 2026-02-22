using UnityEngine;

public class RespawnItem : MonoBehaviour
{
    public Transform spawnPoint;     // Optional custom spawn point
    public float fallThreshold = -10f;

    private Rigidbody rb;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Save initial position/rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // If no spawn point assigned, use initial position
        if (spawnPoint == null)
        {
            GameObject temp = new GameObject(name + "_SpawnPoint");
            temp.transform.position = initialPosition;
            temp.transform.rotation = initialRotation;
            spawnPoint = temp.transform;
        }
    }

    void Update()
    {
        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}