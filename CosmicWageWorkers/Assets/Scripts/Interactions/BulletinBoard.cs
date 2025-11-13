using UnityEngine;

public class BulletinBoard : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bulletinUI; // The parent UI object that holds cleanliness, stocking, and collectable info

    private bool isPlayerNearby;

    private void Start()
    {
        if (bulletinUI != null)
            bulletinUI.SetActive(false); // Hide UI at start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (bulletinUI != null)
                bulletinUI.SetActive(true); // Show UI when player approaches
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (bulletinUI != null)
                bulletinUI.SetActive(false); // Hide UI when player leaves
        }
    }
}

