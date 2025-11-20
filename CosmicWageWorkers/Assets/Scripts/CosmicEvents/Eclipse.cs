using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Eclipse : MonoBehaviour
{
    [Header("Settings")]
    public float duration = 20f;
    public float speedMultiplier = 2f; // How much faster the player moves

    [Header("References")]
    public PlayerMovement playerMovement;
    public CosmicPhenomenonUIManager uiManager;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip startEndClip;

    private bool eventActive = false;

    public void TriggerEclipse()
    {
        if (eventActive || playerMovement == null) return;

        eventActive = true;

        // Show UI icon
        if (uiManager != null)
            uiManager.ShowEclipse(true);

        // Play start sound
        if (audioSource != null && startEndClip != null)
            audioSource.PlayOneShot(startEndClip);

        // Apply speed boost
        playerMovement.SetSpeedMultiplier(speedMultiplier);

        // Start timer
        StartCoroutine(EclipseRoutine());
    }

    private IEnumerator EclipseRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Reset player speed
        playerMovement.SetSpeedMultiplier(1f);

        // Play end sound
        if (audioSource != null && startEndClip != null)
            audioSource.PlayOneShot(startEndClip);

        // Hide UI icon
        if (uiManager != null)
            uiManager.ShowEclipse(false);

        eventActive = false;
    }
}