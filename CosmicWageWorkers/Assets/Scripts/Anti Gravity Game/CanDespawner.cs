using UnityEngine;

public class CanDespawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Soup") || collision.name.Contains("Can"))
        {
            Destroy(collision.gameObject);
        }
    }
}
