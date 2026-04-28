using UnityEngine;

public class KillZone : MonoBehaviour
{
    private CheckpointSystem checkpointSystem;

    private void Start()
    {
        checkpointSystem = Object.FindFirstObjectByType<CheckpointSystem>();
        //Debug.Log($"KillZone found CheckpointSystem: {checkpointSystem != null}");
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Debug.Log($"KillZone triggered by: {collision.gameObject.name}");
        
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Player hit killzone - respawning at last checkpoint");
            checkpointSystem.RespawnAtLastCheckpoint();
        }
    }
}