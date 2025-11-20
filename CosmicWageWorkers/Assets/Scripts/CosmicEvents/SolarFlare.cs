using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SolarFlare : MonoBehaviour
{
    [Header("UI Settings")]
    public Image flashPanel;           // Fullscreen white panel (UI Image)
    public float fadeInDuration = 0.2f;  // Fast fade-in
    public float holdDuration = 3f;      // Fully white hold
    public float fadeOutDuration = 2f;   // Slow fade-out
    public AudioSource flashAudio;       // Optional sound
    public AudioClip flashSound;

    private bool isFlashing = false;

    /// <summary>
    /// Call this to trigger the solar flare flash.
    /// </summary>
    public void TriggerFlare()
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashRoutine());
        }
    }

    private IEnumerator FlashRoutine()
    {
        isFlashing = true;

        // Play sound
        if (flashAudio != null && flashSound != null)
            flashAudio.PlayOneShot(flashSound);

        // Ensure panel is visible
        flashPanel.gameObject.SetActive(true);

        // Fast fade-in
        yield return StartCoroutine(FadeAlpha(0f, 1f, fadeInDuration));

        // Hold fully white
        yield return new WaitForSeconds(holdDuration);

        // Slow fade-out
        yield return StartCoroutine(FadeAlpha(1f, 0f, fadeOutDuration));

        flashPanel.gameObject.SetActive(false);
        isFlashing = false;
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        float elapsed = 0f;
        Color c = flashPanel.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, elapsed / duration);
            flashPanel.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        flashPanel.color = new Color(c.r, c.g, c.b, to);
    }
}