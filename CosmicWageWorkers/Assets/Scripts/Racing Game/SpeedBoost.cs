using UnityEngine;
using System.Collections;

public class SpeedBoostPickup : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostMultiplier = 1.5f;     // how much stronger the boost is
    public float boostDuration = 3f;         // how long the boost lasts (seconds)

    [Header("Respawn Settings")]
    public float respawnTime = 5f;           // seconds before pickup reappears

    private Renderer rend;
    private Collider col;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        VehicleGen4 car = other.GetComponent<VehicleGen4>();
        if (car != null)
        {
            // Apply the boost
            StartCoroutine(ApplySpeedBoost(car));

            // Handle pickup respawn
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator ApplySpeedBoost(VehicleGen4 car)
    {
        // Save original stats
        float originalTorque = car.maxMotorTorque;
        float originalSpeedCap = car.maxSpeedKph;

        // Apply boost
        car.maxMotorTorque *= boostMultiplier;
        car.maxSpeedKph *= boostMultiplier;

        // Wait for duration
        yield return new WaitForSeconds(boostDuration);

        // Restore values
        car.maxMotorTorque = originalTorque;
        car.maxSpeedKph = originalSpeedCap;
    }

    private IEnumerator Respawn()
    {
        // Hide and disable pickup
        rend.enabled = false;
        col.enabled = false;

        // Wait before reappearing
        yield return new WaitForSeconds(respawnTime);

        // Reactivate pickup
        rend.enabled = true;
        col.enabled = true;
    }
}

