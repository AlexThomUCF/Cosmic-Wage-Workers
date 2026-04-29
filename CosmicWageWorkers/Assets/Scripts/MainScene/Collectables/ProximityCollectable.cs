using UnityEngine;

public class ProximityCollectible : MonoBehaviour
{
    public string collectibleID;
    public Transform player;
    public float collectDistance = 2f;

    private void Start()
    {
        // Disable if already collected
        if (CollectibleManager.Instance != null &&
            CollectibleManager.Instance.IsCollected(collectibleID))
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= collectDistance)
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
