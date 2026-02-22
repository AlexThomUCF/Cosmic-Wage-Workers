using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels & Animator")]
    public Animator pauseAni;               // Animator on PausePanel
    public CanvasGroup pauseCanvasGroup;    // Main pause panel
    public CanvasGroup pauseButtons;        // Buttons group
    public CanvasGroup optionsPanel;        // Options panel

    [Header("Other")]
    public GameObject generalHUD;           // HUD
    public GameObject ResumeButton;         // Default button for controller/keyboard

    private bool gameIsPaused = false;
    private bool isTransitioning = false;

    void Start()
    {
        Time.timeScale = 1f; // ensure game starts unpaused
        optionsPanel.alpha = 0f;
        optionsPanel.interactable = false;
        optionsPanel.blocksRaycasts = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
        {
            if (gameIsPaused) StartUnpause();
            else StartPause();
        }
    }

    // Start pausing
    void StartPause()
    {
        isTransitioning = true;
        gameIsPaused = true;

        // Show main pause panel
        pauseCanvasGroup.alpha = 1f;
        pauseCanvasGroup.interactable = true;
        pauseCanvasGroup.blocksRaycasts = true;

        // Show buttons panel, hide options
        pauseButtons.alpha = 1f;
        pauseButtons.interactable = true;
        pauseButtons.blocksRaycasts = true;

        optionsPanel.alpha = 0f;
        optionsPanel.interactable = false;
        optionsPanel.blocksRaycasts = false;

        generalHUD.SetActive(false);

        // Unlock cursor for UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseAni.SetTrigger("PauseOn");
        EventSystem.current.SetSelectedGameObject(ResumeButton);
    }

    // Start unpausing
    void StartUnpause()
    {
        isTransitioning = true;
        pauseAni.SetTrigger("PauseOff");
    }

    // Called at end of PauseOn animation
    public void OnPauseAnimationFinished()
    {
        Time.timeScale = 0f;
        isTransitioning = false;
    }

    // Called at end of PauseOff animation
    public void OnUnpauseAnimationFinished()
    {
        Time.timeScale = 1f;

        // Hide pause panel
        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;

        generalHUD.SetActive(true);

        // Lock cursor back for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameIsPaused = false;
        isTransitioning = false;
    }

    // Resume button
    public void ResumeGame()
    {
        if (!isTransitioning && gameIsPaused) StartUnpause();
    }

    // Open options panel
    public void OpenOptions()
    {
        if (isTransitioning) return;

        pauseButtons.alpha = 0f;
        pauseButtons.interactable = false;
        pauseButtons.blocksRaycasts = false;

        optionsPanel.alpha = 1f;
        optionsPanel.interactable = true;
        optionsPanel.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(null);
    }

    // Close options panel
    public void CloseOptions()
    {
        optionsPanel.alpha = 0f;
        optionsPanel.interactable = false;
        optionsPanel.blocksRaycasts = false;

        pauseButtons.alpha = 1f;
        pauseButtons.interactable = true;
        pauseButtons.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(ResumeButton);
    }

    // Main menu button
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI"); // Replace with your main menu scene name
    }
}