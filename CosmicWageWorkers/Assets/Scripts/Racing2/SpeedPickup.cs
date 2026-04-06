using UnityEngine;
using System.Collections;

public class SpeedPickup : MonoBehaviour
{
    PlayerMovement player;

    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float duration = 5f;

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SpeedBoost());
        }
    }

    private IEnumerator SpeedBoost()
    {
        // Hide the object
        gameObject.SetActive(false);

        // Apply speed boost
        player.SetSpeedMultiplier(speedMultiplier);

        // Wait
        yield return new WaitForSeconds(duration);

        // Reset speed
        player.SetSpeedMultiplier(1f);

        // Destroy after effect ends
        Destroy(gameObject);
    }
}