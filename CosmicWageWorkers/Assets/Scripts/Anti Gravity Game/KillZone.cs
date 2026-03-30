using UnityEngine;

public class KillZone : MonoBehaviour
{
    private CheckpointSystem checkpointSystem;
    private RandomPlatformGroup[] platformGroups;

    private void Start()
    {
        checkpointSystem = FindObjectOfType<CheckpointSystem>();
        platformGroups = FindObjectsOfType<RandomPlatformGroup>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkpointSystem.RespawnAtLastCheckpoint();
            
            // Randomize all platform groups when player dies
            foreach (RandomPlatformGroup group in platformGroups)
            {
                group.RandomizeSelection();
            }
        }
    }
}