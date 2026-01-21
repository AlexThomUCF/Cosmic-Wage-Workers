using UnityEngine;
using System.Collections;

public class SpeedBoostPickup : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostMultiplier = 1.5f;
    public float boostDuration = 3f;

    [Header("Respawn Settings")]
    public float respawnTime = 5f;

    [Header("Audio")]
    public AudioClip collectSound;        // Drag sound here
    public AudioSource audioSource;       // Drag an AudioSource here (optional)

    private Renderer rend;
    private Collider col;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();

        // Auto-assign an AudioSource if not set
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        VehicleGen4_Arcade car = other.GetComponent<VehicleGen4_Arcade>();
        if (car != null)
        {
            // Play sound on collect
            PlayCollectSound();

            // Apply the boost
            StartCoroutine(ApplySpeedBoost(car));

            // Handle pickup respawn
            StartCoroutine(Respawn());
        }
    }

    private void PlayCollectSound()
    {
        if (audioSource != null && collectSound != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
        else
        {
            Debug.LogWarning("Collect sound or AudioSource not assigned on " + name);
        }
    }

    private IEnumerator ApplySpeedBoost(VehicleGen4_Arcade car)
    {
        float originalTorque = car.maxMotorTorque;
        float originalSpeedCap = car.maxSpeedKph;

        car.maxMotorTorque *= boostMultiplier;
        car.maxSpeedKph *= boostMultiplier;

        yield return new WaitForSeconds(boostDuration);

        car.maxMotorTorque = originalTorque;
        car.maxSpeedKph = originalSpeedCap;
    }

    private IEnumerator Respawn()
    {
        rend.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        rend.enabled = true;
        col.enabled = true;
    }
}



