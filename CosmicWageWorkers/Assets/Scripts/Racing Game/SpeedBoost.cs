using UnityEngine;
using System.Collections;

public class SpeedBoostPickup : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostMultiplier = 2f;     // how much stronger the boost is
    public float boostDuration = 3f;       // how long the boost lasts

    [Header("Respawn Settings")]
    public float respawnTime = 5f;         // seconds before pickup reappears

    private Renderer rend;
    private Collider col;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        SimpleCarController car = other.GetComponent<SimpleCarController>();
        if (car != null)
        {
            // Apply speed boost to car
            car.ActivateSpeedBoost(boostMultiplier, boostDuration);

            // Start respawn routine
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        // Hide and disable the pickup
        rend.enabled = false;
        col.enabled = false;

        // Wait for respawn time
        yield return new WaitForSeconds(respawnTime);

        // Reactivate and show the pickup
        rend.enabled = true;
        col.enabled = true;
    }
}
