using UnityEngine;

public class SpawnStopTrigger : MonoBehaviour
{
    public FallingItemManager itemManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemManager.StopSpawning();
        }
    }
}
