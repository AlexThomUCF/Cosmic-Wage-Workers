using UnityEngine;

public class CheckpointSystem : MonoBehaviour
{
    [SerializeField] private Transform[] checkpoints;
    private Vector3 startingPosition;
    private int lastCheckpointIndex = -1;
    private Rigidbody rb;

    private void Start()
    {
        startingPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Check if the collider is one of our checkpoints
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (collision.transform == checkpoints[i])
            {
                // Only update if this checkpoint is ahead of the last one reached
                if (i > lastCheckpointIndex)
                {
                    lastCheckpointIndex = i;
                }
                break;
            }
        }
    }

    public void RespawnAtLastCheckpoint()
    {
        Vector3 respawnPos;

        if (lastCheckpointIndex == -1)
        {
            respawnPos = startingPosition;
        }
        else
        {
            respawnPos = checkpoints[lastCheckpointIndex].position + Vector3.up * 2f;
        }

        transform.position = respawnPos;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}