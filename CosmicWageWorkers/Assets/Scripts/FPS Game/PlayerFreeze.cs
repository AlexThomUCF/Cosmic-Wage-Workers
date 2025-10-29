using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerFreeze : MonoBehaviour
{
    [Header("Freeze Settings")]
    public float maxFreeze = 100f;           // Max frozen value
    public float freezePerHit = 20f;         // How much each hit increases frozenness
    public float thawRate = 10f;             // How fast frozenness decreases per second
    public float freezeMoveMultiplier = 0.5f;// How much movement slows at max freeze
    public float freezeDecayDelay = 2f;      // Time after last hit before thawing starts

    [Header("UI")]
    public Slider freezeMeter;                // Slider showing current frozenness
    public Image freezeImage;

    private float currentFreeze = 0f;
    private float timeSinceHit = 0f;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        if (freezeMeter != null)
        {
            freezeMeter.maxValue = maxFreeze;
            freezeMeter.value = currentFreeze;
        }

        if (freezeImage != null)
        {
            var color = freezeImage.color;
            color.a = 0f; // Start fully transparent
            freezeImage.color = color;
        }
    }

    void Update()
    {
        // Thaw over time if not hit recently
        if (timeSinceHit > freezeDecayDelay && currentFreeze > 0f)
        {
            currentFreeze -= thawRate * Time.deltaTime;
            currentFreeze = Mathf.Max(currentFreeze, 0f);

        }

        // Update freeze overlay alpha based on freeze percentage
        float freezePercentImage = currentFreeze / maxFreeze;
        if (freezeImage != null)
        {
            var color = freezeImage.color;
            color.a = Mathf.Lerp(0f, 0.7f, freezePercentImage); // 0.7f = max alpha at full freeze
            freezeImage.color = color;
        }


        // Update movement speed based on freeze
        if (playerMovement != null)
        {
            float freezePercent = currentFreeze / maxFreeze;
            playerMovement.SetSpeedMultiplier(1f - freezePercent * freezeMoveMultiplier);
        }

        // Update UI meter
        if (freezeMeter != null)
        {
            freezeMeter.value = currentFreeze;
        }

        timeSinceHit += Time.deltaTime;

        // Fully frozen? Reload scene
        if (currentFreeze >= maxFreeze)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    // Call this when hit by a projectile
    public void FreezeHit()
    {
        currentFreeze += freezePerHit;// Add frost vfx camera, more you get hit frosty the screeen turns

        SoundEffectManager.Play("Freeze");
        currentFreeze = Mathf.Min(currentFreeze, maxFreeze);
        timeSinceHit = 0f; // reset thaw timer
    }
}

