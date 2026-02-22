using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject generalHUD;          // Your HUD
    public Animator pauseAni;              // Animator on PausePanel
    public CanvasGroup pauseCanvasGroup;   // CanvasGroup on PausePanel

    private bool gameIsPaused;
    private bool isTransitioning;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
        {
            if (gameIsPaused)
                StartUnpause();
            else
                StartPause();
        }
    }

    // -------------------------
    // START PAUSE
    // -------------------------
    void StartPause()
    {
        isTransitioning = true;
        gameIsPaused = true;

        // Show the panel using CanvasGroup
        pauseCanvasGroup.alpha = 1f;
        pauseCanvasGroup.interactable = true;
        pauseCanvasGroup.blocksRaycasts = true;

        generalHUD.SetActive(false);

        pauseAni.SetTrigger("PauseOn");
    }

    // -------------------------
    // START UNPAUSE
    // -------------------------
    void StartUnpause()
    {
        isTransitioning = true;

        Time.timeScale = 1f; // Resume so animation plays correctly
        pauseAni.SetTrigger("PauseOff");
    }

    // -------------------------
    // CALLED BY RELAY (Animation Event)
    // -------------------------
    public void OnPauseAnimationFinished()
    {
        // Freeze time after pause animation
        Time.timeScale = 0f;
        isTransitioning = false;
    }

    public void OnUnpauseAnimationFinished()
    {
        // Hide the panel visually
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;

        generalHUD.SetActive(true);

        gameIsPaused = false;
        isTransitioning = false;
    }

    // -------------------------
    // RETURN TO MAIN MENU
    // -------------------------
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI");
    }
}