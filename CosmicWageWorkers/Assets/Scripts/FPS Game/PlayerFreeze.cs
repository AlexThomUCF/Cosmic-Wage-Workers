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
    }

    void Update()
    {
        // Thaw over time if not hit recently
        if (timeSinceHit > freezeDecayDelay && currentFreeze > 0f)
        {
            currentFreeze -= thawRate * Time.deltaTime;
            currentFreeze = Mathf.Max(currentFreeze, 0f);
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
        currentFreeze += freezePerHit;
        currentFreeze = Mathf.Min(currentFreeze, maxFreeze);
        timeSinceHit = 0f; // reset thaw timer
    }
}

