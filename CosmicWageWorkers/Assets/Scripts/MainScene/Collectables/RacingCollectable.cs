using UnityEngine;

public class RacingCollectible : MonoBehaviour
{
    public string collectibleID;

    private void Start()
    {
        // Disable if already collected
        if (CollectibleManager.Instance != null &&
            CollectibleManager.Instance.IsCollected(collectibleID))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure it's the player touching it
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (CollectibleManager.Instance != null)
        {
            CollectibleManager.Instance.Collect(collectibleID);
        }

        gameObject.SetActive(false);
    }
}